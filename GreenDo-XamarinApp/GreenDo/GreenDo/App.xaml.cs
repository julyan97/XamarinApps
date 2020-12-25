using Prism;
using Prism.Ioc;
using GreenDo.ViewModels;
using GreenDo.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using GreenDo.Services;
using MediaManager;
using MediaManager.Library;

namespace GreenDo
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
            
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            CrossMediaManager.Current.Init();
            await NavigationService.NavigateAsync("/LoginPage");
        }

        protected async override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<LobyRoomPage, LobyRoomPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

            var videoService = new VideoService();
            await videoService.ConnectAsync();
            containerRegistry.RegisterInstance<IVideoService>(videoService);
            containerRegistry.Register<ICameraService, CameraService>();
        }
    }
}
