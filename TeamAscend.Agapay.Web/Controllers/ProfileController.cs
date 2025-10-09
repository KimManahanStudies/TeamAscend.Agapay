using Microsoft.AspNetCore.Mvc;
using TeamAscend.Agapay.Web.Attributes;
using TeamAscend.Agapay.Web.Models;
using TeamAscend.Agapay.Web.Shared;
using Newtonsoft.Json;

namespace TeamAscend.Agapay.Web.Controllers
{
    [PortalAuthorized]
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                return Redirect("/Users/Login");
            }

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var user = (from row in db.UserAccounts 
                           where row.ID == currentUser.ID && !row.IsDeleted 
                           select row).FirstOrDefault();
                
                return View(user);
            }
        }

        public IActionResult Settings()
        {
            return View();
        }

        public IActionResult ActivityLog()
        {
            return View();
        }

        [HttpPost]
        [Route("~/Profile/SaveProfile")]
        public IActionResult SaveProfile(UserAccount request)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null || currentUser.ID != request.ID)
            {
                return Redirect("/Users/Login");
            }

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var user = (from row in db.UserAccounts 
                           where row.ID == request.ID && !row.IsDeleted 
                           select row).FirstOrDefault();
                
                if (user != null)
                {
                    user.Username = request.Username;
                    user.Password = request.Password;
                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;
                    user.MiddleName = request.MiddleName;
                    user.EmailAddress = request.EmailAddress;
                    user.BirthDate = request.BirthDate;
                    user.ContactNo1 = request.ContactNo1;
                    user.ContactNo2 = request.ContactNo2;
                    user.AddressLine1 = request.AddressLine1;
                    user.AddressLine2 = request.AddressLine2;
                    user.City = request.City;
                    user.Region = request.Region;
                    user.Barangay = request.Barangay;
                    user.ZipPostCode = request.ZipPostCode;
                    user.ModifiedBy = user.Username;
                    user.ModifiedDate = DateTime.Now;

                    db.SaveChanges();

                    // Update session cookie with new user info
                    UpdateSessionUser(user);
                }
            }

            return Redirect("/Profile");
        }

        private UserAccount GetCurrentUser()
        {
            if (Request.Cookies.TryGetValue("AGPSession", out string userJson))
            {
                return JsonConvert.DeserializeObject<UserAccount>(userJson);
            }
            return null;
        }

        private void UpdateSessionUser(UserAccount user)
        {
            if (Request.Cookies.Keys.Contains("AGPSession"))
            {
                Response.Cookies.Delete("AGPSession");
            }
            
            CookieOptions option = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(2),
                Secure = true
            };
            
            string userJson = JsonConvert.SerializeObject(user);
            Response.Cookies.Append("AGPSession", userJson, option);
        }
    }
}
