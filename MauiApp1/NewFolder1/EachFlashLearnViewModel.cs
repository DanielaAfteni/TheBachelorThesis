using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class EachFlashLearnViewModel : ObservableRecipient
    {

        private Set _selectedSet;

        public Set SelectedSet
        {
            get => _selectedSet;
            set => SetProperty(ref _selectedSet, value);
        }
        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }

        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand<Set> _navigateCommand;
        private bool _isSwitchToggled;

        public EachFlashLearnViewModel(Set selectedSet)
        {
            SelectedSet = selectedSet;
            Title = selectedSet.Title;
            Flashcards = selectedSet.Flashcards;
        }


        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);
        public ICommand NavigateCommand => _navigateCommand ??= new RelayCommand<Set>(ExecuteNavigate);

        public bool IsSwitchToggled
        {
            get => _isSwitchToggled;
            set
            {
                SetProperty(ref _isSwitchToggled, value);
                Console.WriteLine($"IsSwitchToggled CHANGED");
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

        private async void ExecuteNavigate(Set? selectedSet)
        {
            if (selectedSet == null)
            {
                // Handle the case where selectedSet is null
                return;
            }

            Console.WriteLine($"Navigation SELECTED");
            if (IsSwitchToggled)
            {
                // Navigate to the voice page
                await Shell.Current.Navigation.PushAsync(new EachFlashLearnVoicePage(selectedSet));
                Console.WriteLine($"EachFlashQuizVoicePage SELECTED");
                Console.WriteLine($"{selectedSet.Title}");
            }
            else
            {
                // Navigate to the text page
                await Shell.Current.Navigation.PushAsync(new EachFlashLearnTextPage(selectedSet));
                Console.WriteLine($"EachFlashQuizTextPage SELECTED");
                Console.WriteLine($"{selectedSet.Title}");
            }
        }

    }
}
