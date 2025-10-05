using Microsoft.AspNetCore.Mvc;
using TeamAscend.Agapay.Web.Models;
using TeamAscend.Agapay.Web.Shared;

namespace TeamAscend.Agapay.Web.Controllers
{
    public class UsersController : Controller
    {

        [Route("~/Admin/Users")]
        public ActionResult Index()
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


        public UserAccount SaveRecord([FromBody] UserAccount request)
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

            return resp;
        }


        public IActionResult Login()
        {
            return RedirectToAction("Index");
        }
    }
}
