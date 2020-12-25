using GreenDo.Services;
using MediaManager;
using MediaManager.Library;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GreenDo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService, ICameraService cameraService, IVideoService videoService)
            : base(navigationService)
        {
            Title = "Main Page";
            VideoNodes = new ObservableCollection<VideoNode>
            {
                new VideoNode
                {
                    Author = "Play4u",
                    Likes = 10,
                    Hearts = 15
                },
                new VideoNode
                {
                    Author = "Mey4u",
                    Likes = 10,
                    Hearts = 15
                }
            };
            CameraService = cameraService;
            VideoService = videoService;
            NavigateToLoginCommand = new DelegateCommand(async () => await NavigationService.NavigateAsync("/LoginPage"));
            ReactToHeartCommand = new DelegateCommand(() => ReactToVideo(null, "heart"));
            ReactToLikeCommand = new DelegateCommand<VideoNode>((n) => ReactToVideo(n, "like"));
            RecordVideoCommand = new DelegateCommand(async () => await RecordVideo());
        }

        public ICameraService CameraService { get; set; }
        public IVideoService VideoService { get; set; }

        public ObservableCollection<VideoNode> VideoNodes { get; set; }

        public DelegateCommand RecordVideoCommand { get; set; }

        public DelegateCommand ReactToHeartCommand { get; set; }
        public DelegateCommand<VideoNode> ReactToLikeCommand { get; set; }
        public ICommand NavigateToLoginCommand { get; set; }
        public ICommand PlayItemsCommand { get; set; }


        private async Task RecordVideo()
        {
            var video = await CameraService.TakeVideoAsync();
        }

        public void ReactToVideo(VideoNode videoNode, string reactionType)
        {
            if (reactionType == "heart")
            {
                videoNode.HeartsPressed = !videoNode.HeartsPressed;
                videoNode.Hearts += videoNode.HeartsPressed ? 1 : -1;
            }
            else
            {
                videoNode.LikesPressed = !videoNode.HeartsPressed;
                videoNode.Likes += videoNode.HeartsPressed ? 1 : -1;
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var s = "https://play4u-euwe.streaming.media.azure.net/9a6a70ec-d68e-49ad-9962-9fb1fd39eff7/july_tree.ism/manifest(format=m3u8-aapl)";
            var item = await CrossMediaManager.Current.Extractor.CreateMediaItem(s);
            item.MediaType = MediaType.Hls;


            await CrossMediaManager.Current.Play(item);
        }

    }

    public class VideoNode
    {
        public string Link { get; set; }
        public string Author { get; set; }
        public int Likes { get; set; }
        public bool LikesPressed { get; set; } = false;
        public int Hearts { get; set; }
        public bool HeartsPressed { get; set; } = false;
    }
}
