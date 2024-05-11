using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private RelayCommand _readAnswerCommand;
        private RelayCommand _listenCommand;

        private bool _isListeningForAnswer = false;
        private string _userAnswer = "";

        private string _answer;

        public string Answer
        {
            get => _answer;
            set => SetProperty(ref _answer, value);
        }

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);
        public ICommand ReadAnswerCommand => _readAnswerCommand ??= new RelayCommand(ExecuteReadAnswer);

        public ICommand ListenCommand => _listenCommand ??= new RelayCommand(StartListening);


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

        private async void ExecuteReadAnswer()
        {
            if (!string.IsNullOrEmpty(Answer))
            {
                // Read the answer using text-to-speech
                await TextToSpeech.SpeakAsync(Answer);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No answer.", "OK");
            }
        }

        private async void StartListening()
        {
            var isGranted = await SpeechToText.RequestPermissions();
            if (!isGranted)
            {
                await Toast.Make("Permission not granted").Show(CancellationToken.None);
                return;
            }

            SpeechToText.Default.RecognitionResultUpdated += OnRecognitionTextUpdated;
            SpeechToText.Default.RecognitionResultCompleted += OnRecognitionTextCompleted;
            await SpeechToText.StartListenAsync(CultureInfo.CurrentCulture, CancellationToken.None);
            Console.WriteLine("Listening for user's question...");
        }

        void OnRecognitionTextUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            if (_isListeningForAnswer)
            {
                _userAnswer = recognitionResult;
                Console.WriteLine($"User's question (Updated): {_userAnswer}");
            }
        }

        async void OnRecognitionTextCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            Console.WriteLine($"QUESTION: {recognitionResult}");
            Question = recognitionResult;
            ExecuteAskQuestion();
            Console.WriteLine("SENDING question...");
            if (_isListeningForAnswer)
            {
                _isListeningForAnswer = false;
                Question = recognitionResult;
                ExecuteAskQuestion();
                Console.WriteLine("SENDING question...");
                //CheckAnswer(recognitionResult);
                //await Task.Delay(1000); // Delay before speaking the next question
            }
        }
    }
}
