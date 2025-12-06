using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Areas.Branch.ViewModels;
using PresentationLayer.Areas.Item.ViewModels;
using PresentationLayer.Areas.Stock.ViewModels;

namespace PresentationLayer.Areas.Branch.Controllers
{
    [Area("Branch")]
    [Authorize]
    public class BranchController : Controller
    {

        private readonly IUnitOfWork _UnitOfWork;
        private readonly UserManager<ApplicationUser> _UserManager;


        public BranchController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _UnitOfWork = unitOfWork;
            _UserManager = userManager;
        }
        [Authorize(Policy = "Stock.View")]
        public async Task<IActionResult> Index(BranchesWithSearch vm)
        {
            if (vm.PageId < 1)
                return NotFound();

            ApplicationUser user = (await _UserManager.GetUserAsync(User))!;

            List<CoreLayer.Models.Branch> branches = await _UnitOfWork.Branches.GetAsync(t =>
                (user.BranchId == null || t.Id == user.BranchId)
                &&
                string.IsNullOrEmpty(vm.Search) || t.Name.Contains(vm.Search)
            );

            int totalPages = 0;
            if (branches.Count != 0)
            {
                // Pagination
                const int itemsInPage = 6;
                totalPages = (int)Math.Ceiling(branches.Count / (double)itemsInPage);
                if (vm.PageId > totalPages)
                    return NotFound();
                vm.Branches = branches.Skip((vm.PageId - 1) * itemsInPage).Take(itemsInPage).ToList();
            }

            vm.NoPages = totalPages;
            return View(vm);
        }
        [HttpGet]
        [Authorize(Policy = "Stock.Add|Stock.Edit")]
        public async Task<IActionResult> Save(int? Id)
        {
            var branchVM = new BranchVM();
            var branch = await _UnitOfWork.Branches.GetOneAsync(x => x.Id == Id);
            ApplicationUser user = (await _UserManager.GetUserAsync(User))!;

            // Display Edit Page
            if (branch is not null)
            {
                branchVM.Name = branch.Name;
                branchVM.BranchId = branch.Id;
                branchVM.Address = branch.Address;
                branchVM.PhoneNumber = branch.PhoneNumber;
                return View(branchVM);
            }
            return View(branchVM);
        }

        [HttpGet]
        [Authorize(Policy = "ClothingItem.BranchItem")]
        public async Task<IActionResult> ViewBranchItems(BranchItemsFilters2VM vm)
        {

            var branchItems = await _UnitOfWork.BranchItems.GetAsync(
                b => (b.BranchId == vm.Id)
                &&
                (string.IsNullOrEmpty(vm.Search)
                || b.Item.Barcode.Contains(vm.Search)
                || b.Item.Name.Contains(vm.Search))
                && (vm.SellingPriceFilter == null || b.SellingPrice >= vm.SellingPriceFilter)
                && (vm.BuyingPriceAvgFilter == null || b.BuyingPriceAvg >= vm.BuyingPriceAvgFilter)
                && (vm.LastBuyingPriceFilter == null || b.LastBuyingPrice >= vm.LastBuyingPriceFilter)
                && (vm.QuantityFilter == null || b.Quantity >= vm.QuantityFilter)
                && (vm.RestockThresholdFilter == null || b.RestockThreshold >= vm.RestockThresholdFilter)
                && (vm.DiscountRateFilter == null || b.DiscountRate >= vm.DiscountRateFilter)
                && (vm.OutDatedInMonthsFilter == null || b.OutDatedInMonths >= vm.OutDatedInMonthsFilter)
                , include: [b => b.Branch, b => b.Item]
                , false);


            int totalPages = 0;

            if (branchItems.Count != 0)
            {
                // Sorting By...
                switch (vm.SortBy)
                {
                    case "qty_asc":
                        branchItems = branchItems.OrderBy(d => d.Quantity).ToList();
                        break;
                    case "qty_desc":
                        branchItems = branchItems.OrderByDescending(d => d.Quantity).ToList();
                        break;
                    case "rth_asc":
                        branchItems = branchItems.OrderBy(d => d.RestockThreshold).ToList();
                        break;
                    case "rth_desc":
                        branchItems = branchItems.OrderByDescending(d => d.RestockThreshold).ToList();
                        break;
                    case "avg_asc":
                        branchItems = branchItems.OrderBy(d => d.BuyingPriceAvg).ToList();
                        break;
                    case "avg_desc":
                        branchItems = branchItems.OrderByDescending(d => d.BuyingPriceAvg).ToList();
                        break;
                    case "last_asc":
                        branchItems = branchItems.OrderBy(d => d.LastBuyingPrice).ToList();
                        break;
                    case "last_desc":
                        branchItems = branchItems.OrderByDescending(d => d.LastBuyingPrice).ToList();
                        break;
                    case "sell_asc":
                        branchItems = branchItems.OrderBy(d => d.SellingPrice).ToList();
                        break;
                    case "sell_desc":
                        branchItems = branchItems.OrderByDescending(d => d.SellingPrice).ToList();
                        break;
                    case "disc_asc":
                        branchItems = branchItems.OrderBy(d => d.DiscountRate).ToList();
                        break;
                    case "disc_desc":
                        branchItems = branchItems.OrderByDescending(d => d.DiscountRate).ToList();
                        break;
                    case "out_asc":
                        branchItems = branchItems.OrderBy(d => d.OutDatedInMonths).ToList();
                        break;
                    case "out_desc":
                        branchItems = branchItems.OrderByDescending(d => d.OutDatedInMonths).ToList();
                        break;
                    default:
                        //If no 'SortBy' is provided
                        branchItems = branchItems.OrderBy(d => d.BranchId).ToList();
                        break;
                }

                // Pagination
                const int itemsInPage = 4;
                totalPages = (int)Math.Ceiling(branchItems.Count / (double)itemsInPage);
                if (vm.PageId > totalPages)
                    vm.PageId = 1;

                branchItems = branchItems.Skip((vm.PageId - 1) * itemsInPage).Take(itemsInPage).ToList();
            }

            vm.NoPages = totalPages;
            vm.BranchItems = branchItems;

            return View(vm);
        }

