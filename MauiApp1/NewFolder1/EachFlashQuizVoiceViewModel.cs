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
        //public string RecognitionText { get; set; }

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






        public EachFlashQuizVoiceViewModel (Set selectedSet)
        {



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
}
