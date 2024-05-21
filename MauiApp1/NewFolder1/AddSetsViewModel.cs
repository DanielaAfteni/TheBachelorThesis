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
    public partial class AddSetsViewModel : ObservableRecipient
    {
        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand _addCommand;

        private string _token;
        private string _nickname;
        private string _group;
        private string _email;
        private string _setId;
        private string _newQuestion;
        private string _newAnswer;

        public AddSetsViewModel(string token, string email, string group, string nickname, string setId)
        {
            _token = token;
            _nickname = nickname;
            _group = group;
            _email = email;
            _setId = setId;
            Console.WriteLine($"User ID: {_token}");
            Console.WriteLine($"Set ID: {_setId}");
        }

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);
        public ICommand AddCommand => _addCommand ??= new RelayCommand(ExecuteAdd);

        public string NewQuestion
        {
            get => _newQuestion;
            set => SetProperty(ref _newQuestion, value);
        }

        public string NewAnswer
        {
            get => _newAnswer;
            set => SetProperty(ref _newAnswer, value);
        }

        private string _pairQuestion;
        private string _pairAnswer;

        public string PairQuestion
        {
            get => _pairQuestion;
            set => SetProperty(ref _pairQuestion, value);
        }

        public string PairAnswer
        {
            get => _pairAnswer;
            set => SetProperty(ref _pairAnswer, value);
        }

        private async void ExecuteGoBack()
        {
            // Navigate back to the previous page
            //await Shell.Current.Navigation.PopAsync();
            await Shell.Current.Navigation.PushAsync(new FlashcardsPage(_token, _email, _group, _nickname));
        }

        private async void ExecuteLogOut()
        {
            // Navigate to the login page
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async void ExecuteAdd()
        {
            // Perform sign-up validation
            bool isValid = ValidateQuestionAndAnswer();

            if (isValid)
            {
                // Prepare the payload
                var payload = new
                {
                    question = NewQuestion,
                    answer = NewAnswer,
                    flashCardSetId = _setId
                };
                Console.WriteLine($"question: {NewQuestion}");
                Console.WriteLine($"answer: {NewAnswer}");
                Console.WriteLine($"flashCardSetId: {_setId}");


                /*var userId = "ac92088d - 083c - 4a1d - a5be - fe9325d2961b";

                // Display userId in the console
                Console.WriteLine($"The userID is {userId}");
                await Shell.Current.Navigation.PushAsync(new HomePage(userId));*/
                // Serialize the payload
                var jsonPayload = JsonConvert.SerializeObject(payload);

                // Send POST request to the API
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                var response = await client.PostAsync("https://assistant-gateway.azurewebsites.net/api/flash-cards",
                                                       new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and parse the response
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    // Extract the userId from the response
                    if(responseData != null)
                    {
                        int pairId = responseData.entity.id;
                        string pairQuestion = responseData.entity.question;
                        string pairAnswer = responseData.entity.answer;
                        int pairSetId = responseData.entity.setId;

                        // Display userId in the console
                        Console.WriteLine($"The pairId is {pairId}");
                        Console.WriteLine($"The pairQuestion is {pairQuestion}");
                        Console.WriteLine($"The pairAnswer is {pairAnswer}");
                        Console.WriteLine($"The pairSetId is {pairSetId}");



                        PairQuestion = pairQuestion;
                        PairAnswer = pairAnswer;

                        Console.WriteLine($"Response Question: {PairQuestion}");
                        Console.WriteLine($"Response Answer: {PairAnswer}");
                        //await Shell.Current.Navigation.PushAsync(new HomePage(userId));
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to register user. Status code: {response.StatusCode}");
                    // Display an error message if request fails
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to add question and answer.", "OK");
                }
            }
            else
            {
                // Display an error message for invalid sign-up information
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid question or answer.", "OK");
            }
        }

        private bool ValidateQuestionAndAnswer()
        {
            // Perform sign-up validation logic here
            // For demonstration purposes, let's assume validation is successful if all fields are non-empty
            return !string.IsNullOrEmpty(NewQuestion) &&
                   !string.IsNullOrEmpty(NewAnswer);
        }

    }
}
