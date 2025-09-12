using Microsoft.AspNetCore.Mvc;

namespace TeamAscend.Agapay.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return RedirectToAction("Index");
        }
    }
}
