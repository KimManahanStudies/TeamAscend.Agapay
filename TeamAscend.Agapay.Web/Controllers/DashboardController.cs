using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using TeamAscend.Agapay.Web.Attributes;
using TeamAscend.Agapay.Web.Models;
using TeamAscend.Agapay.Web.Shared;

namespace TeamAscend.Agapay.Web.Controllers
{
    [PortalAuthorized]
    public class DashboardController : Controller
    {
        [Route("~/Admin/Dashboard")]
        public IActionResult Index()
        {
            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {

                DataHelper.SetAppGlobalSettingsDefaults(db);
            }

            return View();
        } 
        
    }
}
