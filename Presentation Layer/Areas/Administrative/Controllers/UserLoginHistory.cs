using CoreLayer;
using CoreLayer.Models;
using CoreLayer.Models.ItemVarients;
using InfrastructureLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Areas.Item.ViewModels;
using PresentationLayer.Utility;

namespace PresentationLayer.Areas.administrative.Controllers
{
    [Area("administrative")]
    [Authorize]
    public class UserLoginHistoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public UserLoginHistoryController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize(Policy = "UserLoginHistory.View")]
        public async Task<IActionResult> Index()
        {
            var UserLoginHistories = (await _UnitOfWork.UserLoginHistories.GetAsync(include: [l => l.User])).OrderByDescending(u=>u.Id);
            return View(UserLoginHistories);
        }

    }
}
