/*using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;



namespace MauiApp1.NewFolder1
{
    public partial class EachFlashQuizVoiceViewModel : ObservableRecipient
    {
        //public string RecognitionText { get; set; }
        private string _token;

        string RecognitionText = "";
        public Command ListenCommand { get; set; }
        public Command ListenCancelCommand { get; set; }

        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }

        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand _listenCommand;
        private RelayCommand _listenCancelCommand;


        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);


        private CancellationTokenSource tokenSource = new CancellationTokenSource();


        private bool _nextCommandSpoken = false;






        public EachFlashQuizVoiceViewModel (string token, Set selectedSet)
        {

            _token = token;
            Title = selectedSet.Title;
            Flashcards = selectedSet.Flashcards;
            //SpeakQuestions();
            //_ = StartListening();



            ListenCommand = new Command(StartListening);
            Console.WriteLine($"TEXT: {RecognitionText}");
            ListenCancelCommand = new Command(StopListening);

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

        private async Task SpeakQuestions()
        {
            foreach (var flashcard in Flashcards)
            {
                if (!string.IsNullOrEmpty(flashcard.Question))
                {
                    await TextToSpeech.SpeakAsync(flashcard.Question);
                }
            }
        }

        async Task Listen()
        {
            RecognitionText = string.Empty;
            var isAuthorized = await SpeechToText.RequestPermissions(CancellationToken.None);
            if (isAuthorized)
            {
                var recognitionResult = await SpeechToText.ListenAsync(CultureInfo.GetCultureInfo("en-us"), new Progress<string>(async partialText =>
                {
                    RecognitionText += partialText + " ";

                }), CancellationToken.None);

                if (recognitionResult.IsSuccessful)
                {
                    RecognitionText = recognitionResult.Text;
                    Console.WriteLine($"TEXT10: {RecognitionText}");
                }
                else
                {
                    await Toast.Make(recognitionResult.Exception.Message).Show(CancellationToken.None);
                }
            }
            else
            {
                await Toast.Make("Permission denied").Show(CancellationToken.None);
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
            //Console.WriteLine($"TEXT10: {RecognitionText}");
            SpeechToText.Default.RecognitionResultCompleted += OnRecognitionTextCompleted;
            //Console.WriteLine($"TEXT10: {RecognitionText}");

            await SpeechToText.StartListenAsync(CultureInfo.CurrentCulture, CancellationToken.None);
            Console.WriteLine($"TEXT10: {RecognitionText}");

        }

        private async void StopListening()
        {
            await SpeechToText.StopListenAsync(CancellationToken.None);
            SpeechToText.Default.RecognitionResultUpdated -= OnRecognitionTextUpdated;
            SpeechToText.Default.RecognitionResultCompleted -= OnRecognitionTextCompleted;
            Console.WriteLine($"TEXT1: {RecognitionText}");

        }

        void OnRecognitionTextUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs args)
        {
            var curr = args.RecognitionResult.ToString();
            RecognitionText += args.RecognitionResult;
            Console.WriteLine($"TEXT0: {curr}");


        }

        async void OnRecognitionTextCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs args)
        {
            var curr1 = args.RecognitionResult.ToString();
            RecognitionText = args.RecognitionResult;
            Console.WriteLine($"TEXT1111: {curr1}");

            // Check if the recognized text contains the word "next"
            if (curr1.ToLower().Contains("next") && !_nextCommandSpoken)
            {
                // If "next" is detected and not spoken yet, speak the questions
                _nextCommandSpoken = true; // Set the flag to true to prevent multiple executions
                _ = SpeakQuestions();
            }
        }

    }
}*/

