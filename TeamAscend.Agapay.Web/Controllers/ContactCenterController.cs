using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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
            var resp = new PhonebookPageVM();
            resp.Contacts = new List<PhonebookVM>();
            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var contacts = (from row in db.Phonebooks where !row.IsDeleted select row).ToList();
                if (contacts != null)
                {                    
                    foreach(var row in contacts)
                    {
                        var vm = new PhonebookVM
                        {
                            // Base properties inherited automatically
                            ID = row.ID,
                            ContactName = row.ContactName,
                            ContactNumber = row.ContactNumber,
                            Location = row.Location,
                            Agency = row.Agency,
                            District = row.District,
                            Barangay = row.Barangay,
                            BarangayCaptain = row.BarangayCaptain,
                            EmergencyHotline = row.EmergencyHotline,

                            // Additional display properties with fallback logic
                            DisplayName = (!string.IsNullOrWhiteSpace(row.Agency)) ? row.Agency :
                            (!string.IsNullOrWhiteSpace(row.BarangayCaptain)) ? row.BarangayCaptain :
                            row.ContactName,
                            DisplayNumber = (!string.IsNullOrWhiteSpace(row.EmergencyHotline)) ?
                            row.EmergencyHotline : row.ContactNumber
                        };
                        resp.Contacts.Add(vm);
                    }
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
                    contact.ModifiedBy = "SYSTEM";
                    contact.ModifiedDate = DateTime.Now;
                    res = db.SaveChanges();
                    DataHelper.SetLastModfied(db);
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
                    existingContact.ContactNumber = request.ContactNumber;
                    existingContact.Location = request.Location;
                    existingContact.Barangay = request.Barangay;
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
                DataHelper.SetLastModfied(db);
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
