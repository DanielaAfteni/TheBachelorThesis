using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class ScoreViewModel : ObservableRecipient
    {
        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand _goToTryAgainCommand;
        private RelayCommand _goToHomePageCommand;


        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);

        public ICommand GoToTryAgainCommand => _goToTryAgainCommand ??= new RelayCommand(ExecuteGoTryAgain);
        public ICommand GoToHomePageCommand => _goToHomePageCommand ??= new RelayCommand(ExecuteGoHomePage);


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

        private async void ExecuteGoTryAgain()
        {
            // Navigate back to the previous page
            await Shell.Current.Navigation.PopAsync();
        }

        private async void ExecuteGoHomePage()
        {
            // Navigate to the login page
            await Shell.Current.GoToAsync($"{nameof(HomePage)}");
        }



        private string _scoreText;
        public string ScoreText
        {
            get => _scoreText;
            set => SetProperty(ref _scoreText, value);
        }

        private Color _frameColor;
        public Color FrameColor
        {
            get => _frameColor;
            set => SetProperty(ref _frameColor, value);
        }

        public ScoreViewModel(int correctCount, int totalQuestions)
        {
            CalculateScore(correctCount, totalQuestions);
        }

        private void CalculateScore(int correctCount, int totalQuestions)
        {
            double percentage = (double)correctCount / totalQuestions;

            ScoreText = $"{correctCount}/{totalQuestions}";

            if (percentage >= 0.5)
            {
                FrameColor = Color.FromRgb(93, 176, 117);
            }
            else
            {
                FrameColor = Color.FromRgb(252, 91, 91);
            }
        }
    }
}
