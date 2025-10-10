using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TeamAscend.Agapay.Web.Attributes;
using TeamAscend.Agapay.Web.Models;
using TeamAscend.Agapay.Web.Shared;

namespace TeamAscend.Agapay.Web.Controllers
{
    public class AppCenterController : ControllerBase
    {
        [Route("api/AppCenter/Register")]
        public UserAccount Register([FromBody]UserAccount request)
        {
            var resp = new UserAccount();
            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var existingUser = (from row in db.UserAccounts where row.ID == request.ID select row).FirstOrDefault();
                if (existingUser != null)
                {
                    //Tell the app the user or email already exist!!!
                }
                else
                {
                    request.Username = request.EmailAddress;
                    request.Role = "USER";
                    request.ContactNo1 = "TEST";
                    request.ContactNo2 = "TEST";
                    request.AddressLine2 = "TEST";
                    request.City = "TEST";
                    request.Region = "TEST";
                    request.Barangay = "TEST";
                    request.ZipPostCode = "TEST";
                    request.IsDeleted = false;

                    request.CreatedBy = request.CreatedBy ?? "SYSTEM";
                    request.ModifiedBy = request.ModifiedBy ?? "SYSTEM";
                    request.CreatedDate = DateTime.Now;
                    request.ModifiedDate = DateTime.Now;

                    db.UserAccounts.Add(request);
                }

                db.SaveChanges();
                resp = request;
            }
            return resp;
        }
    }
}
