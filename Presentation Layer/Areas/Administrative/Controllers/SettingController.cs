using CoreLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Areas.DashBoard.Controllers
{
    [Area("Administrative")]
    [Authorize]
    public class SettingController : Controller
    {

        [Authorize(Policy = "Setting.View")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
