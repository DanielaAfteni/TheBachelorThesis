using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class ScheduleViewModel: ObservableRecipient
    {
        private string _token;
        private string _nickname;
        private string _groupUser;
        private string _email;
        private HttpClient _httpClient;

        private string _group;
        private string _day;
        private RelayCommand _pickPdfCommand;

        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand _goToScheduleDayCommand;
        private RelayCommand _goToScheduleGroupCommand;


        public string Group
        {
            get => _group;
            set => SetProperty(ref _group, value);
        }

        public string Day
        {
            get => _day;
            set => SetProperty(ref _day, value);
        }

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);

        public ICommand GoToScheduleDayCommand => _goToScheduleDayCommand ??= new RelayCommand(ExecuteGoToScheduleDayCommand);
        public ICommand GoToScheduleGroupCommand => _goToScheduleGroupCommand ??= new RelayCommand(ExecuteGoToScheduleGroupCommand);


        public ICommand PickPdfCommand => _pickPdfCommand ??= new RelayCommand(ExecutePickPdf);


        public ScheduleViewModel(string token, string email, string groupUser, string nickname)
        { 
            _token = token;
            _nickname = nickname;
            _groupUser = groupUser;
            _email = email;
        }

        private async void ExecuteGoToScheduleGroupCommand()
        {
            bool isValid = ValidateScheduleGroup();

            if (isValid)
            {
                await Shell.Current.Navigation.PushAsync(new ScheduleGroupPage(_token, Group));
                //await Shell.Current.Navigation.PushAsync(new ScheduleGroupPage(Group));
            }
            else
            {
                // Display an error message
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid Group name.", "OK");
            }

        }

        private async void ExecuteGoToScheduleDayCommand()
        {
            bool isValid = ValidateScheduleDay();

            if (isValid)
            {
                await Shell.Current.Navigation.PushAsync(new ScheduleDayPage(_token, Day));
            }
            else
            {
                // Display an error message
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid Day.", "OK");
            }

        }

        private bool ValidateScheduleGroup()
        {
            // Perform login validation logic here
            // For demonstration purposes, let's assume validation is successful if both fields are non-empty
            return !string.IsNullOrEmpty(Group);
        }

        private bool ValidateScheduleDay()
        {
            // Perform login validation logic here
            // For demonstration purposes, let's assume validation is successful if both fields are non-empty
            return !string.IsNullOrEmpty(Day);
        }

        private async void ExecutePickPdf()
        {
            try
            {
                // Pick a PDF file
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Pdf,
                    PickerTitle = "Pick a PDF file"
                });

                if (result != null)
                {
                    // Process the selected PDF file
                    var stream = await result.OpenReadAsync();
                    // Handle the PDF file stream
                    Console.WriteLine($"PDF was LOADED");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                Console.WriteLine($"Error picking PDF: {ex.Message}");
            }
        }

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
    }
}
