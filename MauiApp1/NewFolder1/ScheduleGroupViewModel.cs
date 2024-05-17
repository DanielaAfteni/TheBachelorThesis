using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class ScheduleGroupViewModel : ObservableRecipient
    {
        private string _token;

        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;

        private string _group;
        private string _day;

        private ObservableCollection<ScheduleItem> _scheduleItems;
        public ScheduleGroupViewModel(string token, string group, string day)
        {
            _token = token;
            _group = group;
            _day = day;
            _scheduleItems = new ObservableCollection<ScheduleItem>();
            InitializeAsync();
            Console.WriteLine($"INSERTED {group}");
        }

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);

        public ObservableCollection<ScheduleItem> ScheduleItems
        {
            get => _scheduleItems;
            set => SetProperty(ref _scheduleItems, value);
        }

        private async Task InitializeAsync()
        {
            await GetScheduleAsync();
        }

        private async Task GetScheduleAsync()
        {
            /*try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                var response = await client.GetAsync($"https://assistant-gateway.azurewebsites.net/api/subjects/group/{_group}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var scheduleResponse = JsonConvert.DeserializeObject<ScheduleApiResponse>(responseContent);

                    if (scheduleResponse.Success && scheduleResponse.Entity != null)
                    {
                        // Define the order of days of the week
                        var dayOrder = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

                        foreach (var dayOfWeek in dayOrder)
                        {
                            foreach (var item in scheduleResponse.Entity.Where(item => item.Day == dayOfWeek).OrderBy(item => item.StartTime))
                            {
                                ScheduleItems.Add(item);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error in API response: {scheduleResponse.ErrorMessage}");
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
            }*/

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                var subjectGroup = _group;
                var subjectDay = Enum.TryParse(_day, true, out DayOfWeek dayOfWeek) ? dayOfWeek : DateTimeOffset.Now.DayOfWeek;
                var response = await client.GetAsync($"https://assistant-gateway.azurewebsites.net/api/subjects?Group={subjectGroup}&Day={subjectDay}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var scheduleResponse = JsonConvert.DeserializeObject<ScheduleApiResponse>(responseContent);

                    if (scheduleResponse.Success && scheduleResponse.Entity != null)
                    {
                        // Define the order of days of the week

                        ScheduleItems = new ObservableCollection<ScheduleItem>(
                        [
                            .. scheduleResponse.Entity
                                                        .OrderBy(item => item.StartTime)
,
                        ]);
                    }
                    else
                    {
                        Console.WriteLine($"Error in API response: {scheduleResponse.ErrorMessage}");
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
    public class ScheduleItem
    {
        public string Title { get; set; }
        public string Day { get; set; }
        public string Group { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CourseInfo { get; set; }
    }

    public class ScheduleResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public ObservableCollection<ScheduleItem> Entity { get; set; }
    }

    public class ScheduleApiResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public List<ScheduleItem> Entity { get; set; }
    }
}
