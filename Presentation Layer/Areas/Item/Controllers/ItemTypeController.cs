using CoreLayer;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Areas.Item.ViewModels;
using PresentationLayer.Utility;
using System.Drawing;

namespace PresentationLayer.Areas.Item.Controllers
{
    [Area("Item")]
    [Authorize]
    public class ItemTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ItemTypeController(ApplicationDbContext db)
        {
            _db = db;
        }

        // -------------------- TREE --------------------

        [Authorize(Policy = "ItemType.View")]
        public async Task<IActionResult> Index()
        {
            var roots = await _db.ItemTypes
                .AsNoTracking()
                .Where(t => t.ItemTypeId == null)
                .OrderBy(t => t.Name)
                .Select(t => new ItemTypeNodeVM(
                    t.Id,
                    t.Name,
                    t.ItemTypeId,
                    t.Children.Any()
                ))
                .ToListAsync();

            return View(roots);
        }

        // AJAX endpoint that returns ONLY direct children of a node
        [HttpGet]
        public async Task<IActionResult> Children(int id)
        {
            var children = await _db.ItemTypes
                .AsNoTracking()
                .Where(t => t.ItemTypeId == id)
                .OrderBy(t => t.Name)
                .Select(t => new ItemTypeNodeVM(
                    t.Id,
                    t.Name,
                    t.ItemTypeId,
                    t.Children.Any()
                ))
                .ToListAsync();

            return PartialView("_ItemTypeChildren", children);
        }

        // -------------------- CREATE --------------------