/*
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class EachFlashQuizVoiceViewModel : ObservableRecipient
    {
        private string _token;
        private int _currentFlashcardIndex = 0;
        private bool _isListeningForNext = false;
        //private List<Flashcard> _flashcards;
        private string _userAnswer = "";
        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }

        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);

        public Command ListenCommand { get; set; }
        public Command ListenCancelCommand { get; set; }

        public EachFlashQuizVoiceViewModel(string token, Set selectedSet)
        {
            _token = token;
            Title = selectedSet.Title;
            Flashcards = selectedSet.Flashcards;
            //_flashcards = selectedSet.Flashcards;
            ListenCommand = new Command(StartListening);
            ListenCancelCommand = new Command(StopListening);
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
        }

        private async void StopListening()
        {
            await SpeechToText.StopListenAsync(CancellationToken.None);
            SpeechToText.Default.RecognitionResultUpdated -= OnRecognitionTextUpdated;
            SpeechToText.Default.RecognitionResultCompleted -= OnRecognitionTextCompleted;
        }

        void OnRecognitionTextUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            if (_isListeningForNext && recognitionResult.ToLower().Contains("next"))
            {
                _isListeningForNext = false;
                SpeakNextQuestion();
            }
            else
            {
                _userAnswer += recognitionResult;
            }
        }

        async void OnRecognitionTextCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            if (_isListeningForNext && recognitionResult.ToLower().Contains("next"))
            {
                _isListeningForNext = false;
                SpeakNextQuestion();
            }
            else
            {
                CheckAnswer();
            }
        }

        private async void SpeakNextQuestion()
        {
            if (_currentFlashcardIndex < Flashcards.Count)
            {
                var question = Flashcards[_currentFlashcardIndex].Question;
                await TextToSpeech.SpeakAsync(question);
                _isListeningForNext = true;
            }
            else
            {
                // Quiz completed
                await Toast.Make("Quiz completed").Show(CancellationToken.None);
            }
        }

        private void CheckAnswer()
        {
            var currentFlashcard = Flashcards[_currentFlashcardIndex];
            if (_userAnswer.ToLower().Trim() == currentFlashcard.Answer.ToLower().Trim())
            {
                // Correct answer
                // Handle scoring or any other logic
                // Move to the next question
                _currentFlashcardIndex++;
                SpeakNextQuestion();
            }
            else
            {
                // Incorrect answer
                // Handle incorrect answer logic if needed
                // Repeat the question or move to the next question
                SpeakNextQuestion();
            }
            _userAnswer = ""; // Reset user's answer for the next question
        }
    }
}
*/




/*
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class EachFlashQuizVoiceViewModel : ObservableRecipient
    {
        private string _token;
        private int _currentFlashcardIndex = 0;
        private bool _isListeningForNext = false;
        private string _userAnswer = "";
        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }

        public ICommand GoBackCommand { get; private set; }
        public ICommand LogOutCommand { get; private set; }

        public Command ListenCommand { get; set; }
        public Command ListenCancelCommand { get; set; }

        public EachFlashQuizVoiceViewModel(string token, Set selectedSet)
        {
            _token = token;
            Title = selectedSet.Title;
            Flashcards = selectedSet.Flashcards;
            ListenCommand = new Command(StartListening);
            ListenCancelCommand = new Command(StopListening);
            GoBackCommand = new RelayCommand(ExecuteGoBack);
            LogOutCommand = new RelayCommand(ExecuteLogOut);
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
        }

        private async void StopListening()
        {
            await SpeechToText.StopListenAsync(CancellationToken.None);
            SpeechToText.Default.RecognitionResultUpdated -= OnRecognitionTextUpdated;
            SpeechToText.Default.RecognitionResultCompleted -= OnRecognitionTextCompleted;
        }

        void OnRecognitionTextUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            Console.WriteLine($"Recognized (Updated): {recognitionResult}");
            if (_isListeningForNext && recognitionResult.ToLower().Contains("next"))
            {
                _isListeningForNext = false;
                SpeakNextQuestion();
            }
            else
            {
                _userAnswer += recognitionResult;
            }
        }

        async void OnRecognitionTextCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            Console.WriteLine($"Recognized (Completed): {recognitionResult}");
            if (_isListeningForNext && recognitionResult.ToLower().Contains("next"))
            {
                _isListeningForNext = false;
                SpeakNextQuestion();
            }
            else
            {
                CheckAnswer();
            }
        }

        private async void SpeakNextQuestion()
        {
            if (_currentFlashcardIndex < Flashcards.Count)
            {
                var question = Flashcards[_currentFlashcardIndex].Question;
                await TextToSpeech.SpeakAsync(question);
                _isListeningForNext = false; // Stop listening for "next" temporarily
                await Task.Delay(1000); // Delay to ensure speech is completed before listening for answer
                StartListeningForAnswer();
            }
            else
            {
                // Quiz completed
                await Toast.Make("Quiz completed").Show(CancellationToken.None);
            }
        }

        private async void StartListeningForAnswer()
        {
            _userAnswer = ""; // Clear previous answer
            _isListeningForNext = false; // Ensure not listening for "next" while listening for answer
            await SpeechToText.StartListenAsync(CultureInfo.CurrentCulture, CancellationToken.None);
        }

        private void CheckAnswer()
        {
            var currentFlashcard = Flashcards[_currentFlashcardIndex];
            if (_userAnswer.ToLower().Trim() == currentFlashcard.Answer.ToLower().Trim())
            {
                // Correct answer
                // Handle scoring or any other logic
                // Move to the next question
                _currentFlashcardIndex++;
                SpeakNextQuestion();
            }
            else
            {
                // Incorrect answer
                // Handle incorrect answer logic if needed
                // Repeat the question or move to the next question
                SpeakNextQuestion();
            }
            _userAnswer = ""; // Reset user's answer for the next question
        }
    }
}
*/


