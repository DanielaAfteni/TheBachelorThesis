using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class FlashcardsViewModel : ObservableRecipient
    {
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                FilterSets(value); // Call FilterSets method when SearchText changes
            }
        }

        private ObservableCollection<Set> _filteredSets;
        public ObservableCollection<Set> FilteredSets
        {
            get => _filteredSets;
            set => SetProperty(ref _filteredSets, value);
        }

        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand<Set> _editSetCommand = null!;
        private RelayCommand<Set> _deleteSetCommand = null!;

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);

        public ICommand EditSetCommand => _editSetCommand ??= new RelayCommand<Set>(ExecuteEditSet);

        public ICommand DeleteSetCommand => _deleteSetCommand ??= new RelayCommand<Set>(ExecuteDeleteSet);

        public ObservableCollection<Set> Sets { get; set; }

        public Command<Set> NavigateToEachFlashcardSetCommand { get; set; }

        public FlashcardsViewModel()
        {
            // JSON data
            string jsonData = @"
        {
            ""sets"": [
                {
                    ""title"": ""Set 111"",
                    ""flashcards"": [
                        { ""question"": ""What is the capital of France?"", ""answer"": ""Paris"" },
                        { ""question"": ""How many continents are there in the world?"", ""answer"": ""7"" },
                        { ""question"": ""Who wrote Romeo and Juliet?"", ""answer"": ""William Shakespeare"" }
                    ]
                },
                {
                    ""title"": ""Set 222"",
                    ""flashcards"": [
                        { ""question"": ""What is the chemical formula for water?"", ""answer"": ""H2O"" },
                        { ""question"": ""What is the tallest mammal on Earth?"", ""answer"": ""Giraffe"" },
                        { ""question"": ""What is the capital of Spain?"", ""answer"": ""Madrid"" }
                    ]
                },
                {
                    ""title"": ""Set 333"",
                    ""flashcards"": [
                        { ""question"": ""Q13"", ""answer"": ""A13"" },
                        { ""question"": ""Q23"", ""answer"": ""A23"" },
                        { ""question"": ""Q33"", ""answer"": ""A33"" }
                    ]
                }
            ]
        }";

            // Deserialize JSON data into a dictionary
            var setsData = JsonConvert.DeserializeObject<Dictionary<string, List<Set>>>(jsonData);

            // Extract the list of sets from the deserialized data
            List<Set> setsList = setsData["sets"];

            // Convert the list to an ObservableCollection
            Sets = new ObservableCollection<Set>(setsList);
            FilteredSets = Sets; // Initialize FilteredSets with all sets

            NavigateToEachFlashcardSetCommand = new Command<Set>(OnSetSelected);

            _editSetCommand = new RelayCommand<Set>(ExecuteEditSet);
            _deleteSetCommand = new RelayCommand<Set>(ExecuteDeleteSet);
        }

        private async void OnSetSelected(Set selectedSet)
        {
            // Navigate to the EachFlashcardSet page and pass the selected set
            Console.WriteLine($"SELECTED {selectedSet.title}");
            await Shell.Current.Navigation.PushAsync(new EachFlashcardSetPage(selectedSet));
            
        }

        private void ExecuteEditSet(Set? selectedSet)
        {
            // Check for nullability before performing actions on selectedSet
            if (selectedSet != null)
            {
                // Perform edit action here
                Console.WriteLine($"Editing set: {selectedSet.title}");
                // Navigate to the editing page
                // Example: await Shell.Current.Navigation.PushAsync(new EditingPage(selectedSet));

                // You can add your logic here to navigate to the editing page
                // For now, let's just update the title of the selected set
                string old_title = selectedSet.title;
                selectedSet.title = "Updated Set Title"; // Update the title as per your requirements
                Console.WriteLine($"The title of the set \"{old_title}\" CHANGED \"{selectedSet.title}\".");
            }
        }

        private void ExecuteDeleteSet(Set? selectedSet)
        {
            // Check for nullability before performing actions on selectedSet
            if (selectedSet != null)
            {
                // Perform edit action here
                Console.WriteLine($"Deleting set: {selectedSet.title}");
                // Navigate to the editing page
                // Example: await Shell.Current.Navigation.PushAsync(new EditingPage(selectedSet));

                // You can add your logic here to navigate to the editing page
                // For now, let's just update the title of the selected set
                Console.WriteLine($"DELETED \"{selectedSet.title}\".");
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

        public void FilterSets(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                FilteredSets = Sets;
            }
            else
            {
                FilteredSets = new ObservableCollection<Set>(Sets.Where(s => s.title.ToLower().Contains(searchText.ToLower())));
            }
        }

    }

    public class Flashcard
    {
        public string question { get; set; }
        public string answer { get; set; }
    }

    // Model for representing a set
    public class Set
    {
        public string title { get; set; }
        public List<Flashcard> flashcards { get; set; }
    }
}
