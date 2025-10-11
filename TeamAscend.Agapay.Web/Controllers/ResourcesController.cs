using Microsoft.AspNetCore.Mvc;
using TeamAscend.Agapay.Web.Attributes;
using TeamAscend.Agapay.Web.Models;
using TeamAscend.Agapay.Web.Shared;
using Newtonsoft.Json;

namespace TeamAscend.Agapay.Web.Controllers
{
    [PortalAuthorized]
    public class ResourcesController : Controller
    {
        [Route("~/Admin/Locations")]
        public IActionResult Locations()
        {
            var locations = new List<MapLocation>();

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                locations = (from row in db.MapLocations where !row.IsDeleted select row).ToList();
            }
            return View(locations);
        }

        [HttpGet]
        public MapLocation GetLocation(int ID)
        {
            MapLocation location = null;

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                location = (from row in db.MapLocations 
                           where row.ID == ID && !row.IsDeleted 
                           select row).FirstOrDefault();
            }

            return location;
        }

        [HttpGet]
        public int DeleteLocation(int ID)
        {
            int result = 0;

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var location = (from row in db.MapLocations 
                              where row.ID == ID && !row.IsDeleted 
                              select row).FirstOrDefault();
                if(location != null)
                {
                    location.IsDeleted = true;
                    result = db.SaveChanges();
                }
            }

            return result;
        }

        [HttpPost]
        [Route("~/Resources/SaveLocation")]
        public IActionResult SaveLocation(MapLocation request)
        {
            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var existingLocation = (from row in db.MapLocations 
                                      where row.ID == request.ID 
                                      select row).FirstOrDefault();
                
                if (existingLocation != null)
                {
                    existingLocation.Name = request.Name;
                    existingLocation.LocationType = request.LocationType;  // Added LocationType
                    existingLocation.Address = request.Address;
                    existingLocation.MapCoordinates = request.MapCoordinates;
                    existingLocation.Description = request.Description;
                    existingLocation.ExtraDetails = request.ExtraDetails;
                    existingLocation.ModifiedBy = request.ModifiedBy ?? "SYSTEM";
                    existingLocation.ModifiedDate = DateTime.Now;
                }
                else
                {
                    request.CreatedBy = request.CreatedBy ?? "SYSTEM";
                    request.ModifiedBy = request.ModifiedBy ?? "SYSTEM";
                    request.CreatedDate = DateTime.Now;
                    request.ModifiedDate = DateTime.Now;
                    request.IsDeleted = false;
                    
                    // Get current user ID from session
                    if (Request.Cookies.TryGetValue("AGPSession", out string userJson))
                    {
                        var currentUser = JsonConvert.DeserializeObject<UserAccount>(userJson);
                        request.UserID = currentUser.ID;
                    }

                    db.MapLocations.Add(request);
                }

                db.SaveChanges();
            }

            return Redirect("/Admin/Locations");
        }

        [HttpGet]
        public JsonResult GetAllLocations()
        {
            var locations = new List<MapLocation>();

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                locations = (from row in db.MapLocations 
                            where !row.IsDeleted 
                            select row).ToList();
            }

            return Json(locations);
        }

        [Route("~/Admin/Map")]
        public IActionResult MapView()
        {
            return View();
        }
    }
}
