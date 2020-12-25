using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace GreenDo.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        public LoginPageViewModel(INavigationService navigationService)
            :base(navigationService)
        {
            SigninCommand = new DelegateCommand(async () => await LoginAsync());
        }

        public ICommand SigninCommand { get; set; }
        private string username;
        public string Username { get => username; set => SetProperty(ref username, value); }

        private async Task LoginAsync()
        {
            var parameters = new NavigationParameters();
            parameters.Add("username", Username);

            await NavigationService.NavigateAsync("NavigationPage/LobyRoomPage");
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }
}
