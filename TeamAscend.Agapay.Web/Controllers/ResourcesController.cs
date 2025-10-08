using Microsoft.AspNetCore.Mvc;
using TeamAscend.Agapay.Web.Attributes;
using TeamAscend.Agapay.Web.Models;
using TeamAscend.Agapay.Web.Shared;

namespace TeamAscend.Agapay.Web.Controllers
{
    [PortalAuthorized]
    public class ResourcesController : Controller
    {
        
        [Route("~/Admin/Locations")]
        public IActionResult Locations()
        {
            return View();
        }

        [Route("~/Admin/Map")]
        public IActionResult MapView()
        {
            return View();
        }
    }
}