        [HttpPost]
        [Authorize(Policy = "ClothingItem.BranchItem")]
        public async Task<IActionResult> SaveBranchItem(BranchItemDTO branchItemDTO)
        {
            var branchItem = await _UnitOfWork.BranchItems.GetOneAsync(b => b.BranchId == branchItemDTO.BranchId && b.ItemId == branchItemDTO.ItemId);

            if (branchItem is null)
                return Json(new { status = false });

            //branchItem.Quantity = branchItemDTO.Quantity;
            //branchItem.BuyingPriceAvg = branchItemDTO.BuyingPriceAvg;
            //branchItem.LastBuyingPrice = branchItemDTO.LastBuyingPrice;
            branchItem.SellingPrice = branchItemDTO.SellingPrice;
            branchItem.DiscountRate = branchItemDTO.DiscountRate;
            branchItem.RestockThreshold = branchItemDTO.RestockThreshold;
            branchItem.OutDatedInMonths = branchItemDTO.OutDatedInMonths;

            var result = await _UnitOfWork.BranchItems.UpdateAsync(branchItem);

            if (result)
                return Json(new { status = true });
            else
                return Json(new { status = false });

        }

        [HttpPost]
        [Authorize(Policy = "Stock.Add|Stock.Edit")]
        public async Task<IActionResult> Save(BranchVM branchVM)
        {
            ApplicationUser user = (await _UserManager.GetUserAsync(User))!;

            if (branchVM.BranchId is not null)
            {
                // Editing a Branch
                if ((await _UnitOfWork.Branches.GetOneAsync(b => b.Id == branchVM.BranchId, null, tracked: false)) is CoreLayer.Models.Branch oldBranch)
                {
                    // Checking Name Uniqueness
                    if ((await _UnitOfWork.Branches.GetOneAsync(e => e.Name == branchVM.Name && e.Id != branchVM.BranchId) is CoreLayer.Models.Branch))
                    {
                        ModelState.AddModelError(nameof(branchVM.Name), "Name already exists");

                        return View(branchVM);
                    }
                    oldBranch.Name = branchVM.Name;
                    oldBranch.Address = branchVM.Address;
                    oldBranch.PhoneNumber = branchVM.PhoneNumber;
                    var result = await _UnitOfWork.Branches.UpdateAsync(oldBranch);
                    if (result)
                    {
                        TempData["success"] = "Branch updated";
                        return RedirectToAction(nameof(Index));
                    }
                    TempData["Error"] = "A Db Error Updating Branch";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Branch Not Found";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Adding a New Branch
                // Checking Name Uniqueness
                if ((await _UnitOfWork.Branches.GetOneAsync(e => e.Name == branchVM.Name && e.Id != branchVM.BranchId) is CoreLayer.Models.Branch))
                {
                    ModelState.AddModelError(nameof(branchVM.Name), "Name already exists");
                    ViewBag.ShowBranchItems = false;
                    return View(branchVM);
                }

                var newBranch = branchVM.Adapt<CoreLayer.Models.Branch>();

                var result = await _UnitOfWork.Branches.CreateAsync(newBranch);

                if (result)
                {
                    var items = await _UnitOfWork.Items.GetAsync();
                    var branchItems = new List<BranchItem>();
                    if (items.Count() > 0)
                    {
                        foreach (var item in items)
                        {
                            var branchItem = new BranchItem()
                            {
                                Quantity = 0,
                                SellingPrice = 0,
                                BuyingPriceAvg = 0,
                                LastBuyingPrice = 0,
                                ItemId = item.Id,
                                BranchId = newBranch.Id
                            };
                            branchItems.Add(branchItem);
                        }

                        var resultAddItemBranch = await _UnitOfWork.BranchItems.CreateRangeAsync(branchItems);
                        if (!resultAddItemBranch)
                        {
                            TempData["success"] = "Branch Added";
                            TempData["error"] = "Couldn't add items in the branch";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    TempData["success"] = "Branch Added with its Items";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Adding Branch";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Policy = "Stock.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var branch = await _UnitOfWork.Branches.GetOneAsync(b => b.Id == id);

            if (branch is not null)
            {

                var branchResult = await _UnitOfWork.Branches.DeleteAsync(branch);

                if (branchResult)
                {
                    TempData["success"] = "Branch Deleted Successfully";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "A Db Error Deleting Branch";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = "Branch Not Found";
            return RedirectToAction(nameof(Index));
        }
    }
}
