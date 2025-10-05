using Microsoft.AspNetCore.Mvc;
using TeamAscend.Agapay.Web.Models;
using TeamAscend.Agapay.Web.Shared;

namespace TeamAscend.Agapay.Web.Controllers
{
    public class DashboardController : Controller
    {
        [Route("~/Admin/Dashboard")]
        public IActionResult Index()
        {
            return View();
        } 

        public IActionResult Phonebook()
        {
            return View();
        }

        public IActionResult BlogPosts()
        {
            return View();
        }

        public IActionResult ResourceLocator()
        {
            return View();
        }

        public IActionResult NewsArticles()
        {
            return View();
        }
    }
}