        [HttpGet]
        [Authorize(Policy = "ItemType.Add")]
        public async Task<IActionResult> Create(int? parentId)
        {
            var vm = new ItemTypeInputVM { ParentId = parentId };

            if (parentId is not null)
                vm.ParentName = await _db.ItemTypes
                    .Where(t => t.Id == parentId)
                    .Select(t => t.Name)
                    .FirstOrDefaultAsync();

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = "ItemType.Add")]
        public async Task<IActionResult> Create(ItemTypeInputVM vm)
        {
            ModelState.Remove("ItemType.Name");
            if (!ModelState.IsValid)
                return View(vm);

            Result result = new Result();

            // Enforce unique among siblings
            var nameTaken = await _db.ItemTypes
                .AnyAsync(t => t.ItemTypeId == vm.ParentId && t.Name == vm.Name + " " + vm.ParentName);
            if (nameTaken)
            {
                ModelState.AddModelError(nameof(vm.Name), "A sibling with the same name already exists.");
                return View(vm);
            }

            // Image Upload
            if (vm.formFile != null)
            {
                // Saving Physically
                result = ImageService.UploadNewImage(vm.formFile);
                if (result.Success)
                    vm.ItemType.Image = result.Image;
                else
                {
                    TempData["Error"] = result.ErrorMessage;
                    return View(vm);
                }
            }
            if (vm.ParentName is not null)
            {
                if (vm.ParentName.Contains(vm.Name, StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError(nameof(vm.Name), "Cannot set the parent to itself.");
                    return View(vm);
                }
                vm.Name = vm.Name + " " + vm.ParentName;
            }
            try
            {
                _db.ItemTypes.Add(new ItemType
                {
                    Name = vm.Name,
                    ItemTypeId = vm.ParentId,
                    Image = vm.ItemType.Image
                });
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Root Parent Name Already Exists or a DB error";
                return View(vm);
            }
        }

        // -------------------- EDIT --------------------

        [HttpGet]
        [Authorize(Policy = "ItemType.Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await _db.ItemTypes.FindAsync(id);
            if (entity is null)
            {
                TempData["Error"] = "Item Type Not Found";
                return RedirectToAction(nameof(Index));
            }

            var vm = new ItemTypeInputVM
            {
                ParentId = entity.ItemTypeId,
                Name = entity.Name,
                ParentName = entity.ItemTypeId == null
                    ? "(root)"
                    : await _db.ItemTypes.Where(t => t.Id == entity.ItemTypeId).Select(t => t.Name).FirstOrDefaultAsync(),
                ItemType = entity
            };

            ViewBag.Id = id; // pass id to form
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = "ItemType.Edit")]
        public async Task<IActionResult> Edit(int id, ItemTypeInputVM vm)
        {
            var entity = await _db.ItemTypes.FindAsync(id);
            if (entity is null)
            {
                TempData["Error"] = "Item Type Not Found";
                return RedirectToAction(nameof(Index));
            }

            ModelState.Remove("ItemType.Name");
            if (!ModelState.IsValid)
            {
                vm.ItemType.Image = entity.Image;
                vm.ItemType.Id = entity.Id;
                ViewBag.Id = id; // pass id to form
                return View(vm);
            }

            Result result = new Result();

            // 1. Prevent moving under itself
            if (vm.ParentId == id || vm.ParentName.Contains(vm.Name, StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(vm.Name), "Cannot set the parent to itself.");
                vm.ItemType.Image = entity.Image;
                vm.ItemType.Id = entity.Id;
                ViewBag.Id = id; // pass id to form
                return View(vm);
            }

            // 2. Prevent moving under one of its descendants (cycle check)
            if (vm.ParentId != null)
            {
                var flat = await _db.ItemTypes
                    .AsNoTracking()
                    .Select(t => new { t.Id, t.ItemTypeId })
                    .ToListAsync();

                // Build parent->children map
                var childMap = flat
                    .Where(x => x.ItemTypeId != null)
                    .GroupBy(x => x.ItemTypeId!.Value)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(v => v.Id).ToList()
                    );

                // Collect all descendants of this node
                var stack = new Stack<int>();
                stack.Push(id);
                var descendants = new HashSet<int>();

                while (stack.Count > 0)
                {
                    var current = stack.Pop();
                    if (childMap.TryGetValue(current, out var kids))
                    {
                        foreach (var kid in kids)
                        {
                            if (descendants.Add(kid))
                                stack.Push(kid);
                        }
                    }
                }

                if (descendants.Contains(vm.ParentId.Value))
                {
                    ModelState.AddModelError(nameof(vm.ParentId),
                        "Cannot move this item under one of its descendants.");
                    vm.ItemType.Image = entity.Image;
                    vm.ItemType.Id = entity.Id;
                    ViewBag.Id = id; // pass id to form
                    return View(vm);
                }
            }

            // 3. Prevent duplicate sibling names
            var nameTaken = await _db.ItemTypes
                .AnyAsync(t => t.ItemTypeId == vm.ParentId && t.Name == vm.Name && t.Id != id);

            if (nameTaken)
            {
                ModelState.AddModelError(nameof(vm.Name), "A sibling with the same name already exists.");
                vm.ItemType.Image = entity.Image;
                vm.ItemType.Id = entity.Id;
                ViewBag.Id = id; // pass id to form
                return View(vm);
            }

            // Replacing with a New Image
            if (vm.formFile != null)
            {
                if (entity.Image != null)
                {
                    // Deleting Old Image Physically
                    result = ImageService.DeleteImage(entity.Image);
                    if (!result.Success)
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return View(vm);
                    }
                }

                // Add new Image Physically 
                result = ImageService.UploadNewImage(vm.formFile);
                if (result.Success)
                    vm.ItemType.Image = result.Image;
                else
                {
                    TempData["Error"] = result.ErrorMessage;
                    return View(vm);
                }
            }
            else
            {
                // Old Image deleted
                if (vm.deleteImage)
                {
                    // Deleting Old Image Physically
                    result = ImageService.DeleteImage(entity.Image!);
                    if (!result.Success)
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return View(vm);
                    }
                }
                else
                    // Old Image Submitted
                    vm.ItemType.Image = entity.Image;
            }

            // Apply updates
            entity.Name = vm.Name;
            entity.ItemTypeId = vm.ParentId;
            entity.Image = vm.ItemType.Image;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // -------------------- DELETE (recursive) --------------------

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Policy = "ItemType.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = new Result();
            // Load all ItemType Id + ParentId
            var flat = await _db.ItemTypes
                .AsNoTracking()
                .Select(t => new { t.Id, t.ItemTypeId })
                .ToListAsync();

            // Build parent -> children dictionary (only non-null parents)
            var childMap = flat
                .Where(x => x.ItemTypeId != null)
                .GroupBy(x => x.ItemTypeId!.Value) // safe because filtered
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(v => v.Id).ToList()
                );

            // Collect IDs to delete recursively
            var toDelete = new List<int>();
            var stack = new Stack<int>();
            stack.Push(id);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                toDelete.Add(current);

                if (childMap.TryGetValue(current, out var kids))
                {
                    foreach (var kid in kids)
                        stack.Push(kid);
                }
            }

            // Delete all collected nodes in a transaction
            using var tx = await _db.Database.BeginTransactionAsync();
            var entities = await _db.ItemTypes
                .Where(t => toDelete.Contains(t.Id))
                .ToListAsync();

            foreach (var item in entities)
            {
                if (item.Image != null)
                {
                    // Deleting Image Physically
                    result = ImageService.DeleteImage(item.Image);
                    if (!result.Success)
                    {
                        TempData["Error"] = result.ErrorMessage;
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            try
            {
            _db.ItemTypes.RemoveRange(entities);
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Item Type is in use of a CLothing Item";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}