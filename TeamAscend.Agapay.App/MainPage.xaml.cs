using TeamAscend.Agapay.App.Shared;

namespace TeamAscend.Agapay.App
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            var AppShell = Handler.MauiContext.Services.GetService<AppShellContext>();
            var DB = Handler.MauiContext.Services.GetService<DatabaseContext>();
            //Load User Session
            if (AppShell != null)
            {
                var loggedUser = AppShell.GetSessionUser();

                if (loggedUser != null)
                {
                    AppShell.CurrentUser = loggedUser;
                    AppShell.IsUserLoggedIn = true;
                }
            }
            
        }
    }
}
