using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TeamAscend.Agapay.App.Model;
using TeamAscend.Agapay.App.Models;
using TeamAscend.Agapay.App.Shared;
using TeamAscend.Agapay.Web.Models;

namespace TeamAscend.Agapay.App.Shared
{
    /// <summary>
    /// Centralized Class for handing global level things for the App
    /// Loaded in MauiProgram as Singleton (one instance only within the App)
    /// </summary>
    public class AppShellContext
    {
        public static AppShellContext Instance { set; get; }

        public AppShellContext()
        {
            Instance = this;
        }

        private bool _isUserLoggedIn;
        public bool IsUserLoggedIn
        {
            get
            {
                return _isUserLoggedIn;
            }
            set
            {
                if (_isUserLoggedIn != value)
                {
                    _isUserLoggedIn = value;
                    NotifyStateChanged();
                }
            }
        }
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public AppUser CurrentUser { get; set; }

        public void SetSessionUser(AppUser CurrentUser)
        {
            var uFilePath = FileSystem.AppDataDirectory + "/sid.key";
            var jsonRaw = JsonConvert.SerializeObject(CurrentUser);
            var encodedData = StringUtilities.Base64Encode(jsonRaw);
            File.WriteAllText(uFilePath, encodedData);
        }

        public AppUser GetSessionUser()
        {
            AppUser res = null;
            var uFilePath = FileSystem.AppDataDirectory + "/sid.key";

            if (File.Exists(uFilePath))
            {
                try
                {
                    var encodedData = File.ReadAllText(uFilePath);
                    var jsonRaw = StringUtilities.Base64Decode(encodedData);
                    res = JsonConvert.DeserializeObject<AppUser>(jsonRaw);
                }
                catch (Exception ex) { }
            }
            return res;
        }

        public void ClearSessionUser()
        {
            var uFilePath = FileSystem.AppDataDirectory + "/sid.key";
            File.Delete(uFilePath);
        }

