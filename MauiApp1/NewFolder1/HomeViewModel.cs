using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class HomeViewModel: ObservableRecipient
    {
        private RelayCommand _logOutCommand;
        private RelayCommand _goToMainPageCommand;
        private RelayCommand _goToSchedulePageCommand;
        private RelayCommand _goToPersonalCabinetPageCommand;
        private RelayCommand _goToChatGPTPageCommand;

        private string _userId;

        public HomeViewModel(string userId)
        {
            _userId = userId;
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
            await Shell.Current.GoToAsync($"{nameof(FlashcardsPage)}");
        }

        private async void ExecuteGoToSchedulePage()
        {
            // Navigate to the schedule page
            await Shell.Current.GoToAsync($"{nameof(SchedulePage)}");
        }

        private async void ExecuteGoToPersonalCabinetPage()
        {
            // Navigate to the personal cabinet page
            await Shell.Current.GoToAsync($"{nameof(PersonalCabinetPage)}");
        }

        private async void ExecuteGoToChatGPTPage()
        {
            // Navigate to the ChatGPT page
            await Shell.Current.GoToAsync($"{nameof(ChatGPTPage)}");
        }
    }
}