/*
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class EachFlashQuizVoiceViewModel : ObservableRecipient
    {
        private string _token;
        private int _currentFlashcardIndex = 0;
        private bool _isListeningForNext = false;
        private string _userAnswer = "";
        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }

        public ICommand GoBackCommand { get; private set; }
        public ICommand LogOutCommand { get; private set; }

        public Command ListenCommand { get; set; }
        public Command ListenCancelCommand { get; set; }

        public EachFlashQuizVoiceViewModel(string token, Set selectedSet)
        {
            _token = token;
            Title = selectedSet.Title;
            Flashcards = selectedSet.Flashcards;
            ListenCommand = new Command(StartListening);
            ListenCancelCommand = new Command(StopListening);
            GoBackCommand = new RelayCommand(ExecuteGoBack);
            LogOutCommand = new RelayCommand(ExecuteLogOut);
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
        }

        private async void StopListening()
        {
            await SpeechToText.StopListenAsync(CancellationToken.None);
            SpeechToText.Default.RecognitionResultUpdated -= OnRecognitionTextUpdated;
            SpeechToText.Default.RecognitionResultCompleted -= OnRecognitionTextCompleted;
        }

        void OnRecognitionTextUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            Console.WriteLine($"Recognized (Updated): {recognitionResult}");
            if (_isListeningForNext && recognitionResult.ToLower().Contains("next"))
            {
                _isListeningForNext = false;
                SpeakNextQuestion();
            }
            else
            {
                _userAnswer += recognitionResult;
            }
        }

        async void OnRecognitionTextCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            Console.WriteLine($"Recognized (Completed): {recognitionResult}");
            if (_isListeningForNext && recognitionResult.ToLower().Contains("next"))
            {
                _isListeningForNext = false;
                SpeakNextQuestion();
            }
            else
            {
                CheckAnswer();
            }
        }

        private async void StartListeningForAnswer()
        {
            var isGranted = await SpeechToText.RequestPermissions();
            if (!isGranted)
            {
                await Toast.Make("Permission not granted").Show(CancellationToken.None);
                return;
            }

            SpeechToText.Default.RecognitionResultUpdated += OnRecognitionTextUpdatedAnswer;
            SpeechToText.Default.RecognitionResultCompleted += OnRecognitionTextCompletedAnswer;
            await SpeechToText.StartListenAsync(CultureInfo.CurrentCulture, CancellationToken.None);
        }

        private async void StopListeningForAnswer()
        {
            await SpeechToText.StopListenAsync(CancellationToken.None);
            SpeechToText.Default.RecognitionResultUpdated -= OnRecognitionTextUpdatedAnswer;
            SpeechToText.Default.RecognitionResultCompleted -= OnRecognitionTextCompletedAnswer;
        }

        void OnRecognitionTextUpdatedAnswer(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            Console.WriteLine($"Recognized (Answer Updated): {recognitionResult}");
            _userAnswer += recognitionResult;
        }

        async void OnRecognitionTextCompletedAnswer(object? sender, SpeechToTextRecognitionResultCompletedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            Console.WriteLine($"Recognized (Answer Completed): {recognitionResult}");
            CheckAnswer();
        }

        private async void SpeakNextQuestion()
        {
            if (_currentFlashcardIndex < Flashcards.Count)
            {
                var question = Flashcards[_currentFlashcardIndex].Question;
                await TextToSpeech.SpeakAsync(question);
                _isListeningForNext = false; // Stop listening for "next" temporarily
                await Task.Delay(1000); // Delay to ensure speech is completed before listening for answer
                StartListeningForAnswer();
            }
            else
            {
                // Quiz completed
                await Toast.Make("Quiz completed").Show(CancellationToken.None);
            }
        }

        private void CheckAnswer()
        {
            var currentFlashcard = Flashcards[_currentFlashcardIndex];
            if (_userAnswer.ToLower().Trim() == currentFlashcard.Answer.ToLower().Trim())
            {
                // Correct answer
                // Handle scoring or any other logic
                // Move to the next question
                _currentFlashcardIndex++;
                SpeakNextQuestion();
            }
            else
            {
                // Incorrect answer
                // Handle incorrect answer logic if needed
                // Repeat the question or move to the next question
                SpeakNextQuestion();
            }
            _userAnswer = ""; // Reset user's answer for the next question
        }
    }
}*/




