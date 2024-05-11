using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class EachFlashQuizTextViewModel : ObservableRecipient
    {
        private string _token;
        private string _nickname;
        private string _group;
        private string _email;
        private Set _selectedSet;

        public Set SelectedSet
        {
            get => _selectedSet;
            set => SetProperty(ref _selectedSet, value);
        }
        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }
        public List<string> UserAnswers { get; } = new List<string>(); // List to store user's answers

        public List<bool> IsCorrect { get; } = new List<bool>();

        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand<Set> _checkAnswersCommand;

        public EachFlashQuizTextViewModel(string token, string email, string group, string nickname, Set selectedSet)
        {
            _token = token;
            _nickname = nickname;
            _group = group;
            _email = email;
            SelectedSet = selectedSet;
            Title = selectedSet.Title;
            Flashcards = selectedSet.Flashcards;
        }

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);
        public ICommand CheckAnswersCommand => _checkAnswersCommand ??= new RelayCommand<Set>(ExecuteCheckAnswers);

        

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

        /*private async void ExecuteCheckAnswers(Set? selectedSet)
        {
            int correctCount = 0;
            for (int i = 0; i < Flashcards.Count; i++)
            {
                if (i < UserAnswers.Count && Flashcards[i].Answer == UserAnswers[i])
                {
                    correctCount++;
                }
            }

            //int totalQuestions = Math.Min(Flashcards.Count, UserAnswers.Count);
            int totalQuestions = Flashcards.Count;
            Console.WriteLine($"Total correct answers: {correctCount}/{totalQuestions}");
            //await Shell.Current.GoToAsync($"//{nameof(ScorePage)}?correctCount={correctCount}&totalQuestions={totalQuestions}");
            //await Shell.Current.Navigation.PushAsync(new ScorePage(correctCount, totalQuestions));
            await Shell.Current.Navigation.PushAsync(new ScorePage(_token, _email, _group, _nickname, correctCount, totalQuestions));
        }*/

        /*private async void ExecuteCheckAnswers(Set? selectedSet)
        {
            int correctCount = 0;
            for (int i = 0; i < Flashcards.Count; i++)
            {
                if (i < UserAnswers.Count)
                {
                    // Prepare the payload for verification
                    var payload = new
                    {
                        user_email = _email,
                        user_answer = UserAnswers[i],
                        flashcard_answer = Flashcards[i].Answer,
                        question = $"Is the user's answer '{UserAnswers[i]}' similar to '{Flashcards[i].Answer}'? If yes then send me true, if not send me false."
                    };
                    var jsonPayload = JsonConvert.SerializeObject(payload);

                    // Send POST request to the OpenAI (chatGPT) API
                    using var client = new HttpClient();
                    var response = await client.PostAsync("http://10.0.2.2:8080/chat",
                                                           new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        // Read and parse the response
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                        // Extract the verification result
                        string verificationResult = responseData.response;
                        Console.WriteLine($"Verification result for question {i + 1}: {verificationResult}");

                        if (verificationResult.ToLower() == "true")
                        {
                            // Increment correct count if verification result is true
                            correctCount++;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to verify answer {i + 1}. Status code: {response.StatusCode}");
                        // Display an error message if verification fails
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to verify answer.", "OK");
                    }
                }
            }

            //int totalQuestions = Math.Min(Flashcards.Count, UserAnswers.Count);
            int totalQuestions = Flashcards.Count;
            Console.WriteLine($"Total correct answers: {correctCount}/{totalQuestions}");
            await Shell.Current.Navigation.PushAsync(new ScorePage(_token, _email, _group, _nickname, correctCount, totalQuestions));
        }*/

        private async void ExecuteCheckAnswers(Set? selectedSet)
        {
            int correctCount = 0;
            for (int i = 0; i < Flashcards.Count; i++)
            {
                if (i < UserAnswers.Count && Flashcards[i].Answer == UserAnswers[i])
                {
                    Console.WriteLine($"SAME {Flashcards[i].Answer} === {UserAnswers[i]}");
                    // If the user's answer matches the flashcard's answer exactly, count it as correct
                    correctCount++;
                }
                else
                {
                    Console.WriteLine($"OPEN AI verification: {Flashcards[i].Answer} and {UserAnswers[i]}");
                    // Prepare the payload for verification if the answers are not exactly the same
                    var payload = new
                    {
                        user_email = _email,
                        user_answer = UserAnswers[i],
                        flashcard_answer = Flashcards[i].Answer,
                        question = $"Is the user's answer '{UserAnswers[i]}' similar to '{Flashcards[i].Answer}'? If yes then send me just the word true, if not send me just the word false."
                    };
                    var jsonPayload = JsonConvert.SerializeObject(payload);

                    // Send POST request to the OpenAI (chatGPT) API
                    using var client = new HttpClient();
                    var response = await client.PostAsync("http://10.0.2.2:8080/chat",
                                                           new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        // Read and parse the response
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                        // Extract the verification result
                        string verificationResult = responseData.response;
                        Console.WriteLine($"Verification result for question {i + 1}: {verificationResult}");

                        if (verificationResult.ToLower() == "true")
                        {
                            Console.WriteLine($"OPEN AI -- TRUE -- {Flashcards[i].Answer} and {UserAnswers[i]} are the same");
                            // Increment correct count if verification result is true
                            correctCount++;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to verify answer {i + 1}. Status code: {response.StatusCode}");
                        // Display an error message if verification fails
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to verify answer.", "OK");
                    }
                }
            }

            //int totalQuestions = Math.Min(Flashcards.Count, UserAnswers.Count);
            int totalQuestions = Flashcards.Count;
            Console.WriteLine($"Total correct answers: {correctCount}/{totalQuestions}");
            await Shell.Current.Navigation.PushAsync(new ScorePage(_token, _email, _group, _nickname, correctCount, totalQuestions));
        }



        public void UpdateUserAnswer(int index, string answer)
        {
            if (index >= 0 && index < Flashcards.Count)
            {
                if (UserAnswers.Count <= index)
                {
                    UserAnswers.Add(answer);
                }
                else
                {
                    UserAnswers[index] = answer;
                }
            }
        }
    }
}
