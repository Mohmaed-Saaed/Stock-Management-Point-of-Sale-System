using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using CoreLayer.Models.Operations;
using InfrastructureLayer;
using InfrastructureLayer.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.administrative.ViewModels;
using PresentationLayer.Areas.Branch.ViewModels;
using PresentationLayer.Areas.Sales.DTOs;
using PresentationLayer.Utility;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Threading.Tasks;
using static CoreLayer.Models.Global;


namespace PresentationLayer.Areas.Sales.Controllers
{
    [Area("Sales")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Policy = "POS")]
    public class PosApiController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public PosApiController(IUnitOfWork UnitOfWork, UserManager<ApplicationUser> userManager)
        {
            _UnitOfWork = UnitOfWork;
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        // GET: /api/Sales/PosApi/user
        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            ApplicationUser user = (await _userManager.GetUserAsync(User))!;
            if (user is not null)
            {
                var result =
                    new
                    {
                        id = user.Id,
                        name = user.UserName
                    };
                return Ok(result);
            }
            else
                return NotFound();
        }


        // GET: /api/Sales/PosApi/branches
        [HttpGet("branches")]
        public async Task<IActionResult> GetBranches()
        {
            try
            {
                ApplicationUser user = (await _userManager.GetUserAsync(User))!;
                var branches = await _UnitOfWork.Branches.GetAsync();

                if (user.BranchId is not null && branches.Find(b => b.Id == user.BranchId) is CoreLayer.Models.Branch branch)
                {
                    // The user has an assigned branch
                    var result = new[]
                    {
                        new
                        {
                            id = branch.Id,
                            name = branch.Name
                        }
                    };
                    return Ok(result);
                }
                else
                {
                    // Branch to be selected in the page
                    var result = branches.Select(b => new
                    {
                        id = b.Id,
                        name = b.Name
                    }).ToArray();
                    return Ok(result);
                }
            }
            catch
            {
                // Error Log
                TempData["Error"] = "Error fetching branch data";
                return StatusCode(500, new { message = "An error occurred while fetching branch data" });
            }
        }


        // GET: /api/Sales/PosApi/itemtypes
        [HttpGet("itemtypes")]
        public async Task<IActionResult> GetItemTypes()
        {
            try
            {
                var leafItemTypes = await _UnitOfWork.ItemTypes.GetLeafNodesAsync();
                if (leafItemTypes is null)
                {
                    TempData["Error"] = "Error fetching Item types from Db";
                    return StatusCode(500, new { message = "An error occurred while fetching item types" });
                }
                // Transform the data to match the expected format at pos.js
                var result = leafItemTypes.Select(t => new
                {
                    id = t.Id,
                    name = t.Name
                }).ToArray();

                return Ok(result);
            }
            catch
            {
                // Error Log
                TempData["Error"] = "Error fetching Item types";
                return StatusCode(500, new { message = "An error occurred while fetching item types" });
            }
        }


        // GET: /api/Sales/PosApi/items?typeId?branchId
        [HttpGet("items")]
        public async Task<IActionResult> GetItems([FromQuery] int typeId, [FromQuery] int branchId)
        {
            try
            {
                ApplicationUser user = (await _userManager.GetUserAsync(User))!;

                // Use Include and ThenInclude to load related data
                var branchItems = await _UnitOfWork.BranchItems.GetAsyncIncludes(
                b => b.BranchId == branchId && b.Quantity > 0,
                new List<Func<IQueryable<BranchItem>, IQueryable<BranchItem>>>
                {
                    b=>b.Include(b => b.Item).ThenInclude(b=>b.ItemType)
                }, false);

                if (branchItems is null)
                {
                    TempData["Error"] = "Error fetching items from Db";
                    return StatusCode(500, new { message = "An error occurred while fetching items" });
                }

                var result = branchItems.Select(i => new
                {
                    id = i.ItemId,
                    name = i.Item.Name,
                    type = i.Item.ItemType.Name,
                    typeId = i.Item.ItemTypeId,
                    barcode = i.Item.Barcode,
                    quantity = i.Quantity,
                    price = i.SellingPrice,
                    discountPrice = (i.DiscountRate == 0) ? i.SellingPrice : i.SellingPrice - i.SellingPrice * i.DiscountRate / 100.0,
                    discountRate = i.DiscountRate,
                    imageName = i.Item.Image
                });

                // All Items Button
                if (typeId == 0)
                    return Ok(result.ToArray());
                else
                {
                    result = result.Where(b => b.typeId == typeId);
                    return Ok(result.ToArray());
                }
            }
            catch
            {
                TempData["Error"] = "Error fetching items";
                return StatusCode(500, new { message = "An error occurred while fetching items" });
            }
        }


        // GET: /api/Sales/PosApi/customers
        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var customers = await _UnitOfWork.Partners.GetAsync(p => p.partnerType == Partner.PartnerType.RetailCustomer);

                var result = customers.Select(b => new
                {
                    id = b.Id,
                    name = b.Name
                }).ToArray();
                return Ok(result);
            }
            catch
            {
                // Error Log
                TempData["Error"] = "Error fetching customers";
                return StatusCode(500, new { message = "An error occurred while fetching customers" });
            }
        }


        // GET: /api/Sales/PosApi/discounts
        [HttpGet("discounts")]
        public async Task<IActionResult> GetDiscounts()
        {
            try
            {
                var activeDiscounts = (await _UnitOfWork.Discounts.GetActiveDiscountsAsync()).OrderByDescending(d => d.Rate).Take(1);
                if (activeDiscounts is null)
                {
                    TempData["Error"] = "Error fetching Discounts from Db";
                    return StatusCode(500, new { message = "An Db error occurred while fetching discounts" });
                }

                // Transform the data to match the expected format at pos.js
                var result = activeDiscounts.Select(t => new
                {
                    id = t.Id,
                    name = t.Name,
                    rate = t.Rate
                }).ToArray();
                return Ok(result);

            }
            catch
            {
                // Error Log
                TempData["Error"] = "Error fetching Discounts";
                return StatusCode(500, new { message = "An error occurred while fetching discounts" });
            }
        }


        // POST: /api/Sales/PosApi/createSalesInvoice
        [HttpPost("createSalesInvoice")]
        public async Task<IActionResult> CreateSalesInvoice([FromBody] CreateSalesInvoiceDTO dto)
        {
            if (dto == null)
                return BadRequest("No invoice received");

            var invoice = new SalesInvoice
            {
                BranchId = dto.BranchId,
                RetailCustomerId = dto.RetailCustomerId,
                ApplicationUserId = dto.ApplicationUserId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                status = Status.Approved,
                TotalQuantity = dto.TotalQuantity,
                TotalAmount = dto.TotalAmount,
                TotalDiscountRate = dto.TotalDiscountRate,
                TotalDiscountAmount = dto.TotalDiscountAmount,
                GrandTotal = dto.GrandTotal,
                RoundedGrandTotal = dto.RoundedGrandTotal,
                Code = "1",
                OperationItems = dto.OperationItems.Select(o => new OperationItem
                {
                    ItemId = o.ItemId,
                    Quantity = o.Quantity,
                    SellingPrice = o.SellingPrice,
                    DiscountRate = o.DiscountRate,
                    TotalPrice = o.DiscountPrice * o.Quantity,
                }).ToList(),
                PaidCash = dto.PaidCash,
                Change = dto.Change
            };

            var created = await _UnitOfWork.SalesInvoices.CreateAsync(invoice);
            if (!created)
                return BadRequest("Error creating Invoice");

            foreach (var item in invoice.OperationItems)
                item.OperationId = invoice.Id;

            // Apply general discounts
            var discountSalesInvoices = dto.GeneralDiscounts
                .Select(d => new DiscountSalesInvoice { DiscountId = d, OperationId = invoice.Id })
                .ToList();
            foreach (var discount in discountSalesInvoices)
            {
                if (!(await _UnitOfWork.Discounts.IncrementDiscountsUses(discount.DiscountId)))
                    return BadRequest($"Error Incrementing Uses of Discount No.{discount.DiscountId}");
            }
            invoice.DiscountSalesInvoices = discountSalesInvoices;

            invoice.Code = _UnitOfWork.SalesInvoices.GenerateCode(invoice.BranchId);

            var updated = await _UnitOfWork.SalesInvoices.UpdateAsync(invoice);
            if (!updated)
                return BadRequest("Error updating Invoice");

            // Subtract stock quantities - do all and fail if any fail
            foreach (var item in invoice.OperationItems)
            {
                var branchItem = await _UnitOfWork.BranchItems
                    .GetOneAsync(bi => bi.BranchId == invoice.BranchId && bi.ItemId == item.ItemId, null, false);

                if (branchItem == null)
                    return BadRequest($"BranchItem not found for ItemId={item.ItemId}");

                branchItem.Quantity -= item.Quantity;
                var branchUpdateOk = await _UnitOfWork.BranchItems.UpdateAsync(branchItem);
                if (!branchUpdateOk)
                    return BadRequest("Error subtracting quantity from branch");
            }

            // Add The Sales Invoice to each BranchItem's (BranchItemSalesInvoices) List
            await _UnitOfWork.SalesInvoices.AddBranchItemTrackingAsync(invoice);

            // Create Sales Invoice Pdf Receipt
            var pdfUrl = $"/api/Sales/PosApi/receipt?operationId={invoice.Id}";

            // Return invoice id and pdf url to client
            return Ok(new { invoiceId = invoice.Id });
        }


        // GET: /api/Sales/PosApi/receipt?operationId
        [HttpGet("receipt")]
        public async Task<IActionResult> GetReceipt([FromQuery] int operationId)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var invoice = await _UnitOfWork.SalesInvoices.GetOneAsyncIncludes(
                s => s.Id == operationId,
                new List<Func<IQueryable<SalesInvoice>, IQueryable<SalesInvoice>>>
                {
                    s => s.Include(s => s.Branch),
                    s => s.Include(s => s.RetailCustomer),
                    s => s.Include(s => s.ApplicationUser),
                    s => s.Include(s => s.DiscountSalesInvoices).ThenInclude(s => s.Discount),
                    s => s.Include(s => s.OperationItems).ThenInclude(s => s.Item)
                },
                false
            );

            if (invoice == null)
                return NotFound("Invoice not found");

            var document = new InvoiceDocument(invoice);
            var pdfBytes = document.GeneratePdf();

            // Add inline disposition so browser tries to open instead of download
            Response.Headers.Add("Content-Disposition", $"inline; filename={invoice.Code}.pdf");

            return File(pdfBytes, "application/pdf");
        }
    }
}
