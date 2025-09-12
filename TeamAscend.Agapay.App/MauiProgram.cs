using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Controls.Handlers.Compatibility;
#if IOS || MACCATALYST
using Microsoft.Maui.Controls.Platform.iOS;
using UIKit; // Required for iOS types
#endif
#if ANDROID
using Android.Views; // Required for Android types
#endif

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
//            .ConfigureMauiHandlers(handlers =>
//                {
//                    // Disable overscroll effect for Android
//                    BlazorWebViewHandler.BlazorWebViewMapper.AppendToMapping("CustomBlazorWebViewMapper", (handler, view) =>
//                    {
//#if IOS || MACCATALYST
//                        //handler.PlatformView.ScrollView.Bounces = false;
//#endif
//#if ANDROID
//                        //handler.PlatformView.OverScrollMode = OverScrollMode.Never;
//#endif
//                    });
//                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
