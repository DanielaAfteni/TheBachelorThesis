using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private string? _group;
        private string? _day;
        private RelayCommand _pickPdfCommand;

        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand _goToScheduleDayCommand;
        private RelayCommand _goToScheduleGroupCommand;

        private ObservableCollection<ScheduleItem2> _scheduleItems2;


        public string? Group
        {
            get => _group;
            set => SetProperty(ref _group, value);
        }

        public string? Day
        {
            get => _day;
            set => SetProperty(ref _day, value);
        }

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);

        public ICommand GoToScheduleGroupCommand => _goToScheduleGroupCommand ??= new RelayCommand(ExecuteGoToScheduleGroupCommand);


        public ICommand PickPdfCommand => _pickPdfCommand ??= new RelayCommand(ExecutePickPdf);


        public ScheduleViewModel(string token, string email, string groupUser, string nickname)
        { 
            _token = token;
            _nickname = nickname;
            _groupUser = groupUser;
            _email = email;
            _scheduleItems2 = new ObservableCollection<ScheduleItem2>();
            InitializeAsync();
            Console.WriteLine($"User's group: {_groupUser}");
        }

        public ObservableCollection<ScheduleItem2> ScheduleItems2
        {
            get => _scheduleItems2;
            set => SetProperty(ref _scheduleItems2, value);
        }

        private async Task InitializeAsync()
        {
            await GetScheduleAsync();
        }


        private async Task GetScheduleAsync()
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                var subjectGroup = _group ?? _groupUser;
                Group = subjectGroup;
                _day ??= DateTimeOffset.Now.DayOfWeek.ToString();
                var subjectDay = Enum.TryParse(_day, true, out DayOfWeek dayOfWeek) ? dayOfWeek : DateTimeOffset.Now.DayOfWeek;
                var response = await client.GetAsync($"https://assistant-gateway.azurewebsites.net/api/subjects?Group={subjectGroup}&Day={subjectDay}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var scheduleResponse2 = JsonConvert.DeserializeObject<ScheduleApiResponse2>(responseContent);

                    if (scheduleResponse2.Success && scheduleResponse2.Entity != null)
                    {
                        // Define the order of days of the week

                        ScheduleItems2 =new ObservableCollection<ScheduleItem2>(
                        [
                            .. scheduleResponse2.Entity
                                                        .OrderBy(item => item.StartTime)
,
                        ]);
                    }
                    else
                    {
                        Console.WriteLine($"Error in API response: {scheduleResponse2.ErrorMessage}");
                    }
                }
                else
                {
                    Console.WriteLine($"Error getting schedule: Status Code {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting schedule: {ex.Message}");
            }
        }

        private async void ExecuteGoToScheduleGroupCommand()
        {
            bool isValid = ValidateScheduleGroup();

            if (isValid)
            {
                await Shell.Current.Navigation.PushAsync(new ScheduleGroupPage(_token, Group, Day.ToString()));
                //await Shell.Current.Navigation.PushAsync(new ScheduleGroupPage(Group));
            }
            else
            {
                // Display an error message
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid Group name.", "OK");
            }

        }


        private bool ValidateScheduleGroup()
        {
            // Perform login validation logic here
            // For demonstration purposes, let's assume validation is successful if both fields are non-empty
            return !string.IsNullOrEmpty(Group);
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

    public class ScheduleItem2
    {
        public string Title { get; set; }
        public string Day { get; set; }
        public string Group { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CourseInfo { get; set; }
    }

    public class ScheduleResponse2
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public ObservableCollection<ScheduleItem2> Entity { get; set; }
    }

    public class ScheduleApiResponse2
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public List<ScheduleItem2> Entity { get; set; }
    }
}
