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
    public class ColorController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public ColorController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        [HttpGet]
        [Authorize(Policy = "Color.View")]
        public async Task<IActionResult> Index(ItemVariantListWithSearchVM<Color> vm)
        {
            if (vm.PageId < 1)
                return NotFound();

            List<Color> colors = await _UnitOfWork.Colors.GetAsync(b =>
                string.IsNullOrEmpty(vm.Search) || b.Name.Contains(vm.Search)
                );

            int totalPages = 0;
            if (colors.Count != 0)
            {
                // Pagination
                const int itemsInPage = 6;
                totalPages = (int)Math.Ceiling(colors.Count / (double)itemsInPage);
                if (vm.PageId > totalPages)
                    return NotFound();
                vm.ItemVariantList = colors.Skip((vm.PageId - 1) * itemsInPage).Take(itemsInPage).ToList();
            }

            vm.NoPages = totalPages;
            return View(vm);
        }

        [HttpGet]
        [Authorize(Policy = "Color.Add|Color.Edit")]
        public async Task<IActionResult> Save(int id = 0)
        {
            var colorVM = new ItemVariantVM<Color>();

            // Display Edit Page
            if (id != 0)
            {
                if ((await _UnitOfWork.Colors.GetOneAsync(b => b.Id == id)) is Color color)
                {
                    colorVM.ItemVariant = color;
                    return View(colorVM);
                }

                TempData["Error"] = "Color Not Found";
                return RedirectToAction(nameof(Index));
            }

            // Display Add Page
            return View(colorVM);
        }
        [HttpPost]
        [Authorize(Policy = "Color.Add|Color.Edit")]
        public async Task<IActionResult> Save(ItemVariantVM<Color> colorVM)
        {
            if (!ModelState.IsValid)
                return View(colorVM);
            Result result = new Result();

            // Saving a Newly-Added Color
            if (colorVM.ItemVariant.Id == 0)
            {
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Colors.GetOneAsync(e => e.Name == colorVM.ItemVariant.Name) is Color))
                {
                    ModelState.AddModelError("ItemVariant.Name", "Name already exists");
                    return View(colorVM);
                }
                if (colorVM.formFile != null)
                {
                    // Saving Physically
                    result = ImageService.UploadNewImage(colorVM.formFile);
                    if (result.Success)
                        colorVM.ItemVariant.Image = result.Image;
                    else
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return View(colorVM);
                    }
                }

                // Saving in Db
                var createResult = await _UnitOfWork.Colors.CreateAsync(colorVM.ItemVariant);
                if (createResult)
                {
                    TempData["Success"] = "Color Added Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Updating Color";
                return RedirectToAction(nameof(Index));
            }

            // Saving an Existing Color
            if ((await _UnitOfWork.Colors.GetOneAsync(b => b.Id == colorVM.ItemVariant.Id)) is Color color)
            {
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Colors.GetOneAsync(e => e.Name == colorVM.ItemVariant.Name && e.Id != colorVM.ItemVariant.Id) is Color))
                {
                    ModelState.AddModelError("ItemVariant.Name", "Name already exists");
                    colorVM.ItemVariant.Image = color.Image;
                    return View(colorVM);
                }
                // Replacing with a New Image
                if (colorVM.formFile != null)
                {
                    if (color.Image != null)
                    {
                        // Deleting Old Image Physically
                        result = ImageService.DeleteImage(color.Image);
                        if (!result.Success)
                        {
                            TempData["Error"] = result.ErrorMessage;
                            return View(colorVM);
                        }
                    }

                    // Add new Image Physically
                    result = ImageService.UploadNewImage(colorVM.formFile);
                    if (result.Success)
                        colorVM.ItemVariant.Image = result.Image;
                    else
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return View(colorVM);
                    }
                }
                else
                {
                    // Old Image deleted
                    if (colorVM.deleteImage)
                    {
                        // Deleting Old Image Physically
                        result = ImageService.DeleteImage(color.Image!);
                        if (!result.Success)
                        {
                            TempData["Error"] = result.ErrorMessage;
                            return View(colorVM);
                        }
                    }
                    else
                        // Old Image Submitted
                        colorVM.ItemVariant.Image = color.Image;
                }
                _UnitOfWork.Colors.DetachEntity(color);

                var updateResult = await _UnitOfWork.Colors.UpdateAsync(colorVM.ItemVariant);
                if (updateResult)
                {
                    TempData["Success"] = "Color Updated Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Updating Color";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Color Not Found";
            return View(colorVM);
        }
        [HttpPost]
        [Authorize(Policy = "Color.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = new Result();
            if ((await _UnitOfWork.Colors.GetOneAsync(b => b.Id == id)) is Color color)
            {
                if (color.Image != null)
                {
                    // Deleting Image Physically
                    result = ImageService.DeleteImage(color.Image);
                    if (!result.Success)
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return RedirectToAction(nameof(Index));
                    }
                }

                var deleteResult = await _UnitOfWork.Colors.DeleteAsync(color);
                if (deleteResult)
                {
                    TempData["Success"] = "Color Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Error Deleting Color";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Color Not Found";
            return RedirectToAction(nameof(Index));
        }
    }
}
