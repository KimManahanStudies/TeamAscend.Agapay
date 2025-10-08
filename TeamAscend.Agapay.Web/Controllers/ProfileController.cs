using Microsoft.AspNetCore.Mvc;
using TeamAscend.Agapay.Web.Attributes;

namespace TeamAscend.Agapay.Web.Controllers
{
    [PortalAuthorized]
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