/*
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class EachFlashQuizVoiceViewModel : ObservableRecipient
    {
        private string _token;
        private int _currentFlashcardIndex = 0;
        private bool _isListeningForAnswer = false;
        private string _userAnswer = "";
        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }

        public ICommand GoBackCommand { get; private set; }
        public ICommand LogOutCommand { get; private set; }

        public Command ListenCommand { get; set; }
        public Command ListenCancelCommand { get; set; }

        public EachFlashQuizVoiceViewModel(string token, Set selectedSet)
        {
            _token = token;
            Title = selectedSet.Title;
            Flashcards = selectedSet.Flashcards;
            ListenCommand = new Command(StartListening);
            ListenCancelCommand = new Command(StopListening);
            GoBackCommand = new RelayCommand(ExecuteGoBack);
            LogOutCommand = new RelayCommand(ExecuteLogOut);

            SpeakNextQuestion();
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
        }

        private async void StopListening()
        {
            await SpeechToText.StopListenAsync(CancellationToken.None);
            SpeechToText.Default.RecognitionResultUpdated -= OnRecognitionTextUpdated;
            SpeechToText.Default.RecognitionResultCompleted -= OnRecognitionTextCompleted;
        }

        void OnRecognitionTextUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            if (_isListeningForAnswer)
            {
                _userAnswer = recognitionResult;
            }
        }

        async void OnRecognitionTextCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs args)
        {
            if (_isListeningForAnswer)
            {
                _isListeningForAnswer = false;
                CheckAnswer();
                await Task.Delay(1000); // Delay before speaking the next question
                SpeakNextQuestion();
            }
        }

        private async void SpeakNextQuestion()
        {
            if (_currentFlashcardIndex < Flashcards.Count)
            {
                var question = Flashcards[_currentFlashcardIndex].Question;
                await TextToSpeech.SpeakAsync(question);
                _isListeningForAnswer = true;
            }
            else
            {
                // Quiz completed
                await Toast.Make("Quiz completed").Show(CancellationToken.None);
            }
        }

        private void CheckAnswer()
        {
            var currentFlashcard = Flashcards[_currentFlashcardIndex];
            if (_userAnswer.ToLower().Trim() == currentFlashcard.Answer.ToLower().Trim())
            {
                // Correct answer
                // Handle scoring or any other logic
            }
            else
            {
                // Incorrect answer
                // Handle incorrect answer logic if needed
            }
            _currentFlashcardIndex++;
        }
    }
}*/









