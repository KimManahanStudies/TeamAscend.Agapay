using Microsoft.Extensions.Logging;
using TeamAscend.Agapay.App.Shared;
using TeamAscend.Agapay.Web.Services;

namespace TeamAscend.Agapay.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddSingleton<AppShellContext>();
            builder.Services.AddSingleton<FacebookService>();

            return builder.Build();
        }
    }
}
