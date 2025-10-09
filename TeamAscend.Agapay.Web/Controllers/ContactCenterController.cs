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
            var resp = new List<Phonebook>();

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var contacts = (from row in db.Phonebooks where !row.IsDeleted select row).ToList();
                if (contacts != null)
                {
                    resp = contacts;
                }
            }
            return View(resp);
        }

        [HttpGet]
        public Phonebook GetContact(int ID)
        {
            Phonebook resp = null;

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                resp = (from row in db.Phonebooks where row.ID == ID && !row.IsDeleted select row).FirstOrDefault();
            }

            return resp;
        }

        [HttpGet]
        public int DeleteContact(int ID)
        {
            int res = 0;

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var contact = (from row in db.Phonebooks where row.ID == ID && !row.IsDeleted select row).FirstOrDefault();
                if(contact != null)
                {
                    contact.IsDeleted = true;
                    res = db.SaveChanges();
                }
            }

            return res;
        }

        [HttpPost]
        [Route("~/ContactCenter/SaveContact")]
        public IActionResult SaveContact(Phonebook request)
        {
            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var existingContact = (from row in db.Phonebooks where row.ID == request.ID select row).FirstOrDefault();
                if (existingContact != null)
                {
                    existingContact.ContactName = request.ContactName;
                    existingContact.ContactNo = request.ContactNo;
                    existingContact.Location = request.Location;
                    existingContact.BarangayName = request.BarangayName;
                    existingContact.ModifiedBy = request.ModifiedBy ?? "SYSTEM";
                    existingContact.ModifiedDate = DateTime.Now;
                }
                else
                {
                    request.CreatedBy = request.CreatedBy ?? "SYSTEM";
                    request.ModifiedBy = request.ModifiedBy ?? "SYSTEM";
                    request.CreatedDate = DateTime.Now;
                    request.ModifiedDate = DateTime.Now;
                    request.IsDeleted = false;

                    db.Phonebooks.Add(request);
                }

                db.SaveChanges();
            }

            return Redirect("/Admin/Phonebook");
        }

        [Route("~/Admin/Hotlines")]
        public IActionResult Hotlines()
        {
            return View();
        }
    }
}