/*

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class EachFlashQuizVoiceViewModel : ObservableRecipient
    {
        private string _token;
        private int _currentFlashcardIndex = 0;
        private bool _isListeningForAnswer = false;
        private string _userAnswer = "";
        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }

        public ICommand GoBackCommand { get; private set; }
        public ICommand LogOutCommand { get; private set; }

        public Command ListenCommand { get; set; }
        public Command ListenCancelCommand { get; set; }

        public EachFlashQuizVoiceViewModel(string token, Set selectedSet)
        {
            _token = token;
            Title = selectedSet.Title;
            Flashcards = selectedSet.Flashcards;
            ListenCommand = new Command(StartListening);
            ListenCancelCommand = new Command(StopListening);
            GoBackCommand = new RelayCommand(ExecuteGoBack);
            LogOutCommand = new RelayCommand(ExecuteLogOut);

            SpeakNextQuestion();
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
            Console.WriteLine("Listening for user's answer...");
        }

        private async void StopListening()
        {
            await SpeechToText.StopListenAsync(CancellationToken.None);
            SpeechToText.Default.RecognitionResultUpdated -= OnRecognitionTextUpdated;
            SpeechToText.Default.RecognitionResultCompleted -= OnRecognitionTextCompleted;
            Console.WriteLine("Stopped listening.");
        }

        void OnRecognitionTextUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            if (_isListeningForAnswer)
            {
                _userAnswer = recognitionResult;
                Console.WriteLine($"User's answer (Updated): {_userAnswer}");
            }
        }

        async void OnRecognitionTextCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            Console.WriteLine($"ANSWER: {recognitionResult}");
            if (_isListeningForAnswer)
            {
                _isListeningForAnswer = false;
                CheckAnswer();
                await Task.Delay(1000); // Delay before speaking the next question
                SpeakNextQuestion();
            }
        }

        private async void SpeakNextQuestion()
        {
            if (_currentFlashcardIndex < Flashcards.Count)
            {
                var question = Flashcards[_currentFlashcardIndex].Question;
                Console.WriteLine($"Question {_currentFlashcardIndex + 1}: {question}");
                await TextToSpeech.SpeakAsync(question);
                _isListeningForAnswer = true;
            }
            else
            {
                // Quiz completed
                await Toast.Make("Quiz completed").Show(CancellationToken.None);
            }
        }

        private void CheckAnswer()
        {
            var currentFlashcard = Flashcards[_currentFlashcardIndex];
            if (_userAnswer.ToLower().Trim() == currentFlashcard.Answer.ToLower().Trim())
            {
                // Correct answer
                // Handle scoring or any other logic
            }
            else
            {
                // Incorrect answer
                // Handle incorrect answer logic if needed
            }
            _currentFlashcardIndex++;
        }
    }
}

*/