        public async Task SyncData(DatabaseContext DBContext)
        {
            string DTFile = $"{FileSystem.AppDataDirectory}/LSD.txt";
            string LastSyncDate = "";
            bool FreshDatabase = false;
            if (!File.Exists(Constants.DatabasePath) || !File.Exists(DTFile))
            {
                FreshDatabase = true;
            }

            using (var httpClient = new HttpClient())
            {
                try
                {
                    if (FreshDatabase)
                    {
                        var SyncAPI_URL = $"{Constants.AgapayWebAPI_URL}api/AppCenter/AllData";
                        var rawResp = await httpClient.GetStringAsync(SyncAPI_URL);
                        if (!string.IsNullOrWhiteSpace(rawResp))
                        {
                            var loadedData = JsonConvert.DeserializeObject<AppDataVM>(rawResp);
                            if (loadedData != null)
                            {
                                DBContext.Init();

                                // Save Blog Posts
                                if (loadedData.Contents != null)
                                {
                                    foreach (var post in loadedData.Contents)
                                    {
                                        var appPost = new AppBlogPost
                                        {
                                            ID = post.ID,
                                            UserID = post.UserID,
                                            Title = post.Title,
                                            Content = post.Content,
                                            BlogStatus = post.BlogStatus,
                                            BlogType = post.BlogType,
                                            CoverPhoto = post.CoverPhoto,
                                            IsDeleted = post.IsDeleted,
                                            CreatedBy = post.CreatedBy,
                                            CreatedDate = post.CreatedDate,
                                            ModifiedBy = post.ModifiedBy,
                                            ModifiedDate = post.ModifiedDate
                                        };
                                        DBContext.SaveBlogPost(appPost);
                                    }
                                }

                                // Save Map Locations
                                if (loadedData.Locations != null)
                                {
                                    foreach (var location in loadedData.Locations)
                                    {
                                        var appLocation = new AppMapLocation
                                        {
                                            ID = location.ID,
                                            UserID = location.UserID,
                                            Name = location.Name,
                                            MapCoordinates = location.MapCoordinates,
                                            Address = location.Address,
                                            LocationType = location.LocationType,
                                            Description = location.Description,
                                            ExtraDetails = location.ExtraDetails,
                                            IsDeleted = location.IsDeleted,
                                            CreatedBy = location.CreatedBy,
                                            CreatedDate = location.CreatedDate,
                                            ModifiedBy = location.ModifiedBy,
                                            ModifiedDate = location.ModifiedDate
                                        };
                                        DBContext.SaveMapLocation(appLocation);
                                    }
                                }

                                // Save Phonebook Entries
                                if (loadedData.Hotlines != null)
                                {
                                    foreach (var entry in loadedData.Hotlines)
                                    {
                                        var appPhonebook = new AppPhonebook
                                        {
                                            ID = entry.ID,
                                            ContactName = entry.ContactName,
                                            ContactNo = entry.ContactNo,
                                            Location = entry.Location,
                                            BarangayName = entry.BarangayName,
                                            IsDeleted = entry.IsDeleted,
                                            CreatedBy = entry.CreatedBy,
                                            CreatedDate = entry.CreatedDate,
                                            ModifiedBy = entry.ModifiedBy,
                                            ModifiedDate = entry.ModifiedDate
                                        };
                                        DBContext.SavePhonebookEntry(appPhonebook);
                                    }
                                }

                                var LSDStr = await httpClient.GetStringAsync($"{Constants.AgapayWebAPI_URL}api/AppCenter/GetServerDate");
                                File.WriteAllText(DTFile, LSDStr);

                            }
                        }
                    }
                    else
                    {
                        //For Quicker Checking, Compare Dates
                        var LSDStr = File.ReadAllText(DTFile);
                        var SyncDate_URL = $"{Constants.AgapayWebAPI_URL}api/AppCenter/AppGlobalSettings";
                        var rawRespDate = await httpClient.GetStringAsync(SyncDate_URL);
                        if (!string.IsNullOrWhiteSpace(rawRespDate))
                        {
                            var setData = JsonConvert.DeserializeObject<Dictionary<string, string>>(rawRespDate);
                            if (setData!=null)
                            {
                                if (setData.Keys.Contains("LastModifiedDate"))
                                {
                                    if (!string.IsNullOrWhiteSpace(setData["LastModifiedDate"]))
                                    {
                                        var date1 = DateTime.Parse(LSDStr);
                                        var date2 = DateTime.Parse(setData["LastModifiedDate"]);
                                        if(date1 > date2)
                                        {
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                            


                        var SyncAPI_URL = $"{Constants.AgapayWebAPI_URL}api/AppCenter/LatestData?LastSyncDate={LSDStr}";
                        var rawResp = await httpClient.GetStringAsync(SyncAPI_URL);
                        if (!string.IsNullOrWhiteSpace(rawResp))
                        {
                            var loadedData = JsonConvert.DeserializeObject<AppDataVM>(rawResp);
                            if (loadedData != null)
                            {
                                DBContext.Init();

                                // Save Blog Posts
                                if (loadedData.Contents != null)
                                {
                                    foreach (var post in loadedData.Contents)
                                    {
                                        var appPost = new AppBlogPost
                                        {
                                            ID = post.ID,
                                            UserID = post.UserID,
                                            Title = post.Title,
                                            Content = post.Content,
                                            BlogStatus = post.BlogStatus,
                                            BlogType = post.BlogType,
                                            CoverPhoto = post.CoverPhoto,
                                            IsDeleted = post.IsDeleted,
                                            CreatedBy = post.CreatedBy,
                                            CreatedDate = post.CreatedDate,
                                            ModifiedBy = post.ModifiedBy,
                                            ModifiedDate = post.ModifiedDate
                                        };
                                        var hasExistingData = DBContext.BlogPosts.Where(r => r.ID == post.ID).FirstOrDefault() != null;
                                        if (!hasExistingData)
                                        {
                                            DBContext.SaveBlogPost(appPost);
                                        }
                                        else
                                        {
                                            DBContext.UpdateBlogPost(appPost);
                                        }

                                    }
                                }

                                // Save Map Locations
                                if (loadedData.Locations != null)
                                {
                                    foreach (var location in loadedData.Locations)
                                    {
                                        var appLocation = new AppMapLocation
                                        {
                                            ID = location.ID,
                                            UserID = location.UserID,
                                            Name = location.Name,
                                            MapCoordinates = location.MapCoordinates,
                                            Address = location.Address,
                                            LocationType = location.LocationType,
                                            Description = location.Description,
                                            ExtraDetails = location.ExtraDetails,
                                            IsDeleted = location.IsDeleted,
                                            CreatedBy = location.CreatedBy,
                                            CreatedDate = location.CreatedDate,
                                            ModifiedBy = location.ModifiedBy,
                                            ModifiedDate = location.ModifiedDate
                                        };
                                        var hasExistingLocation = DBContext.MapLocations.Where(r => r.ID == location.ID).FirstOrDefault() != null;
                                        if (!hasExistingLocation)
                                        {
                                            DBContext.SaveMapLocation(appLocation);
                                        }
                                        else
                                        {
                                            DBContext.UpdateMapLocation(appLocation);
                                        }
                                    }
                                }

                                // Save Phonebook Entries
                                if (loadedData.Hotlines != null)
                                {
                                    foreach (var entry in loadedData.Hotlines)
                                    {
                                        var appPhonebook = new AppPhonebook
                                        {
                                            ID = entry.ID,
                                            ContactName = entry.ContactName,
                                            ContactNo = entry.ContactNo,
                                            Location = entry.Location,
                                            BarangayName = entry.BarangayName,
                                            IsDeleted = entry.IsDeleted,
                                            CreatedBy = entry.CreatedBy,
                                            CreatedDate = entry.CreatedDate,
                                            ModifiedBy = entry.ModifiedBy,
                                            ModifiedDate = entry.ModifiedDate
                                        };
                                        var hasExistingEntry = DBContext.PhonebookEntries.Where(r => r.ID == entry.ID).FirstOrDefault() != null;
                                        if (!hasExistingEntry)
                                        {
                                            DBContext.SavePhonebookEntry(appPhonebook);
                                        }
                                        else
                                        {
                                            DBContext.UpdatePhonebookEntry(appPhonebook);
                                        }
                                    }
                                }


                                LSDStr = await httpClient.GetStringAsync($"{Constants.AgapayWebAPI_URL}api/AppCenter/GetServerDate");
                                File.WriteAllText(DTFile, LSDStr);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
