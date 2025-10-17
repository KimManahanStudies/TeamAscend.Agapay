using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Reflection;
using TeamAscend.Agapay.Web.Models;

namespace TeamAscend.Agapay.Web.Shared
{
    public class DataHelper
    {
        public static void SetAppGlobalSettingsDefaults(AgapayTestDBContext db)
        {
            //Adding App Global Settings Default Values
            var allSettings = (from row in db.AppGlobalSettings
                               where !row.IsDeleted
                               select row).ToList();
            if (allSettings.Count == 0)
            {

                AppGlobalSettingVM defaults = new AppGlobalSettingVM();
                var transObj = Utilities.ToDictionary(defaults);

                foreach (string configName in transObj.Keys)
                {
                    db.AppGlobalSettings.Add(new AppGlobalSetting
                    {
                        SettingsName = configName,
                        SettingsValue = transObj[configName].ToString(),
                        IsDeleted = false,
                        CreatedBy = "SYSTEM",
                        ModifiedBy = "SYSTEM",
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                    });
                }
                db.SaveChanges();
            }
        }


        public static string GetAppGlobalSetting(AgapayTestDBContext db, string Name)
        {
            SetAppGlobalSettingsDefaults(db);
            var specSetting = (from row in db.AppGlobalSettings
                               where !row.IsDeleted
                               && row.SettingsName == Name
                               select row).First();
            if (specSetting != null)
            {
                if (!string.IsNullOrWhiteSpace(specSetting.SettingsValue))
                {
                    return specSetting.SettingsValue;
                }
            }

            return null;
        }

        public static void SetAppGlobalSetting(AgapayTestDBContext db, string Name,string Value)
        {
            SetAppGlobalSettingsDefaults(db);
            var specSetting = (from row in db.AppGlobalSettings
                               where !row.IsDeleted
                               && row.SettingsName == Name
                               select row).First();
            if (specSetting != null)
            {
                specSetting.SettingsValue = Value;
            }
            else
            {
                db.AppGlobalSettings.Add(new AppGlobalSetting
                {
                    SettingsName = Name,
                    SettingsValue = Value,
                    IsDeleted = false,
                    CreatedBy = "SYSTEM",
                    ModifiedBy = "SYSTEM",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                });
            }

            db.SaveChanges();
        }


        public static void SetLastModfied(AgapayTestDBContext db)
        {
            DataHelper.SetAppGlobalSetting(db, "LastModifiedDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
