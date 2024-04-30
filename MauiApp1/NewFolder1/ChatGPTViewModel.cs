using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class ChatGPTViewModel : ObservableRecipient
    {
        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;

        private string _answer;

        public string Answer
        {
            get => _answer;
            set => SetProperty(ref _answer, value);
        }

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

        private string _question;
        private RelayCommand _askQuestionCommand;

        public string Question
        {
            get => _question;
            set => SetProperty(ref _question, value);
        }

        public ICommand AskQuestionCommand => _askQuestionCommand ??= new RelayCommand(ExecuteAskQuestion);


        private async void ExecuteAskQuestion()
        {
            // Perform sign-up validation
            bool isValid = ValidateQuestion();

            if (isValid)
            {
                // Prepare the payload
                var payload = new
                {
                    user_email = "userchat@gmail.com",
                    question = Question
                };
                Console.WriteLine($"user_email: userchat@gmail.com");
                Console.WriteLine($"question: {Question}");


                /*var userId = "ac92088d - 083c - 4a1d - a5be - fe9325d2961b";

                // Display userId in the console
                Console.WriteLine($"The userID is {userId}");
                await Shell.Current.Navigation.PushAsync(new HomePage(userId));*/
                // Serialize the payload
                var jsonPayload = JsonConvert.SerializeObject(payload);

                // Send POST request to the API
                using var client = new HttpClient();
                var response = await client.PostAsync("http://10.0.2.2:8080/chat",
                                                       new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and parse the response
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    // Extract the userId from the response
                    string responseToQuestion = responseData.response;

                    // Display userId in the console
                    Console.WriteLine($"The response is {responseToQuestion}");
                    //await Shell.Current.Navigation.PushAsync(new HomePage(userId));
                    Answer = responseToQuestion;
                }
                else
                {
                    Console.WriteLine($"Failed to register user. Status code: {response.StatusCode}");
                    // Display an error message if request fails
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to asq question.", "OK");
                }
            }
            else
            {
                // Display an error message for invalid sign-up information
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid question.", "OK");
            }
        }
        private bool ValidateQuestion()
        {
            // Perform sign-up validation logic here
            // For demonstration purposes, let's assume validation is successful if all fields are non-empty
            return !string.IsNullOrEmpty(Question);
        }
    }
}
