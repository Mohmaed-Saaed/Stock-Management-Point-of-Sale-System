using CoreLayer.Models;
using CoreLayer.Models.Operations;
using Humanizer;
using InfrastructureLayer;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Utility;
using QuestPDF.Companion;
using QuestPDF.Infrastructure;
using static CoreLayer.Models.Global;

namespace PresentationLayer.Areas.Sales.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfPreviewController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _env;

        public PdfPreviewController(IUnitOfWork UnitOfWork, IWebHostEnvironment env)
        {
            _UnitOfWork = UnitOfWork;
            _env = env;
        }

        // GET: /api/pdfpreview/invoice/11
        [HttpGet("invoice/{id}")]
        public async Task<IActionResult> PreviewInvoice(int id)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            if (!_env.IsDevelopment())
                return Forbid("PDF Companion preview is allowed only in Development.");

            var invoice = await _UnitOfWork.SalesInvoices.GetOneAsyncIncludes(s => s.Id == 11,
                new List<Func<IQueryable<SalesInvoice>, IQueryable<SalesInvoice>>>
                {
                    s=>s.Include(s=>s.Branch),
                    s=>s.Include(s=>s.RetailCustomer),
                    s=>s.Include(s=>s.ApplicationUser),
                    s=>s.Include(s=>s.DiscountSalesInvoices).ThenInclude(s=>s.Discount),
                    s=>s.Include(s=>s.OperationItems).ThenInclude(s=>s.Item)
                },
                false);

            if (invoice == null)
                return NotFound("Invoice not found");

            var document = new InvoiceDocument(invoice);

            // Open in QuestPDF Companion (desktop app must be running)
            document.ShowInCompanion();

            // Optionally return a simple JSON confirmation
            return Ok(new { message = $"Invoice {id} sent to QuestPDF Companion." });
        }

        // GET: /api/pdfpreview/CreateReceiveOrder
        [HttpGet("CreateReceiveOrder")]
        public async Task<IActionResult> CreateReceiveOrder()
        {
            var order = new ReceiveOrder() {
            BranchId=1,
            SupplierId=1,
            ApplicationUserId = "4032579c-7f7b-4914-acea-aa37a6a8aaed",
            Date = DateOnly.FromDateTime(DateTime.Now),
            Time = TimeOnly.FromDateTime(DateTime.Now),
            status = Status.Approved,
            TotalQuantity = 1,
            TotalAmount = 1,
            TotalDiscountRate = 1,
            TotalDiscountAmount = 1,
            GrandTotal = 1,
            RoundedGrandTotal = 1
            };
            var result = await _UnitOfWork.ReceiveOrders.CreateAsync(order);

            return Ok(new { message = $"Order {order.Id}" });
        }
    }
}
