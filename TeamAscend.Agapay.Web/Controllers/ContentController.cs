
using Microsoft.AspNetCore.Mvc;
using TeamAscend.Agapay.Web.Attributes;
using TeamAscend.Agapay.Web.Models;
using TeamAscend.Agapay.Web.Shared;

namespace TeamAscend.Agapay.Web.Controllers
{
    [PortalAuthorized]
    public class ContentController : Controller
    {
        [Route("~/Admin/Announcements")]
        public IActionResult Announcements()
        {
            return View("Index");
        }
        
        [Route("~/Admin/InfoGraphs")]
        public IActionResult InfoGraphs()
        {
            return View("Index");
        }

        public IActionResult BlogPosts()
        {
            return View("Index");
        }

        public IActionResult NewsArticles()
        {
            return View("Index");
        }
    }
}
