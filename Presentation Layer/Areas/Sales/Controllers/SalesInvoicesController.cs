using CoreLayer.Models;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.Sales.ViewModels;

namespace PresentationLayer.Areas.Sales.Controllers
{
    [Area("Sales")]
    [Authorize]
    public class SalesInvoicesController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<ApplicationUser> _UserManager;
        public SalesInvoicesController(IUnitOfWork UnitOfWork, UserManager<ApplicationUser> userManager)
        {
            _UnitOfWork = UnitOfWork;
            _UserManager = userManager;
        }
        [Authorize(Policy = "SalesInvoice.View")]
        public async Task<IActionResult> Index(SalesInvoicesVM vm)
        {
            if (vm.PageId < 1)
                return NotFound();

            ApplicationUser user = (await _UserManager.GetUserAsync(User))!;

            // Fetching with Search & Filters
            var invoices = await _UnitOfWork.SalesInvoices.GetAsyncIncludes(
                condition: s =>
                (user.BranchId == null || s.BranchId==user.BranchId)
                &&
                (string.IsNullOrEmpty(vm.Search)
                || s.Code!.Contains(vm.Search)
                || (!string.IsNullOrEmpty(s.ApplicationUser.UserName) && s.ApplicationUser.UserName.Contains(vm.Search))
                || s.RetailCustomer.Name.Contains(vm.Search))
                && (vm.BranchId == 0            || s.BranchId == vm.BranchId) 
                && (vm.DateFilter == null       || s.Date == vm.DateFilter) 
                && (vm.TotalQtyFilter == null   || s.TotalQuantity >= vm.TotalQtyFilter)
                && (vm.GrandTotalFilter == null || s.RoundedGrandTotal >= vm.GrandTotalFilter),
                new List<Func<IQueryable<CoreLayer.Models.Operations.SalesInvoice>, IQueryable<CoreLayer.Models.Operations.SalesInvoice>>>
                {
                    s => s.Include(s => s.Branch),
                    s => s.Include(s => s.RetailCustomer),
                    s => s.Include(s => s.ApplicationUser)
                }, false);

            vm.Branches = (await _UnitOfWork.Branches.GetAsync()).Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.Name });

            int totalPages = 0;
            if (invoices.Count != 0)
            {
                // Sorting By...
                switch (vm.SortBy)
                {
                    case "date_asc":
                        invoices = invoices.OrderBy(d => d.Date).OrderBy(d=>d.Time).ToList();
                        break;
                    case "date_desc":
                        invoices = invoices.OrderByDescending(d => d.Date).OrderByDescending(d=>d.Time).ToList();
                        break;
                    case "qty_asc":
                        invoices = invoices.OrderBy(d => d.TotalQuantity).ToList();
                        break;
                    case "qty_desc":
                        invoices = invoices.OrderByDescending(d => d.TotalQuantity).ToList();
                        break;
                    case "grand_asc":
                        invoices = invoices.OrderBy(d => d.RoundedGrandTotal).ToList();
                        break;
                    case "grand_desc":
                        invoices = invoices.OrderByDescending(d => d.RoundedGrandTotal).ToList();
                        break;
                    default:
                        //If no 'SortBy' is provided
                        invoices = invoices.OrderBy(d => d.Id).ToList();
                        break;
                }

                // Pagination
                const int itemsInPage = 6;
                totalPages = (int)Math.Ceiling(invoices.Count / (double)itemsInPage);
                if (vm.PageId > totalPages)
                    return NotFound();
                vm.SalesInvoices = invoices.Skip((vm.PageId - 1) * itemsInPage).Take(itemsInPage).ToList();
            }

            vm.NoPages = totalPages;
            return View(vm);
        }

        [Authorize(Policy = "SalesInvoice.Print")]
        public IActionResult GetReceipt(int id)
        {
            // just redirect to the API endpoint
            return Redirect($"/api/Sales/PosApi/receipt?operationId={id}");
        }
    }
}
