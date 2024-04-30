using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MauiApp1.NewFolder1; // Make sure to include the namespace of FlashcardsViewModel

namespace MauiApp1
{
    public partial class FlashcardsPage : ContentPage
    {
        private string _userId;
        public FlashcardsPage(string userId)
        {
            InitializeComponent(); // Make sure InitializeComponent() method is generated
            _userId = userId;
            BindingContext = new FlashcardsViewModel(userId);

        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Set selectedSet)
            {
                // Navigate to the EachFlashcardSet page, passing the selected set
                //await Navigation.PushAsync(new EachFlashcardSetPage(selectedSet));
                await Navigation.PushAsync(new EachFlashcardSetPage(_userId, selectedSet));
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
