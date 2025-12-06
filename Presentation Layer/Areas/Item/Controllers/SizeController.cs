using CoreLayer;
using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Areas.Item.ViewModels;
using PresentationLayer.Utility;

namespace PresentationLayer.Areas.Item.Controllers
{
    [Area("Item")]
    [Authorize]
    public class SizeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public SizeController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        [Authorize(Policy = "Size.View")]
        public async Task<IActionResult> Index(ItemVariantListWithSearchVM<Size> vm)
        {
            if (vm.PageId < 1)
                return NotFound();

            List<Size> sizes = await _UnitOfWork.Sizes.GetAsync(b =>
                string.IsNullOrEmpty(vm.Search) || b.Name.Contains(vm.Search)
                );

            int totalPages = 0;
            if (sizes.Count != 0)
            {
                // Pagination
                const int itemsInPage = 6;
                totalPages = (int)Math.Ceiling(sizes.Count / (double)itemsInPage);
                if (vm.PageId > totalPages)
                    return NotFound();
                vm.ItemVariantList = sizes.Skip((vm.PageId - 1) * itemsInPage).Take(itemsInPage).ToList();
            }

            vm.NoPages = totalPages;
            return View(vm);
        }

        [HttpGet]
        [Authorize(Policy = "Size.Add|Size.Edit")]
        public async Task<IActionResult> Save(int id = 0)
        {
            var sizeVM = new ItemVariantVM<Size>();

            // Display Edit Page
            if (id != 0)
            {
                if ((await _UnitOfWork.Sizes.GetOneAsync(b => b.Id == id)) is Size size)
                {
                    sizeVM.ItemVariant = size;
                    return View(sizeVM);
                }

                TempData["Error"] = "Size Not Found";
                return RedirectToAction(nameof(Index));
            }

            // Display Add Page
            return View(sizeVM);
        }

        [HttpPost]
        [Authorize(Policy = "Size.Add|Size.Edit")]
        public async Task<IActionResult> Save(ItemVariantVM<Size> sizeVM)
        {
            if (!ModelState.IsValid)
                return View(sizeVM);

            Result result = new Result();

            // Saving a New Size
            if (sizeVM.ItemVariant.Id == 0) 
            {
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Sizes.GetOneAsync(e => e.Name == sizeVM.ItemVariant.Name) is Size))
                {
                    ModelState.AddModelError("ItemVariant.Name", "Name already exists");
                    return View(sizeVM);
                }
                if (sizeVM.formFile != null)
                {
                    result = ImageService.UploadNewImage(sizeVM.formFile);
                    if (result.Success)
                        sizeVM.ItemVariant.Image = result.Image;
                    else
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return View(sizeVM);
                    }
                }

                var createResult = await _UnitOfWork.Sizes.CreateAsync(sizeVM.ItemVariant);
                if (createResult)
                {
                    TempData["Success"] = "Size Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Updating Size";
                return RedirectToAction(nameof(Index));
            }

            // Update existing Size
            if ((await _UnitOfWork.Sizes.GetOneAsync(s => s.Id == sizeVM.ItemVariant.Id)) is Size size)
            {
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Sizes.GetOneAsync(e => e.Name == sizeVM.ItemVariant.Name && e.Id != sizeVM.ItemVariant.Id) is Size))
                {
                    ModelState.AddModelError("ItemVariant.Name", "Name already exists");
                    sizeVM.ItemVariant.Image = size.Image;
                    return View(sizeVM);
                }
                if (sizeVM.formFile != null) // Replace
                {
                    if (size.Image != null)
                    {
                        result = ImageService.DeleteImage(size.Image);
                        if (!result.Success)
                        {
                            TempData["Error"] = result.ErrorMessage;
                            return View(sizeVM);
                        }
                    }
                    result = ImageService.UploadNewImage(sizeVM.formFile);
                    if (result.Success)
                        sizeVM.ItemVariant.Image = result.Image;
                    else
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return View(sizeVM);
                    }
                }
                else
                {
                    if (sizeVM.deleteImage) // Delete old
                    {
                        if (size.Image != null)
                        {
                            result = ImageService.DeleteImage(size.Image);
                            if (!result.Success)
                            {
                                TempData["Error"] = result.ErrorMessage;
                                return View(sizeVM);
                            }
                        }
                    }
                    else
                        sizeVM.ItemVariant.Image = size.Image; // Keep
                }

                _UnitOfWork.Sizes.DetachEntity(size);
                var updateResult = await _UnitOfWork.Sizes.UpdateAsync(sizeVM.ItemVariant);
                if (updateResult)
                {
                    TempData["Success"] = "Size Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Updating Size";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Size Not Found";
            return View(sizeVM);
        }
        [HttpPost]
        [Authorize(Policy = "Size.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = new Result();
            if ((await _UnitOfWork.Sizes.GetOneAsync(b => b.Id == id)) is Size size)
            {
                if (size.Image != null)
                {
                    // Deleting Image Physically
                    result = ImageService.DeleteImage(size.Image);
                    if (!result.Success)
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return RedirectToAction(nameof(Index));
                    }
                }

                var deleteResult = await _UnitOfWork.Sizes.DeleteAsync(size);
                if (deleteResult)
                {
                    TempData["Success"] = "Size Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Deleting Size";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Color Not Found";
            return RedirectToAction(nameof(Index));
        }
    }
}
