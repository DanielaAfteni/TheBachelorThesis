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
    public partial class PersonalCabinetViewModel : ObservableRecipient
    {
        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;

        private string _token;
        private string _nickname;
        private string _group;
        private string _email;
        private int _yearOfStudies;

        public PersonalCabinetViewModel(string token, string email, string group, string nickname)
        {
            _token = token;
            _nickname = nickname;
            _group = group;
            _email = email;
            _yearOfStudies = CalculateYearOfStudies(group);
        }

        public string Nickname => _nickname;
        public string Group => _group;
        public string Email => _email;
        public int YearOfStudies => _yearOfStudies;

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);

        private async void ExecuteGoBack()
        {
            // Navigate back to the previous page
            await Shell.Current.Navigation.PopAsync();
        }

        private async void ExecuteLogOut()
        {
            // Navigate to the login page
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private int CalculateYearOfStudies(string group)
        {
            if (group.StartsWith("FAF-201") || group.StartsWith("FAF-202") || group.StartsWith("FAF-203"))
            {
                return 4;
            }
            else
            {
                return 3;
            }
        }
    }
}
