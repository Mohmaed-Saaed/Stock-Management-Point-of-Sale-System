using CoreLayer;
using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Areas.administrative.ViewModels;
using PresentationLayer.Areas.Administrative.ViewModels;
using PresentationLayer.Areas.Branch.ViewModels;
using System.Globalization;
using System.Linq;

namespace PresentationLayer.Areas.administrative.Controllers
{
    [Area("administrative")]
    [Authorize]
    public class DiscountController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public DiscountController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Policy = "Discount.View")]
        public async Task<IActionResult> Index(DiscountsWithFiltersVM vm)
        {
            if (vm.PageId < 1)
                return NotFound();

            List<Discount> discounts = await _UnitOfWork.Discounts.GetAsync(d =>
            (string.IsNullOrEmpty(vm.Search)
            || d.Name.Contains(vm.Search))
            && (vm.MinRate == null || d.Rate >= vm.MinRate)
            && (vm.IsActiveFilter == null || d.IsActive == vm.IsActiveFilter)
            && (vm.ExpDateFilter == null || d.ExpirationDate == vm.ExpDateFilter)
            && (vm.MinRate == null || d.CurrentUses >= vm.MinRate)
            );

            int totalPages = 0;
            if (discounts.Count != 0)
            {
                // Sorting By...
                switch (vm.SortBy)
                {
                    case "rate_asc":
                        discounts = discounts.OrderBy(d => d.Rate).ToList();
                        break;
                    case "rate_desc":
                        discounts = discounts.OrderByDescending(d => d.Rate).ToList();
                        break;
                    case "exp_date_asc":
                        discounts = discounts.OrderBy(d => d.ExpirationDate).ToList();
                        break;
                    case "exp_date_desc":
                        discounts = discounts.OrderByDescending(d => d.ExpirationDate).ToList();
                        break;
                    case "uses_asc":
                        discounts = discounts.OrderBy(d => d.CurrentUses).ToList();
                        break;
                    case "uses_desc":
                        discounts = discounts.OrderByDescending(d => d.CurrentUses).ToList();
                        break;
                    case "max_uses_asc":
                        discounts = discounts.OrderBy(d => d.MaximumUses).ToList();
                        break;
                    case "max_uses_desc":
                        discounts = discounts.OrderByDescending(d => d.MaximumUses).ToList();
                        break;
                    default:
                        //If no 'SortBy' is provided
                        discounts = discounts.OrderBy(d => d.Id).ToList();
                        break;
                }

                // Pagination
                const int itemsInPage = 6;
                totalPages = (int)Math.Ceiling(discounts.Count / (double)itemsInPage);
                if (vm.PageId > totalPages)
                    return NotFound();
                vm.Discounts = discounts.Skip((vm.PageId - 1) * itemsInPage).Take(itemsInPage).ToList();
            }

            vm.NoPages = totalPages;
            return View(vm);
        }

        [HttpGet]
        [Authorize(Policy = "Discount.Add|Discount.Edit")]
        public async Task<IActionResult> Save(int id = 0)
        {
            var discountVM = new Discount();
            // Display Edit Page
            if (id != 0)
            {
                if ((await _UnitOfWork.Discounts.GetOneAsync(b => b.Id == id)) is Discount discount)
                {
                    return View(discount);
                }

                TempData["Error"] = "Discount Not Found";
                return RedirectToAction(nameof(Index));
            }

            // Display Add Page
            return View(discountVM);
        }

        [HttpPost]
        [Authorize(Policy = "Discount.Add|Discount.Edit")]
        public async Task<IActionResult> Save(Discount discountVM)
        {
            if (!ModelState.IsValid)
                return View(discountVM);

            // Saving a Newly-Added Discount
            if (discountVM.Id == 0)
            {
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Discounts.GetOneAsync(e => e.Name == discountVM.Name) is Discount))
                {
                    ModelState.AddModelError(nameof(discountVM.Name), "Name already exists");
                    return View(discountVM);
                }
                var createResult = await _UnitOfWork.Discounts.CreateAsync(discountVM);
                if (createResult)
                {
                    TempData["Success"] = "Discount Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Adding Discount";
                return RedirectToAction(nameof(Index));
            }

            // Saving an Existing Discount
            if ((await _UnitOfWork.Discounts.GetOneAsync(b => b.Id == discountVM.Id, null, tracked: false)) is Discount)
            {
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Discounts.GetOneAsync(e => e.Name == discountVM.Name && e.Id != discountVM.Id) is Discount))
                {
                    ModelState.AddModelError(nameof(discountVM.Name), "Name already exists");
                    return View(discountVM);
                }
                var updateResult = await _UnitOfWork.Discounts.UpdateAsync(discountVM);
                if (updateResult)
                {
                    TempData["Success"] = "Discount Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Updating Discount";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Discount Not Found";
            return View(discountVM);
        }
        [HttpPost]
        [Authorize(Policy = "Discount.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if ((await _UnitOfWork.Discounts.GetOneAsync(b => b.Id == id)) is Discount discount)
            {
                var deleteResult = await _UnitOfWork.Discounts.DeleteAsync(discount);
                if (deleteResult)
                {
                    TempData["Success"] = "Discount Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Deleting Discount";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Discount Not Found";
            return RedirectToAction(nameof(Index));
        }
    }

}
