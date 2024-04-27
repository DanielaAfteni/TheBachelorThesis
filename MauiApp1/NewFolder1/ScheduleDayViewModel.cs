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
    public partial class ScheduleDayViewModel : ObservableRecipient
    {
        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;

        private string _day;

        private ObservableCollection<ScheduleItemDay> _scheduleItems;

        public ScheduleDayViewModel(string day)
        {
            _day = day;
            _scheduleItems = new ObservableCollection<ScheduleItemDay>();
            InitializeAsync();
            Console.WriteLine($"INSERTED {day}");
        }

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);


        public ObservableCollection<ScheduleItemDay> ScheduleItems
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
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync($"https://moodle-api.azurewebsites.net/api/subjects/day/{_day}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var scheduleResponse = JsonConvert.DeserializeObject<ScheduleApiResponseDay>(responseContent);

                    if (scheduleResponse.Success && scheduleResponse.Entity != null)
                    {
                        var filteredItems = scheduleResponse.Entity.OrderBy(item => item.StartTime).ThenBy(item => item.Group);

                        foreach (var item in filteredItems)
                        {
                            ScheduleItems.Add(item);
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

    public class ScheduleItemDay
    {
        public string Title { get; set; }
        public string Day { get; set; }
        public string Group { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CourseInfo { get; set; }
    }

    public class ScheduleResponseDay
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public ObservableCollection<ScheduleItemDay> Entity { get; set; }
    }

    public class ScheduleApiResponseDay
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public List<ScheduleItemDay> Entity { get; set; }
    }
}
