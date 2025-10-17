using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TeamAscend.Agapay.App.Model;
using TeamAscend.Agapay.App.Shared;
using TeamAscend.Agapay.Web.Models;

namespace TeamAscend.Agapay.App.Components.Pages
{
    public partial class CreateAccount:ComponentBase
    {
        [Inject]
        private IJSRuntime jsRT { get; set; }

        [Inject]
        private AppShellContext Shell { get; set; }

        [Inject]
        private DatabaseContext DB { get; set; }

        [Inject]
        private NavigationManager Nav { get; set; }

        public UserAccount Model { get; set; }

        protected override void OnInitialized()
        {
            Model = new UserAccount();
        }

        public async void Register()
        {
            //var userInfo = JsonConvert.SerializeObject(Model);
            var RegistrationURL = $"{Constants.AgapayWebAPI_URL}api/AppCenter/Register";
            using (var httpClient = new HttpClient())
            {
                var rawResp1 = await httpClient.PostAsync(RegistrationURL, JsonContent.Create(Model));

                //Save to Local for Offline use
                var appUser = new AppUser
                {
                    Username = Model.Username,
                    Role = Model.Role,
                    Password = Model.Password,
                    FirstName = Model.FirstName,
                    LastName = Model.LastName,
                    MiddleName = Model.MiddleName,
                    BirthDate = Model.BirthDate,
                    EmailAddress = Model.EmailAddress,
                    ContactNo1 = Model.ContactNo1,
                    ContactNo2 = Model.ContactNo2,
                    AddressLine1 = Model.AddressLine1,
                    AddressLine2 = Model.AddressLine2,
                    City = Model.City,
                    Region = Model.Region,
                    Barangay = Model.Barangay,
                    ZipPostCode = Model.ZipPostCode,
                    IsDeleted = Model.IsDeleted,
                    CreatedBy = Model.CreatedBy,
                    CreatedDate = Model.CreatedDate,
                    ModifiedBy = Model.ModifiedBy,
                    ModifiedDate = Model.ModifiedDate
                };

                // Save to local database
                DB.Init();
                DB.SaveUser(appUser);

                Nav.NavigateTo("/verifyaccount");
            }
        }
    }
}
