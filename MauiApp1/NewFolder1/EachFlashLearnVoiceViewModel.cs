﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class EachFlashLearnVoiceViewModel : ObservableRecipient
    {
        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }

        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);

        public EachFlashLearnVoiceViewModel(Set selectedSet)
        {
            Title = selectedSet.title;
            Flashcards = selectedSet.flashcards;
            SpeakQuestions();
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
        private async void SpeakQuestions()
        {
            foreach (var flashcard in Flashcards)
            {
                if (!string.IsNullOrEmpty(flashcard.question))
                {
                    await TextToSpeech.SpeakAsync(flashcard.question);
                    await TextToSpeech.SpeakAsync(flashcard.answer);
                }
            }
        }
    }
}