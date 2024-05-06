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
        public FlashcardsPage(string token)
        {
            InitializeComponent();
            _token = token;
            BindingContext = new FlashcardsViewModel(token);

        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Set selectedSet)
            {
                // Navigate to the EachFlashcardSet page, passing the selected set
                //await Navigation.PushAsync(new EachFlashcardSetPage(selectedSet));
                await Navigation.PushAsync(new EachFlashcardSetPage(_token, selectedSet));
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
