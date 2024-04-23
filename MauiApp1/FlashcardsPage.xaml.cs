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
        public FlashcardsPage(FlashcardsViewModel vm)
        {
            InitializeComponent(); // Make sure InitializeComponent() method is generated
            BindingContext = vm;
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Set selectedSet)
            {
                // Navigate to the EachFlashcardSet page, passing the selected set
                await Navigation.PushAsync(new EachFlashcardSetPage(selectedSet));
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