/*
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class EachFlashQuizVoiceViewModel : ObservableRecipient
    {
        private string _token;
        private int _currentFlashcardIndex = 0;
        private int _correctAnswers = 0;
        private int _totalQuestions = 0;
        private bool _isListeningForAnswer = false;
        private string _userAnswer = "";
        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }

        public ICommand GoBackCommand { get; private set; }
        public ICommand LogOutCommand { get; private set; }

        public Command ListenCommand { get; set; }
        public Command ListenCancelCommand { get; set; }

        public EachFlashQuizVoiceViewModel(string token, Set selectedSet)
        {
            _token = token;
            Title = selectedSet.Title;
            Flashcards = selectedSet.Flashcards;
            ListenCommand = new Command(StartListening);
            ListenCancelCommand = new Command(StopListening);
            GoBackCommand = new RelayCommand(ExecuteGoBack);
            LogOutCommand = new RelayCommand(ExecuteLogOut);

            SpeakNextQuestion();
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
            Console.WriteLine("Listening for user's answer...");
        }

        private async void StopListening()
        {
            await SpeechToText.StopListenAsync(CancellationToken.None);
            SpeechToText.Default.RecognitionResultUpdated -= OnRecognitionTextUpdated;
            SpeechToText.Default.RecognitionResultCompleted -= OnRecognitionTextCompleted;
            Console.WriteLine("Stopped listening.");
        }

        void OnRecognitionTextUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            if (_isListeningForAnswer)
            {
                _userAnswer = recognitionResult;
                Console.WriteLine($"User's answer (Updated): {_userAnswer}");
            }
        }

        async void OnRecognitionTextCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            Console.WriteLine($"ANSWER: {recognitionResult}");
            if (_isListeningForAnswer)
            {
                _isListeningForAnswer = false;
                CheckAnswer();
                await Task.Delay(1000); // Delay before speaking the next question
                SpeakNextQuestion();
            }
        }

        private async void SpeakNextQuestion()
        {
            if (_currentFlashcardIndex < Flashcards.Count)
            {
                var question = Flashcards[_currentFlashcardIndex].Question;
                Console.WriteLine($"Question {_currentFlashcardIndex + 1}: {question}");
                await TextToSpeech.SpeakAsync(question);
                _isListeningForAnswer = true;
                _totalQuestions++; // Increment total questions
            }
            else
            {
                // Quiz completed
                DisplayScore();
            }
        }

        private void CheckAnswer()
        {
            var currentFlashcard = Flashcards[_currentFlashcardIndex];
            if (_userAnswer.ToLower().Trim() == currentFlashcard.Answer.ToLower().Trim())
            {
                // Correct answer
                _correctAnswers++; // Increment correct answers
                Console.WriteLine($"AAAA {_correctAnswers}");
            }
            _currentFlashcardIndex++;
        }

        private void DisplayScore()
        {
            Console.WriteLine($"Quiz completed.\nCorrect Answers: {_correctAnswers}\nTotal Questions: {_totalQuestions}");
        }
    }
}
*/



/*

using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class EachFlashQuizVoiceViewModel : ObservableRecipient
    {
        private string _token;
        private int _currentFlashcardIndex = 0;
        private int _correctAnswers = 0;
        private int _totalQuestions = 0;
        private bool _isListeningForAnswer = false;
        private string _userAnswer = "";
        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }

        public ICommand GoBackCommand { get; private set; }
        public ICommand LogOutCommand { get; private set; }

        public Command ListenCommand { get; set; }
        public Command ListenCancelCommand { get; set; }

        public EachFlashQuizVoiceViewModel(string token, Set selectedSet)
        {
            _token = token;
            Title = selectedSet.Title;
            Flashcards = selectedSet.Flashcards;
            ListenCommand = new Command(StartListening);
            ListenCancelCommand = new Command(StopListening);
            GoBackCommand = new RelayCommand(ExecuteGoBack);
            LogOutCommand = new RelayCommand(ExecuteLogOut);

            SpeakNextQuestion();
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
            Console.WriteLine("Listening for user's answer...");
        }

        private async void StopListening()
        {
            await SpeechToText.StopListenAsync(CancellationToken.None);
            SpeechToText.Default.RecognitionResultUpdated -= OnRecognitionTextUpdated;
            SpeechToText.Default.RecognitionResultCompleted -= OnRecognitionTextCompleted;
            Console.WriteLine("Stopped listening.");
        }

        void OnRecognitionTextUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            if (_isListeningForAnswer)
            {
                _userAnswer = recognitionResult;
                Console.WriteLine($"User's answer (Updated): {_userAnswer}");
            }
        }

        async void OnRecognitionTextCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            Console.WriteLine($"ANSWER: {recognitionResult}");
            if (_isListeningForAnswer)
            {
                _isListeningForAnswer = false;
                CheckAnswer();
                await Task.Delay(1000); // Delay before speaking the next question
                SpeakNextQuestion();
            }
        }

        private async void SpeakNextQuestion()
        {
            if (_currentFlashcardIndex < Flashcards.Count)
            {
                var question = Flashcards[_currentFlashcardIndex].Question;
                Console.WriteLine($"Question {_currentFlashcardIndex + 1}: {question}");
                await TextToSpeech.SpeakAsync(question);
                _isListeningForAnswer = true;
                _totalQuestions++; // Increment total questions
            }
            else
            {
                // Quiz completed
                DisplayScore();
            }
        }

        private void CheckAnswer()
        {
            var currentFlashcard = Flashcards[_currentFlashcardIndex];
            if (_userAnswer.ToLower().Trim() == currentFlashcard.Answer.ToLower().Trim())
            {
                // Correct answer
                _correctAnswers++; // Increment correct answers
            }
            _currentFlashcardIndex++;
        }

        private void DisplayScore()
        {
            Console.WriteLine($"Quiz completed.\nCorrect Answers: {_correctAnswers}\nTotal Questions: {_totalQuestions}");
        }
    }
}
*/




