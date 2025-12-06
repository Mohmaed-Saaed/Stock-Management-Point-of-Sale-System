using CoreLayer;
using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using PresentationLayer.Areas.Item.ViewModels;
using PresentationLayer.Areas.Stock.ViewModels;
using PresentationLayer.Utility;

namespace PresentationLayer.Areas.Item.Controllers
{
    [Area("Item")]
    [Authorize]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public BrandController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        [HttpGet]
        [Authorize(Policy = "Brand.View")]
        public async Task<IActionResult> Index(ItemVariantListWithSearchVM<Brand> vm)
        {
            if (vm.PageId < 1)
                return NotFound();

            List<Brand> brands = await _UnitOfWork.Brands.GetAsync(b=>
                string.IsNullOrEmpty(vm.Search) || b.Name.Contains(vm.Search)
                );

            int totalPages = 0;
            if (brands.Count != 0)
            {
                // Pagination
                const int itemsInPage = 6;
                totalPages = (int)Math.Ceiling(brands.Count / (double)itemsInPage);
                if (vm.PageId > totalPages)
                    return NotFound();
                vm.ItemVariantList = brands.Skip((vm.PageId - 1) * itemsInPage).Take(itemsInPage).ToList();
            }

            vm.NoPages = totalPages;
            return View(vm);
        }

        [HttpGet]

        [Authorize(Policy = "Brand.Add|Brand.Edit")]
        public async Task<IActionResult> Save(int id = 0)
        {
            var brandVM = new ItemVariantVM<Brand>();

            // Display Edit Page
            if (id != 0)
            {
                if ((await _UnitOfWork.Brands.GetOneAsync(b => b.Id == id)) is Brand brand)
                {
                    brandVM.ItemVariant = brand;
                    return View(brandVM);
                }

                TempData["Error"] = "Brand Not Found";
                return RedirectToAction(nameof(Index));
            }

            // Display Add Page
            return View(brandVM);
        }

        [HttpPost]
        [Authorize(Policy = "Brand.Add|Brand.Edit")]
        public async Task<IActionResult> Save(ItemVariantVM<Brand> brandVM)
        {
            if (!ModelState.IsValid)
                return View(brandVM);
            Result result = new Result();

            // Saving a Newly-Added Brand
            if (brandVM.ItemVariant.Id == 0)
            {
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Brands.GetOneAsync(e => e.Name == brandVM.ItemVariant.Name) is Brand))
                {
                    ModelState.AddModelError("ItemVariant.Name", "Name already exists");
                    return View(brandVM);
                }

                if (brandVM.formFile != null)
                {
                    // Saving Physically
                    result = ImageService.UploadNewImage(brandVM.formFile);
                    if (result.Success)
                        brandVM.ItemVariant.Image = result.Image;
                    else
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return View(brandVM);
                    }
                }

                // Saving in Db
                var createResult = await _UnitOfWork.Brands.CreateAsync(brandVM.ItemVariant);
                if (createResult)
                {
                    TempData["Success"] = "Brand Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Adding Brand";
                return RedirectToAction(nameof(Index));
            }

            // Saving an Existing Brand
            if ((await _UnitOfWork.Brands.GetOneAsync(b => b.Id == brandVM.ItemVariant.Id)) is Brand brand)
            {
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Brands.GetOneAsync(e => e.Name == brandVM.ItemVariant.Name && e.Id != brandVM.ItemVariant.Id) is Brand))
                {
                    ModelState.AddModelError("ItemVariant.Name", "Name already exists");
                    brandVM.ItemVariant.Image = brand.Image;
                    return View(brandVM);
                }

                // Replacing with a New Image
                if (brandVM.formFile != null)
                {
                    if (brand.Image != null)
                    {
                        // Deleting Old Image Physically
                        result = ImageService.DeleteImage(brand.Image);
                        if (!result.Success)
                        {
                            TempData["Error"] = result.ErrorMessage;
                            return View(brandVM);
                        }
                    }

                    // Add new Image Physically
                    result = ImageService.UploadNewImage(brandVM.formFile);
                    if (result.Success)
                        brandVM.ItemVariant.Image = result.Image;
                    else
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return View(brandVM);
                    }
                }
                else
                {
                    // Old Image deleted
                    if (brandVM.deleteImage)
                    {
                        // Deleting Old Image Physically
                        result = ImageService.DeleteImage(brand.Image!);
                        if (!result.Success)
                        {
                            TempData["Error"] = result.ErrorMessage;
                            return View(brandVM);
                        }
                    }
                    else
                        // Old Image Submitted
                        brandVM.ItemVariant.Image = brand.Image;
                }
                _UnitOfWork.Brands.DetachEntity(brand);

                var updateResult = await _UnitOfWork.Brands.UpdateAsync(brandVM.ItemVariant);
                if (updateResult)
                {
                    TempData["Success"] = "Brand Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Updating Brand";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Brand Not Found";
            return View(brandVM);
        }

        [HttpPost]
        [Authorize(Policy = "Brand.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = new Result();
            if ((await _UnitOfWork.Brands.GetOneAsync(b => b.Id == id)) is Brand brand)
            {
                if (brand.Image != null)
                {
                    // Deleting Image Physically
                    result = ImageService.DeleteImage(brand.Image);
                    if (!result.Success)
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return RedirectToAction(nameof(Index));
                    }
                }
                var deleteResult = await _UnitOfWork.Brands.DeleteAsync(brand);
                if (deleteResult)
                {
                    TempData["Success"] = "Brand Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Deleting Brand";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Brand Not Found";
            return RedirectToAction(nameof(Index));
        }
    }
}
