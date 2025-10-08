
using Microsoft.AspNetCore.Mvc;
using TeamAscend.Agapay.Web.Attributes;
using TeamAscend.Agapay.Web.Models;
using TeamAscend.Agapay.Web.Shared;

namespace TeamAscend.Agapay.Web.Controllers
{
    [PortalAuthorized]
    public class ContactCenterController : Controller
    {
        
        [Route("~/Admin/Phonebook")]
        public IActionResult Phonebook()
        {
            return View();
        }

        [Route("~/Admin/Hotlines")]
        public IActionResult Hotlines()
        {
            return View();
        }
    }
}