using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class EachFlashQuizVoiceViewModel : ObservableRecipient
    {
        private string _token;
        private int _currentFlashcardIndex = 0;
        private int _correctAnswers = 0;
        private int _totalQuestions = 0;
        private bool _isListeningForAnswer = false;
        private string _userAnswer = "";
        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }

        public ICommand GoBackCommand { get; private set; }
        public ICommand LogOutCommand { get; private set; }

        public Command ListenCommand { get; set; }
        public Command ListenCancelCommand { get; set; }

        public EachFlashQuizVoiceViewModel(string token, Set selectedSet)
        {
            _token = token;
            Title = selectedSet.Title;
            Flashcards = selectedSet.Flashcards;
            ListenCommand = new Command(StartListening);
            ListenCancelCommand = new Command(StopListening);
            GoBackCommand = new RelayCommand(ExecuteGoBack);
            LogOutCommand = new RelayCommand(ExecuteLogOut);

            SpeakNextQuestion();
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
            Console.WriteLine("Listening for user's answer...");
        }

        private async void StopListening()
        {
            await SpeechToText.StopListenAsync(CancellationToken.None);
            SpeechToText.Default.RecognitionResultUpdated -= OnRecognitionTextUpdated;
            SpeechToText.Default.RecognitionResultCompleted -= OnRecognitionTextCompleted;
            Console.WriteLine("Stopped listening.");
        }

        void OnRecognitionTextUpdated(object? sender, SpeechToTextRecognitionResultUpdatedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            if (_isListeningForAnswer)
            {
                _userAnswer = recognitionResult;
                Console.WriteLine($"User's answer (Updated): {_userAnswer}");
            }
        }

        async void OnRecognitionTextCompleted(object? sender, SpeechToTextRecognitionResultCompletedEventArgs args)
        {
            var recognitionResult = args.RecognitionResult.ToString();
            Console.WriteLine($"ANSWER: {recognitionResult}");
            if (_isListeningForAnswer)
            {
                _isListeningForAnswer = false;
                CheckAnswer(recognitionResult);
                await Task.Delay(1000); // Delay before speaking the next question
                SpeakNextQuestion();
            }
        }

        private async void SpeakNextQuestion()
        {
            if (_currentFlashcardIndex < Flashcards.Count)
            {
                var question = Flashcards[_currentFlashcardIndex].Question;
                Console.WriteLine($"Question {_currentFlashcardIndex + 1}: {question}");
                await TextToSpeech.SpeakAsync(question);
                _isListeningForAnswer = true;
                _totalQuestions++; // Increment total questions
            }
            else
            {
                // Quiz completed
                DisplayScore();
            }
        }

        private void CheckAnswer(string userAnswer)
        {
            var currentFlashcard = Flashcards[_currentFlashcardIndex];
            if (userAnswer.ToLower().Trim() == currentFlashcard.Answer.ToLower().Trim())
            {
                // Correct answer
                _correctAnswers++; // Increment correct answers
            }
            _currentFlashcardIndex++;
        }

        async private void DisplayScore()
        {
            Console.WriteLine($"Quiz completed.\nCorrect Answers: {_correctAnswers}\nTotal Questions: {_totalQuestions}");
            await Shell.Current.Navigation.PushAsync(new ScorePage(_token, _correctAnswers, _totalQuestions));
        }
    }
}
