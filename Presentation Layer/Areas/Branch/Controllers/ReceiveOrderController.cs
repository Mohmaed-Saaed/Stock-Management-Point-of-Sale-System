using CoreLayer.Models;
using CoreLayer.Models.Operations;
using Humanizer;
using InfrastructureLayer;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.Branch.ViewModels;
using static CoreLayer.Models.Global;
namespace PresentationLayer.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize]
    public class ReceiveOrderController : Controller
    {

        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<ApplicationUser> _UserManager;


        public ReceiveOrderController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _UnitOfWork = unitOfWork;
            _UserManager = userManager;
        }

        [Authorize(Policy = "ReceiveOrder.View")]
        public async Task<IActionResult> Index(ReceiveOrdersWithFiltersVM vm)
        {
            if (vm.PageId < 1)
                return NotFound();

            var userId = _UserManager.GetUserId(User);
            ApplicationUser user = (await _UserManager.GetUserAsync(User))!;

            var ReceiveOrders = await _UnitOfWork.ReceiveOrders.GetAsyncIncludes(
            condition: r =>
            (user.BranchId == null || r.BranchId == user.BranchId)
            && (string.IsNullOrEmpty(vm.Search)
                || r.Supplier.Name.Contains(vm.Search))
            && (vm.BranchId == 0 || r.BranchId == vm.BranchId)
            && (vm.DateFilter == null || r.Date == vm.DateFilter)
            && (vm.DiscountRateFilter == null || r.TotalDiscountRate >= vm.DiscountRateFilter)
            && (vm.TaxRateFilter == null || r.TotalTaxesRate >= vm.TaxRateFilter)
            && (vm.GrandTotalFilter == null || r.GrandTotal >= vm.GrandTotalFilter),
            new List<Func<IQueryable<CoreLayer.Models.Operations.ReceiveOrder>, IQueryable<CoreLayer.Models.Operations.ReceiveOrder>>>
            {
            s => s.Include(s => s.Branch),
            s => s.Include(s => s.Supplier),
            s => s.Include(s => s.ApplicationUser)
            });

            vm.Branches = (await _UnitOfWork.Branches.GetAsync()).Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name });

            int totalPages = 0;
            if (ReceiveOrders.Count != 0)
            {
                // Sorting By...
                switch (vm.SortBy)
                {
                    case "date_asc":
                        ReceiveOrders = ReceiveOrders.OrderBy(d => d.Date).OrderBy(d => d.Time).ToList();
                        break;
                    case "date_desc":
                        ReceiveOrders = ReceiveOrders.OrderByDescending(d => d.Date).OrderByDescending(d => d.Time).ToList();
                        break;
                    case "tax_asc":
                        ReceiveOrders = ReceiveOrders.OrderBy(d => d.TotalTaxesRate).ToList();
                        break;
                    case "tax_desc":
                        ReceiveOrders = ReceiveOrders.OrderByDescending(d => d.TotalTaxesRate).ToList();
                        break;
                    case "disc_asc":
                        ReceiveOrders = ReceiveOrders.OrderBy(d => d.TotalDiscountRate).ToList();
                        break;
                    case "disc_desc":
                        ReceiveOrders = ReceiveOrders.OrderByDescending(d => d.TotalDiscountRate).ToList();
                        break;
                    case "grand_asc":
                        ReceiveOrders = ReceiveOrders.OrderBy(d => d.GrandTotal).ToList();
                        break;
                    case "grand_desc":
                        ReceiveOrders = ReceiveOrders.OrderByDescending(d => d.GrandTotal).ToList();
                        break;
                    default:
                        //If no 'SortBy' is provided
                        ReceiveOrders = ReceiveOrders.OrderByDescending(d => d.Id).ToList();
                        break;
                }

                // Pagination
                const int itemsInPage = 6;
                totalPages = (int)Math.Ceiling(ReceiveOrders.Count / (double)itemsInPage);
                if (vm.PageId > totalPages)
                    return NotFound();
                vm.ReceiveOrders = ReceiveOrders.Skip((vm.PageId - 1) * itemsInPage).Take(itemsInPage).ToList();
            }

            vm.NoPages = totalPages;

            return View(vm);
        }
        [HttpGet]
        [Authorize(Policy = "ReceiveOrder.Add|ReceiveOrder.Edit")]
        public async Task<IActionResult> Save(int? Id)
        {
            var user = _UserManager.GetUserAsync(User).GetAwaiter().GetResult();
            var suplier = _UnitOfWork.Partners.GetAsync(
                s => s.partnerType == Partner.PartnerType.Supplier)
                .GetAwaiter().GetResult()
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString()
                }).ToList();
            var branches = _UnitOfWork.Branches
                .GetAsync(b => user.BranchId == null || b.Id == user.BranchId)
                .GetAwaiter().GetResult()
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString()
                }).ToList();
            var taxes = _UnitOfWork.Taxes
                .GetAsync()
                .GetAwaiter().GetResult()
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.Rate.ToString()
                }).ToList();

            var ReceiveOrderVM = new ReceiveOrderVM()
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                SupplierList = suplier,
                BranchList = branches,
                TaxList = taxes,

                ApplicationUserId = _UserManager.GetUserAsync(User).Result?.Id ?? ""

            };

            if (Id is not null)
            {
                var ReceiveOrder = await _UnitOfWork.ReceiveOrders.GetOneAsync(x => x.Id == Id);

                if (ReceiveOrder is not null)
                {
                    ReceiveOrderVM.ReveiveOrderId = ReceiveOrder.Id;
                    ReceiveOrderVM.Date = ReceiveOrder.Date;
                    ReceiveOrderVM.BranchId = ReceiveOrder.BranchId;
                    ReceiveOrderVM.SupplierId = ReceiveOrder.SupplierId;
                    ReceiveOrderVM.TaxId = ReceiveOrder.TotalTaxesRate;
                    ReceiveOrderVM.TotalDiscountAmount = ReceiveOrder.TotalDiscountAmount;
                    ReceiveOrderVM.DiscountPercentage = ReceiveOrder.TotalDiscountRate;
                    ReceiveOrderVM.GrandTotal = ReceiveOrder.GrandTotal;
                    ReceiveOrderVM.TotalAmount = ReceiveOrder.TotalAmount;
                    ReceiveOrderVM.TotalQuantity = ReceiveOrder.TotalQuantity;
                    ReceiveOrderVM.TotalTaxesAmount = ReceiveOrder.TotalTaxesAmount;
                    ReceiveOrderVM.TotalDiscountRate = ReceiveOrder.TotalDiscountRate;
                    ReceiveOrderVM.TotalTaxesRate = ReceiveOrder.TotalTaxesRate;
                    ReceiveOrderVM.Status = ReceiveOrder.status;
                    ReceiveOrderVM.Code = ReceiveOrder.Code ?? "";

                    var oprationItems = await _UnitOfWork.OperationItems.GetAsync(o => o.OperationId == ReceiveOrder.Id, [o => o.Item]);

                    if (oprationItems.Count() > 0)
                    {
                        ReceiveOrderVM.OperationItems = oprationItems;

                    }

                    ViewBag.IsConfirmed = ReceiveOrder.status;

                    return View(ReceiveOrderVM);
                }


            }

            return View(ReceiveOrderVM);
        }

        [HttpPost]
        [Authorize(Policy = "ReceiveOrder.Confirm")]
        public async Task<IActionResult> Confirm(int receiveOrderId)
        {
            var receiveOrder = await _UnitOfWork.ReceiveOrders.GetOneAsync(r => r.Id == receiveOrderId,
                include: [r => r.OperationItems], tracked: true);

            if (receiveOrder is not null)
            {
                foreach (var item in receiveOrder.OperationItems)
                {
                    var branchItem = await _UnitOfWork.BranchItems
                        .GetOneAsync(b => b.BranchId == receiveOrder.BranchId && b.ItemId == item.ItemId, tracked: true);

                    if (branchItem == null)
                        return BadRequest(new { error = $"BranchItem not found for ItemId={item.ItemId}" });

                    var oldQty = branchItem.Quantity;
                    var oldAvg = branchItem.BuyingPriceAvg;

                    var newQty = item.Quantity;
                    var newPrice = item.BuyingPrice;

                    branchItem.BuyingPriceAvg = ((oldQty * oldAvg) + (newQty * newPrice)) / (oldQty + newQty);

                    branchItem.Quantity = oldQty + newQty;

                    branchItem.LastBuyingPrice = newPrice;
                }

                var resultBranchItem = await _UnitOfWork.BranchItems.CommitAsync();

                if (!resultBranchItem)
                    return BadRequest(new { error = "Error updating branch items" });


                receiveOrder.status = Status.Approved;
                receiveOrder.Code = _UnitOfWork.ReceiveOrders.GenerateCode(receiveOrder.BranchId);
                await _UnitOfWork.ReceiveOrders.CommitAsync();

                TempData["success"] = "Receive order Confirmed successfully";

                return RedirectToAction(nameof(Save), new { id = receiveOrder.Id });
            }

            TempData["success"] = "Bad action";
            return RedirectToAction(nameof(Save));
        }




        [HttpPost]
        [Authorize(Policy = "ReceiveOrder.Add|ReceiveOrder.Edit")]
        public async Task<IActionResult> Save(ReceiveOrderVM receiveOrderVM)
        {
            if (receiveOrderVM.ReveiveOrderId == null)
            {

                var receiveOrder = new ReceiveOrder
                {
                    BranchId = receiveOrderVM.BranchId,
                    SupplierId = receiveOrderVM.SupplierId,
                    GrandTotal = receiveOrderVM.GrandTotal,
                    TotalDiscountRate = receiveOrderVM.TotalDiscountRate,
                    Date = receiveOrderVM.Date,
                    Time = TimeOnly.FromDateTime(DateTime.Now), // Wrong approach
                    status = Status.Draft,
                    TotalQuantity = receiveOrderVM.TotalQuantity,
                    TotalTaxesRate = receiveOrderVM.TotalTaxesRate,
                    TotalAmount = receiveOrderVM.TotalAmount,
                    TotalDiscountAmount = receiveOrderVM.TotalDiscountAmount,
                    TotalTaxesAmount = receiveOrderVM.TotalTaxesAmount,
                    RoundedGrandTotal = (int)Math.Ceiling(receiveOrderVM.GrandTotal),
                    ApplicationUserId = receiveOrderVM.ApplicationUserId,
                    //Code = receiveOrderVM.Code,
                    OperationItems = receiveOrderVM.OperationItems.Select(o => new OperationItem
                    {

                        ItemId = o.ItemId,
                        Quantity = o.Quantity,
                        BuyingPrice = o.BuyingPrice,
                        DiscountRate = o.DiscountRate,
                        TotalPrice = o.TotalPrice,
                    }).ToList()
                };
                var result = await _UnitOfWork.ReceiveOrders.CreateAsync(receiveOrder);

                if (!result)
                    return BadRequest(new { error = "Error creating receive order" });

                TempData["success"] = "Receive order created successfully";

                return Ok(new { status = true });
            }


            var receiceOrder = await _UnitOfWork.ReceiveOrders.GetOneAsync(r => r.Id == receiveOrderVM.ReveiveOrderId
            , tracked: true);

            if (receiceOrder == null)
                return BadRequest(new { status = false, error = "No receive order to edit" });



            receiceOrder.Date = receiveOrderVM.Date;
            receiceOrder.BranchId = receiveOrderVM.BranchId;
            receiceOrder.SupplierId = receiveOrderVM.SupplierId;
            receiceOrder.TotalTaxesRate = receiveOrderVM.TotalTaxesRate;
            receiceOrder.TotalDiscountAmount = receiveOrderVM.TotalDiscountAmount;
            receiceOrder.TotalDiscountRate = receiveOrderVM.TotalDiscountRate;
            receiceOrder.GrandTotal = receiveOrderVM.GrandTotal;
            receiceOrder.TotalAmount = receiveOrderVM.TotalAmount;
            receiceOrder.TotalQuantity = receiveOrderVM.TotalQuantity;
            receiceOrder.TotalTaxesAmount = receiveOrderVM.TotalTaxesAmount;
            receiceOrder.TotalDiscountRate = receiveOrderVM.TotalDiscountRate;
            receiceOrder.TotalTaxesRate = receiveOrderVM.TotalTaxesRate;
            //receiceOrder.Code = 
            var resultReceiceOrder = await _UnitOfWork.ReceiveOrders.CommitAsync();

            var EditedList = new List<OperationItem>();


            foreach (var item in receiveOrderVM.OperationItems)
            {
                var oprationItem = await _UnitOfWork.OperationItems.GetOneAsync(o => o.Id == item.Id, tracked: true);

                if (oprationItem != null)
                {
                    oprationItem.ItemId = item.ItemId;
                    oprationItem.Quantity = item.Quantity;
                    oprationItem.DiscountRate = item.DiscountRate;
                    oprationItem.BuyingPrice = item.BuyingPrice;
                    oprationItem.TotalPrice = item.TotalPrice;
                    EditedList.Add(item);
                }
                else
                {
                    item.OperationId = receiceOrder.Id;
                    await _UnitOfWork.OperationItems.CreateAsync(item);
                }
            }



            var oldOperationItemCount = (await _UnitOfWork.OperationItems.GetAsync(o => o.OperationId == receiceOrder.Id)).ToList();



            if (oldOperationItemCount.Count() > receiceOrder.OperationItems.Count)
            {
                var removedOperationList = oldOperationItemCount.Where(o => EditedList.Any(e => o.Id != e.Id)).ToList();


                await _UnitOfWork.OperationItems.DeleteRangeAsync(removedOperationList);

            }

            var resultOprationIrem = await _UnitOfWork.OperationItems.CommitAsync();


            if (!resultReceiceOrder || !resultOprationIrem)
                return BadRequest(new { status = false, error = "No receive order to edit" });


            return Ok(new { status = true });
        }

        public IActionResult RenderEmptyRow()
        {
            return PartialView("Details", new OperationItem());
        }
        [HttpGet]
        public IActionResult GetItems()
        {
            var items = _UnitOfWork.Items.GetAsync().GetAwaiter().GetResult()
                .Select(i => new { Id = i.Id, Name = i.Name })
                .ToList();

            return Json(items);
        }
        [HttpPost]
        [Authorize(Policy = "ReceiveOrder.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var ReceiveOrder = await _UnitOfWork.ReceiveOrders.GetOneAsync(b => b.Id == id);

            if (ReceiveOrder is not null)
            {

                var ReceiveOrderResult = await _UnitOfWork.ReceiveOrders.DeleteAsync(ReceiveOrder);

                if (ReceiveOrderResult)
                {
                    TempData["success"] = "Receive Order Deleted Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Deleting Receive Order";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Receive Order Not Found";
            return RedirectToAction(nameof(Index));
        }
    }
}
