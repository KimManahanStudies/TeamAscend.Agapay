#if ANDROID
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
#endif
using Application = Microsoft.Maui.Controls.Application;

namespace TeamAscend.Agapay.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

#if ANDROID
            Current.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
#endif
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "Agapay" };
        }
    }
}
