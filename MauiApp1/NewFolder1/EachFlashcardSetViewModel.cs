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
    public partial class EachFlashcardSetViewModel : ObservableRecipient
    {
        private Set _selectedSet;
        private string _token;
        private string _nickname;
        private string _group;
        private string _email;

        public Set SelectedSet
        {
            get => _selectedSet;
            set => SetProperty(ref _selectedSet, value);
        }

        public EachFlashcardSetViewModel(string token, string email, string group, string nickname, Set selectedSet)
        {
            _token = token;
            _nickname = nickname;
            _group = group;
            _email = email;
            SelectedSet = selectedSet;
            Title = selectedSet.Title;
            Flashcards = selectedSet.Flashcards;
        }

        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }

        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand<Set> _startQuizCommand;
        private RelayCommand<Set> _startLearnCommand;

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);
        public ICommand StartQuizCommand => _startQuizCommand ??= new RelayCommand<Set>(ExecuteStartQuiz);
        public ICommand StartLearnCommand => _startLearnCommand ??= new RelayCommand<Set>(ExecuteStartLearn);

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

        private async void ExecuteStartQuiz(Set? selectedSet)
        {
            if (selectedSet == null)
            {
                // Handle the case where selectedSet is null
                return;
            }
            await Shell.Current.Navigation.PushAsync(new EachFlashQuizPage(_token, _email, _group, _nickname, selectedSet));
            Console.WriteLine($"QUIZ selected");
        }

        private async void ExecuteStartLearn(Set? selectedSet)
        {
            if (selectedSet == null)
            {
                // Handle the case where selectedSet is null
                return;
            }
            await Shell.Current.Navigation.PushAsync(new EachFlashLearnPage(_token, selectedSet));
            Console.WriteLine($"LEARN selected");
        }
    }
}
