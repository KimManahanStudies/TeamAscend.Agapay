using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TeamAscend.Agapay.App.Model;
using TeamAscend.Agapay.App.Shared;
using Microsoft.JSInterop;

namespace TeamAscend.Agapay.App.Components.Pages
{
    public partial class SignIn : ComponentBase
    {
        [Inject]
        public AppShellContext Shell { get; set; }

        [Inject]
        public DatabaseContext DB { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private string email = string.Empty;
        private string password = string.Empty;
        private bool isLoading = false;
        private string errorMessage = string.Empty;

        private async Task HandleValidSubmit()
        {
            try
            {
                isLoading = true;
                errorMessage = string.Empty;

                // First try online authentication
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var response = await httpClient.PostAsJsonAsync(
                            $"{Constants.AgapayWebAPI_URL}api/AppCenter/AppAuthenticate",
                            new { Username = email, Password = password }
                        );

                        if (response.IsSuccessStatusCode)
                        {
                            var user = await response.Content.ReadFromJsonAsync<AppUser>();
                            if (user != null)
                            {
                                // Save to local DB
                                var existingUser = DB.Users
                                    .FirstOrDefault(u => u.EmailAddress.Equals(email));

                                if (existingUser == null)
                                    DB.SaveUser(user);
                                else
                                    DB.UpdateUser(user);

                                // Set session
                                Shell.CurrentUser = user;
                                Shell.SetSessionUser(user);
                                Shell.IsUserLoggedIn = true;

                                NavigationManager.NavigateTo("/", true);
                                return;
                            }
                        }
                        errorMessage = "Invalid credentials";
                    }
                    catch (Exception)
                    {
                        // If online auth fails, try offline authentication
                        var offlineUser = DB.Users
                            .FirstOrDefault(u => 
                                u.EmailAddress.Equals(email) && 
                                u.Password.Equals(password) && 
                                !u.IsDeleted);

                        if (offlineUser != null)
                        {
                            // Set session for offline user
                            Shell.CurrentUser = offlineUser;
                            Shell.SetSessionUser(offlineUser);
                            Shell.IsUserLoggedIn = true;

                            NavigationManager.NavigateTo("/", true);
                            return;
                        }
                        errorMessage = "Invalid credentials or no internet connection";
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "An unexpected error occurred";
            }
            finally
            {
                isLoading = false;
                await JSRuntime.InvokeVoidAsync("showError", errorMessage);
            }
        }
    }
}
