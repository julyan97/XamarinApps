using GreenDo.Services;
using MediaManager;
using MediaManager.Library;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GreenDo.ViewModels
{
    public class LobyRoomPageViewModel : ViewModelBase
    {
        public LobyRoomPageViewModel(INavigationService navigationService, ICameraService cameraService, IVideoService videoService)
          : base(navigationService)
        {
            RedirectToCameraCommand = new DelegateCommand(async () => await RedirectToCameraAsync());
            RedirectToFeedCommand = new DelegateCommand(async () => await RedirectToFeedAsync());
            CameraService = cameraService;
            VideoService = videoService;
        }
        public ICameraService CameraService { get; set; }
        public IVideoService VideoService { get; set; }
        public string Username { get; set; }
        public DelegateCommand RedirectToCameraCommand { get; }
        public DelegateCommand RedirectToFeedCommand { get; }

        private async Task RedirectToCameraAsync()
        {
            var video = await CameraService.TakeVideoAsync();
            var url = await VideoService.UploadVideoAsync(video);


            var parameters = new NavigationParameters();
            parameters.Add("username", Username);
            parameters.Add("videoUrl", url);

            var item = await CrossMediaManager.Current.Extractor.CreateMediaItem(url);
            item.MediaType = MediaType.Hls;
            await CrossMediaManager.Current.Play(item);
        }
        private async Task RedirectToFeedAsync()
        {
            var parameters = new NavigationParameters();
            parameters.Add("username", Username);
            await NavigationService.NavigateAsync("MainPage", parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Username = parameters["username"] as string;

        }
    }
}
