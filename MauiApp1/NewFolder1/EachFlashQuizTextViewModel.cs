﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class EachFlashQuizTextViewModel : ObservableRecipient
    {
        public string Title { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }
        public List<string> UserAnswers { get; } = new List<string>(); // List to store user's answers

        public List<bool> IsCorrect { get; } = new List<bool>();

        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand _checkAnswersCommand;

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);
        public ICommand CheckAnswersCommand => _checkAnswersCommand ??= new RelayCommand(ExecuteCheckAnswers);

        public EachFlashQuizTextViewModel(Set selectedSet)
        {
            Title = selectedSet.title;
            Flashcards = selectedSet.flashcards;
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

        private async void ExecuteCheckAnswers()
        {
            int correctCount = 0;
            for (int i = 0; i < Flashcards.Count; i++)
            {
                if (i < UserAnswers.Count && Flashcards[i].answer == UserAnswers[i])
                {
                    correctCount++;
                }
            }

            //int totalQuestions = Math.Min(Flashcards.Count, UserAnswers.Count);
            int totalQuestions = Flashcards.Count;
            Console.WriteLine($"Total correct answers: {correctCount}/{totalQuestions}");
            //await Shell.Current.GoToAsync($"//{nameof(ScorePage)}?correctCount={correctCount}&totalQuestions={totalQuestions}");
            await Shell.Current.Navigation.PushAsync(new ScorePage(correctCount, totalQuestions));
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