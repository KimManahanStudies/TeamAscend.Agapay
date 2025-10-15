using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TeamAscend.Agapay.Web.Attributes;
using TeamAscend.Agapay.Web.Models;
using TeamAscend.Agapay.Web.Shared;

namespace TeamAscend.Agapay.Web.Controllers
{
    public class AppCenterController : ControllerBase
    {
        [HttpGet]
        [Route("api/AppCenter/GetServerDate")]
        public string GetServerDate(string LasySyncDate)
        {
            var resp = DateTime.Now.ToString();
            return resp;
        }

        [HttpGet]
        [Route("api/AppCenter/AppGlobalSettings")]
        public Dictionary<string, string> GetAppGlobalSettings()
        {
            Dictionary<string, string> resp = new Dictionary<string, string>();
            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                var allSettings = (from row in db.AppGlobalSettings
                                   where !row.IsDeleted
                                 select row).ToList();
                foreach (var setting in allSettings) 
                {
                    resp.Add(setting.SettingsName, setting.SettingsValue);
                }
            }
            return resp;
        }

        [HttpGet]
        [Route("api/AppCenter/AllData")]
        public AppDataVM AllAppData()
        {
            var resp = new AppDataVM();
            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                resp.Contents = (from row in db.BlogPosts
                                 where !row.IsDeleted 
                                 && row.BlogStatus == "PUBLISHED"
                                 select row).ToList();
                
                
                //Convert CoverPhoto to URL
                foreach(var itm in resp.Contents)
                {
                    if (!string.IsNullOrWhiteSpace(itm.CoverPhoto))
                    {
                        var splitImgData = itm.CoverPhoto.Split(',');
                        byte[] fileData = Convert.FromBase64String(splitImgData[1]); //newStream.ToArray();// strm.ToArray();

                        var resizeImgData = Utilities.ProportionallyResizeBitmap(fileData, 1000, 1000);

                        itm.CoverPhoto = $"{splitImgData[0]},{Convert.ToBase64String(resizeImgData)}";
                    }                    
                }


                resp.Hotlines = (from row in db.Phonebooks
                                 where !row.IsDeleted 
                                 select row).ToList();
                resp.Locations = (from row in db.MapLocations
                                 where !row.IsDeleted 
                                 select row).ToList();

            }
            return resp;
        }

        [HttpGet]
        [Route("api/AppCenter/LatestData")]
        public AppDataVM LatestData(string LastSyncDate)
        {
            var resp = new AppDataVM();
            var LSDate = DateTime.Now;

            if (!string.IsNullOrWhiteSpace(LastSyncDate))
            {
                LSDate = DateTime.Parse(LastSyncDate);
            }

            using (AgapayTestDBContext db = new AgapayTestDBContext())
            {
                resp.Contents = (from row in db.BlogPosts
                                 where row.ModifiedDate >= LSDate
                                 select row).ToList();


                //Convert CoverPhoto to URL
                foreach (var itm in resp.Contents)
                {
                    if (!string.IsNullOrWhiteSpace(itm.CoverPhoto))
                    {
                        var splitImgData = itm.CoverPhoto.Split(',');
                        byte[] fileData = Convert.FromBase64String(splitImgData[1]); //newStream.ToArray();// strm.ToArray();

                        var resizeImgData = Utilities.ProportionallyResizeBitmap(fileData, 1000, 1000);

                        itm.CoverPhoto = $"{splitImgData[0]},{Convert.ToBase64String(resizeImgData)}";
                    }
                }


                resp.Hotlines = (from row in db.Phonebooks
                                 where row.ModifiedDate >= LSDate
                                 select row).ToList();
                resp.Locations = (from row in db.MapLocations
                                  where row.ModifiedDate >= LSDate
                                  select row).ToList();

            }
            return resp;
        }

        [HttpPost]
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
