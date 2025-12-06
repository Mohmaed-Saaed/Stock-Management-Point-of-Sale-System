using CoreLayer;
using CoreLayer.Models;
using InfrastructureLayer.Data;
using InfrastructureLayer.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.Areas.Administrative.ViewModels;
using PresentationLayer.Utility;


namespace PresentationLayer.Areas.DashBoard.Controllers
{
    [Area("Administrative")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly IUnitOfWork _UnitOfWork;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork UnitOfWork)
        {
            _UserManager = userManager;
            _RoleManager = roleManager;
            _UnitOfWork = UnitOfWork;

        }

        [Authorize(Policy = "User.View")]
        public IActionResult Index(UsersWithSearchVM vm)
        {
            if (vm.PageId < 1)
                return NotFound();

            List<ApplicationUser> applicationUsers = _UserManager.Users.Where(u => (
            (string.IsNullOrEmpty(vm.Search)) ||
            (string.IsNullOrEmpty(u.UserName) || u.UserName.Contains(vm.Search)) ||
            (string.IsNullOrEmpty(u.Email) || u.Email.Contains(vm.Search))
            )).ToList();

            int totalPages = 0;
            if (applicationUsers.Count != 0)
            {
                // Pagination
                const int itemsInPage = 6;
                totalPages = (int)Math.Ceiling(applicationUsers.Count / (double)itemsInPage);
                if (vm.PageId > totalPages)
                    return NotFound();
                vm.ApplicationUsers = applicationUsers.Skip((vm.PageId - 1) * itemsInPage).Take(itemsInPage).ToList();
            }

            vm.NoPages = totalPages;
            return View(vm);
        }

        [HttpGet]
        [Authorize(Policy = "User.Add|User.Edit")]
        public async Task<IActionResult> Save(string? id)
        {
            var createUser = new CreateUser();

            var branches = await _UnitOfWork.Branches.GetAsync();

            createUser.BranchList = branches
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                })
                .ToList();

            createUser.RolesList = _RoleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();

            if (id is not null)
            {
                var user = _UserManager.Users.FirstOrDefault(x => x.Id == id);

                if (user != null)
                {
                    if(user.UserName=="SuperAdmin")
                    {
                        createUser.IsSuperAdmin = true;
                    }
                    createUser.Id = user.Id;

                    var branchId = (await _UnitOfWork.Branches.GetOneAsync((b => b.Id == user.BranchId)))?.Id ?? 0;

                    var roleId = await _UserManager.GetRolesAsync(user);
                    ViewBag.UserId = user.Id;
                    createUser.UserName = user.UserName ?? "";
                    createUser.Email = user.Email ?? "";
                    createUser.BranchId = branchId;
                    createUser.RoleId = roleId.FirstOrDefault() ?? "";

                    return View(createUser);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(createUser);
        }


        [HttpPost]
        [Authorize(Policy = "User.Add|User.Edit")]
        public async Task<IActionResult> Save(CreateUser createUser)
        {
            ApplicationUser userLoggedIn = (await _UserManager.GetUserAsync(User))!;

            if (userLoggedIn.UserName != "SuperAdmin" && createUser.UserName.Contains("superadmin", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError(nameof(createUser.UserName), "The 'SuperAdmin' username is reserved.");
                return View(createUser);
            }

            var branches = await _UnitOfWork.Branches.GetAsync();

            createUser.BranchList = branches
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                })
                .ToList();

            createUser.RolesList = _RoleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();

            var branchId = createUser.BranchId == 0 ? null : createUser.BranchId;
            ApplicationUser user = new()
            {
                UserName = createUser.UserName,
                Email = createUser.Email,
                EmailConfirmed = true,
                BranchId = branchId
            };

            if (createUser.UserId is null)
            {
                var result = await _UserManager.CreateAsync(user, createUser.Password);
                if (result.Succeeded)
                {
                    var userDb = await _UserManager.FindByNameAsync(user.UserName);

                    if (userDb is not null)
                    {
                        if (createUser.RoleId != "0")
                        {
                            var role = await _UserManager.AddToRoleAsync(userDb, createUser.RoleId);

                            if (role.Succeeded)
                            {
                                TempData["success"] = "User Added Successfully";
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors.Select(e => e.Description)));
                    return View(createUser);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var oldUser = await _UserManager.FindByIdAsync(createUser.UserId);

                var UserUpdated = createUser.Adapt(oldUser);


                if (createUser.BranchId == 0)
                {
                    UserUpdated.BranchId = null;
                }
                else
                {
                    UserUpdated.BranchId = createUser.BranchId;
                }



                var newResult = await _UserManager.UpdateAsync(UserUpdated);
                if (newResult.Succeeded)
                {

                    var userRole = await _UserManager.GetRolesAsync(UserUpdated);

                    if (userRole.Count() > 0)
                    {
                        await _UserManager.RemoveFromRoleAsync(UserUpdated, userRole.FirstOrDefault() ?? "");

                        if (createUser.RoleId != "0")
                            await _UserManager.AddToRoleAsync(UserUpdated, createUser.RoleId);

                    }
                    else
                    {
                        if (createUser.RoleId != "0")
                            await _UserManager.AddToRoleAsync(UserUpdated, createUser.RoleId);
                    }

                    TempData["success"] = "User Updated Successfully";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Join(", ", newResult.Errors.Select(e => e.Description)));

                    return View(createUser);
                }
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Policy = "User.Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _UserManager.FindByIdAsync(id);

            if (user is not null)
            {

                var userResult = await _UserManager.DeleteAsync(user);

                if (userResult.Succeeded)
                {
                    TempData["success"] = "User Deleted Succussfully";
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
