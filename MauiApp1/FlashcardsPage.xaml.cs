using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MauiApp1.NewFolder1; 

namespace MauiApp1
{
    public partial class FlashcardsPage : ContentPage
    {
        private string _token;
        private string _nickname;
        private string _group;
        private string _email;
        public FlashcardsPage(string token, string email, string group, string nickname)
        {
            InitializeComponent();
            _token = token;
            _nickname = nickname;
            _group = group;
            _email = email;
            BindingContext = new FlashcardsViewModel(token, email, group, nickname);

        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Set selectedSet)
            {
                // Navigate to the EachFlashcardSet page, passing the selected set
                //await Navigation.PushAsync(new EachFlashcardSetPage(selectedSet));
                await Navigation.PushAsync(new EachFlashcardSetPage(_token, _email, _group, _nickname, selectedSet));
            }

            // Clear the selection
            ((CollectionView)sender).SelectedItem = null;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (BindingContext is FlashcardsViewModel viewModel)
            {
                viewModel.FilterSets(e.NewTextValue);
            }
        }
    }
}
