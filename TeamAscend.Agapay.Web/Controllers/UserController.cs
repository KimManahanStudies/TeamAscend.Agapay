using Microsoft.AspNetCore.Mvc;

namespace TeamAscend.Agapay.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }

        public IActionResult ActivityLog()
        {
            return View();
        }
    }
}
