using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Biometric;
using System;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class HomeViewModel : ObservableRecipient
    {
        private RelayCommand _logOutCommand;
        private RelayCommand _goToMainPageCommand;
        private RelayCommand _goToSchedulePageCommand;
        private RelayCommand _goToPersonalCabinetPageCommand;
        private RelayCommand _goToChatGPTPageCommand;

        private string _token;
        private string _nickname;
        private string _group;
        private string _email;

        public HomeViewModel(string token, string email, string group, string nickname)
        {
            _token = token;
            _nickname = nickname;
            _group = group;
            _email = email;
        }

        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);
        public ICommand GoToMainPageCommand => _goToMainPageCommand ??= new RelayCommand(ExecuteGoToMainPage);
        public ICommand GoToSchedulePageCommand => _goToSchedulePageCommand ??= new RelayCommand(ExecuteGoToSchedulePage);
        public ICommand GoToPersonalCabinetPageCommand => _goToPersonalCabinetPageCommand ??= new RelayCommand(ExecuteGoToPersonalCabinetPage);
        public ICommand GoToChatGPTPageCommand => _goToChatGPTPageCommand ??= new RelayCommand(ExecuteGoToChatGPTPage);

        private async void ExecuteLogOut()
        {
            // Navigate to the login page
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async void ExecuteGoToMainPage()
        {
            // Navigate to the main page
            await Shell.Current.Navigation.PushAsync(new FlashcardsPage(_token, _email, _group, _nickname));
        }

        private async void ExecuteGoToSchedulePage()
        {
            // Navigate to the schedule page
            await Shell.Current.Navigation.PushAsync(new SchedulePage(_token, _email, _group, _nickname));
        }

        private async void ExecuteGoToPersonalCabinetPage()
        {
            var result = await BiometricAuthenticationService.Default.AuthenticateAsync(new AuthenticationRequest()
            {
                Title = "Please authenticate to enter Profile Page",
                NegativeText = "Cancel authentication"
            }, CancellationToken.None);
            if (result.Status == BiometricResponseStatus.Success) 
            {
                await Shell.Current.Navigation.PushAsync(new PersonalCabinetPage(_token, _email, _group, _nickname));
                //await Shell.Current.GoToAsync($"{nameof(PersonalCabinetPage)}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Couldn't authenticate.", "OK");
            }
        }

        private async void ExecuteGoToChatGPTPage()
        {
            // Navigate to the ChatGPT page
            await Shell.Current.GoToAsync($"{nameof(ChatGPTPage)}");
        }
    }
}
