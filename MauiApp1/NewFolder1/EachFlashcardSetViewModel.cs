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

        public Set SelectedSet
        {
            get => _selectedSet;
            set => SetProperty(ref _selectedSet, value);
        }

        public EachFlashcardSetViewModel(Set selectedSet)
        {
            SelectedSet = selectedSet;
            Title = selectedSet.title;
            Flashcards = selectedSet.flashcards;
        }

        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }

        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;
        //private RelayCommand _startQuizCommand;
        //private RelayCommand _startLearnCommand;
        private RelayCommand<Set> _startQuizCommand;
        private RelayCommand<Set> _startLearnCommand;

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);
        //public ICommand StartQuizCommand => _startQuizCommand ??= new RelayCommand(ExecuteStartQuiz);

        public ICommand StartQuizCommand => _startQuizCommand ??= new RelayCommand<Set>(ExecuteStartQuiz);
        //public ICommand StartLearnCommand => _startLearnCommand ??= new RelayCommand(ExecuteStartLearn);

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
            await Shell.Current.Navigation.PushAsync(new EachFlashQuizPage(selectedSet));
            //await Shell.Current.GoToAsync($"{nameof(EachFlashQuizPage)}");
            Console.WriteLine($"QUIZ selected");
            // Implement your logic for starting the quiz mode
        }

        private async void ExecuteStartLearn(Set? selectedSet)
        {
            if (selectedSet == null)
            {
                // Handle the case where selectedSet is null
                return;
            }
            //await Shell.Current.GoToAsync($"{nameof(EachFlashLearnPage)}");
            await Shell.Current.Navigation.PushAsync(new EachFlashLearnPage(selectedSet));

            Console.WriteLine($"LEARN selected");
            // Implement your logic for starting the learn mode
        }
    }
}
