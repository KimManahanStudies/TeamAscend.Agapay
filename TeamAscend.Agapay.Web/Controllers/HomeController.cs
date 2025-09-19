using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TeamAscend.Agapay.Web.Models;

namespace TeamAscend.Agapay.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        [Route("~/Pages/AboutAgapayPh")]
        public IActionResult AboutAgapayPh()
        {
            return View();
        }

        [Route("~/Pages/Maps")]
        public IActionResult Maps()
        {
            return View();
        }

        [Route("~/Pages/TermsAndConditions")]
        public IActionResult TermsAndConditions()
        {
            return View();
        }

        [Route("~/Pages/PrivacyPolicy")]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
