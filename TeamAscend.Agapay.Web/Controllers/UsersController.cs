using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TeamAscend.Agapay.Web.Attributes;
using TeamAscend.Agapay.Web.Models;
using TeamAscend.Agapay.Web.Shared;

namespace TeamAscend.Agapay.Web.Controllers
{
    public class UsersController : Controller
    {
        [PortalAuthorized]
        [Route("~/Admin/Users")]
        public IActionResult Index()
        {
            var resp = new UsersPageViewModel();

            resp.Users = new List<UserAccount>();

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var users = (from row in db.UserAccounts where !row.IsDeleted select row).ToList();
                if (users != null)
                {
                    resp.Users = users;
                }
            }
            return View(resp);
        }

        [HttpPost]
        [Route("~/Users/SaveRecord")]
        public IActionResult SaveRecord(UserAccount request)
        //public IActionResult SaveRecord([FromBody] UserAccount request)
        {
            UserAccount resp = new UserAccount();

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var existingUser = (from row in db.UserAccounts where row.ID == request.ID select row).FirstOrDefault();
                if (existingUser != null)
                {
                    //resp.Users = users;
                    existingUser.Username = request.Username;
                    existingUser.Password = request.Password;
                    existingUser.Role = request.Role;

                    existingUser.FirstName = request.FirstName;
                    existingUser.LastName = request.LastName;
                    existingUser.MiddleName = request.MiddleName;
                    existingUser.EmailAddress = request.EmailAddress;
                    existingUser.BirthDate = request.BirthDate;
                    existingUser.ContactNo1 = request.ContactNo1;
                    existingUser.ContactNo2 = request.ContactNo2;
                    existingUser.AddressLine1 = request.AddressLine1;
                    existingUser.AddressLine2 = request.AddressLine2;
                    existingUser.City = request.City;
                    existingUser.Region = request.Region;
                    existingUser.Barangay = request.Barangay;
                    existingUser.ZipPostCode = request.ZipPostCode;
                    existingUser.IsDeleted = existingUser.IsDeleted;

                    existingUser.ModifiedBy = request.ModifiedBy ?? "SYSTEM";
                    existingUser.ModifiedDate = DateTime.Now;
                }
                else
                {
                    request.CreatedBy = request.CreatedBy ?? "SYSTEM";
                    request.ModifiedBy = request.ModifiedBy ?? "SYSTEM";
                    request.CreatedDate = DateTime.Now;
                    request.ModifiedDate = DateTime.Now;
                    request.IsDeleted = false;
                    request.ModifiedBy = request.ModifiedBy ?? "SYSTEM";

                    db.UserAccounts.Add(request);
                }

                db.SaveChanges();
                resp = request;
            }

            //return resp;
            return Redirect("/Admin/Users");
        }


        public IActionResult Login(LoginVM request = null)
        {
            //Default
            if(request== null)
            {
                request = new LoginVM();
            }

            return View(request);
        }

        public IActionResult Logout()
        {
            if (Request.Cookies.Keys.Contains("AGPSession"))
            {
                string currentUserCookie = Request.Cookies["AGPSession"];
                Response.Cookies.Delete("AGPSession");
            }
            return Redirect("/Admin/Dashboard");
        }

        [HttpPost]
        public IActionResult LoginAuthenticate(LoginVM request)
        {
            var resp = new LoginVM();
            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                if (request != null)
                {
                    var loggingUser = (from row in db.UserAccounts 
                                       where row.Username == request.Username
                                       && row.Password == request.Password
                                       select row).FirstOrDefault();
                    if (loggingUser != null)
                    {
                        //clear session remnants
                        if (Request.Cookies.Keys.Contains("AGPSession"))
                        {
                            string currentUserCookie = Request.Cookies["AGPSession"];
                            Response.Cookies.Delete("AGPSession");
                        }
                        CookieOptions option = new CookieOptions();
                        option.Expires = DateTime.Now.AddDays(2);
                        option.Secure = true;
                        string acc_json = JsonConvert.SerializeObject(loggingUser);
                        Response.Cookies.Append("AGPSession", acc_json, option);
                        return Redirect("/Admin/Dashboard");
                    }
                }
                
            }

            return Redirect("/Users/Login?Status=INVALID");
        }
    }
}
