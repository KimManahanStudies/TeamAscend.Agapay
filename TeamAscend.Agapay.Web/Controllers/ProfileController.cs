using Microsoft.AspNetCore.Mvc;

namespace TeamAscend.Agapay.Web.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
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
