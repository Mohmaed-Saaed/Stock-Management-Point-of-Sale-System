using CoreLayer;
using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Areas.administrative.ViewModels;
using PresentationLayer.Areas.Administrative.ViewModels;
using PresentationLayer.Areas.Item.ViewModels;
using PresentationLayer.Utility;

namespace PresentationLayer.Areas.administrative.Controllers
{
    [Area("administrative")]
    [Authorize]
    public class TaxController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public TaxController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Policy = "Tax.View")]
        public async Task<IActionResult> Index(TaxesWithSearchVM vm)
        {
            if (vm.PageId < 1)
                return NotFound();

            List<Tax> taxes = await _UnitOfWork.Taxes.GetAsync(t =>
            string.IsNullOrEmpty(vm.Search) || t.Name.Contains(vm.Search)
            );

            int totalPages = 0;
            if (taxes.Count != 0)
            {
                // Sorting By...
                switch (vm.SortBy)
                {
                    case "rate_asc":
                        taxes = taxes.OrderBy(d => d.Rate).ToList();
                        break;
                    case "rate_desc":
                        taxes = taxes.OrderByDescending(d => d.Rate).ToList();
                        break;
                    default:
                        //If no 'SortBy' is provided
                        taxes = taxes.OrderBy(d => d.Id).ToList();
                        break;
                }

                // Pagination
                const int itemsInPage = 6;
                totalPages = (int)Math.Ceiling(taxes.Count / (double)itemsInPage);
                if (vm.PageId > totalPages)
                    return NotFound();
                vm.Taxes = taxes.Skip((vm.PageId - 1) * itemsInPage).Take(itemsInPage).ToList();
            }

            vm.NoPages = totalPages;
            return View(vm);
        }

        [HttpGet]
        [Authorize(Policy = "Tax.Add|Tax.Edit")]
        public async Task<IActionResult> Save(int id = 0)
        {
            var taxVM = new Tax();
            // Display Edit Page
            if (id != 0)
            {
                if ((await _UnitOfWork.Taxes.GetOneAsync(b => b.Id == id)) is Tax tax)
                {
                    return View(tax);
                }

                TempData["Error"] = "Tax Not Found";
                return RedirectToAction(nameof(Index));
            }

            // Display Add Page
            return View(taxVM);
        }


        [HttpPost]
        [Authorize(Policy = "Tax.Add|Tax.Edit")]
        public async Task<IActionResult> Save(Tax taxVM)
        {
            if (!ModelState.IsValid)
                return View(taxVM);

            // Saving a Newly-Added Tax
            if (taxVM.Id == 0)
            {
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Taxes.GetOneAsync(e => e.Name == taxVM.Name) is Tax))
                {
                    ModelState.AddModelError(nameof(taxVM.Name), "Name already exists");
                    return View(taxVM);
                }
                var createResult = await _UnitOfWork.Taxes.CreateAsync(taxVM);
                if (createResult)
                {
                    TempData["Success"] = "Tax Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Adding Tax";
                return RedirectToAction(nameof(Index));
            }

            // Saving an Existing Tax
            if ((await _UnitOfWork.Taxes.GetOneAsync(b => b.Id == taxVM.Id, null, tracked: false)) is Tax)
            {
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Taxes.GetOneAsync(e => e.Name == taxVM.Name && e.Id != taxVM.Id) is Tax))
                {
                    ModelState.AddModelError(nameof(taxVM.Name), "Name already exists");
                    return View(taxVM);
                }
                var updateResult = await _UnitOfWork.Taxes.UpdateAsync(taxVM);
                if (updateResult)
                {
                    TempData["Success"] = "Tax Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Updating Tax";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Tax Not Found";
            return View(taxVM);
        }


        [HttpPost]
        [Authorize(Policy = "Tax.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if ((await _UnitOfWork.Taxes.GetOneAsync(b => b.Id == id)) is Tax tax)
            {
                var deleteResult = await _UnitOfWork.Taxes.DeleteAsync(tax);
                if (deleteResult)
                {
                    TempData["Success"] = "Tax Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Deleting Tax";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Tax Not Found";
            return RedirectToAction(nameof(Index));
        }
    }
}
