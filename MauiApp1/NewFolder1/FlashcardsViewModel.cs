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
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Text.RegularExpressions;

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

        private string _titleNewSet;
        private RelayCommand _goToAddSetCommand;
        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand<Set> _editSetCommand = null!;
        private RelayCommand<Set> _deleteSetCommand = null!;

        public string TitleNewSet
        {
            get => _titleNewSet;
            set => SetProperty(ref _titleNewSet, value);
        }

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);

        public ICommand GoToAddSetCommand => _goToAddSetCommand ??= new RelayCommand(ExecuteGoToAddSetCommand);

        public ICommand EditSetCommand => _editSetCommand ??= new RelayCommand<Set>(ExecuteEditSet);

        public ICommand DeleteSetCommand => _deleteSetCommand ??= new RelayCommand<Set>(ExecuteDeleteSet);

        public ObservableCollection<Set> Sets { get; set; }

        public Command<Set> NavigateToEachFlashcardSetCommand { get; set; }

        private string _token;
        private string _nickname;
        private string _group;
        private string _email;

        private HttpClient _httpClient;


        public FlashcardsViewModel(string token, string email, string group, string nickname)
        {
            _token = token;
            _nickname = nickname;
            _group = group;
            _email = email;
            _httpClient = new HttpClient();

            Sets = new ObservableCollection<Set>();


            // Fetch sets data from the API
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            var setsResponse = _httpClient.GetAsync($"https://assistant-gateway.azurewebsites.net/api/flash-card-sets?pageSize=100&pageNumber=1").Result;

            // Inside the LoadSets method
            if (setsResponse.IsSuccessStatusCode)
            {
                var setsData = setsResponse.Content.ReadAsStringAsync().Result;
                var setsEntity = JsonConvert.DeserializeObject<GetEntityResponse<Set>>(setsData); 
                Console.WriteLine($"Sets Entity: {setsEntity}");

                // Check if the required properties exist in the JSON response
                if (setsEntity?.Entity.Items != null)
                {
                    var setItems = setsEntity.Entity.Items;
                    Console.WriteLine($"Set Items: {setItems}");


                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                    var allFlashCardsResponse = _httpClient.GetAsync($"https://flash-cards-api.azurewebsites.net/api/flash-cards?pageSize=100&pageNumber=1").GetAwaiter().GetResult();
                    if (allFlashCardsResponse.IsSuccessStatusCode)
                    {
                        var allFlashCardsResponseContent = allFlashCardsResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        var allFlashCardsResponseData = JsonConvert.DeserializeObject<GetEntityResponse<Flashcard>>(allFlashCardsResponseContent);
                        var allFlashCards = allFlashCardsResponseData?.Entity.Items;
                        foreach (var item in setItems)
                        {
                            var setId = item.Id; // Extract set Id
                            var setTitle = item.Title; // Extract set Title
                            Console.WriteLine($"Fetching flashcards for set with Id: {setId} and Title: {setTitle}");

                            // Fetch flashcards data for the current set
                            var setFlashCards = allFlashCards?.Where(x => x.SetId == setId).ToList();
                            item.Flashcards = setFlashCards ?? [];
                        }
                    }

                    Sets = new ObservableCollection<Set>(setsEntity.Entity.Items);
                    foreach (var s1 in Sets)
                    {
                        Console.WriteLine(s1.Id);
                        Console.WriteLine(s1.Title);
                        Console.WriteLine(s1.Flashcards);
                        foreach (var f1 in s1.Flashcards)
                        {
                            Console.WriteLine();
                            Console.WriteLine(f1.Id);
                            Console.WriteLine(f1.Question);
                            Console.WriteLine(f1.Answer);
                            Console.WriteLine();

                        }
                    }

                }
            }
            if (Sets != null)
            {
                foreach (var set in Sets)
                {
                    Console.WriteLine($"Set Id: {set.Id}");
                    Console.WriteLine($"Set Title: {set.Title}");
                }
            }
        }

        private async void ExecuteGoToAddSetCommand()
        {
            bool isValid = ValidateTitle();

            if (isValid)
            {
                var payload = new
                {
                    title = TitleNewSet,
                };
                Console.WriteLine($"title: {TitleNewSet}");
                Console.WriteLine($"userId: {_token}");
                var jsonPayload = JsonConvert.SerializeObject(payload);

                // Send POST request to the API
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                var response = await client.PostAsync("https://assistant-gateway.azurewebsites.net/api/flash-card-sets",
                                                       new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    // Read and parse the response
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    // Extract the userId from the response
                    string setId = responseData.id;
                    string setTitle = responseData.title;

                    // Display userId in the console
                    Console.WriteLine($"The Set ID is {setId}");
                    Console.WriteLine($"The Set Title is {setTitle}");
                    await Shell.Current.Navigation.PushAsync(new AddSetsPage(_token, _email, _group, _nickname, setId));
                }
                else
                {
                    Console.WriteLine($"Failed to add set. Status code: {response.StatusCode}");
                    // Display an error message if request fails
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to add set.", "OK");
                }
            }
        }

        private bool ValidateTitle()
        {
            return !string.IsNullOrEmpty(TitleNewSet);
        }

        private async void OnSetSelected(Set selectedSet)
        {
            // Navigate to the EachFlashcardSet page and pass the selected set
            Console.WriteLine($"SELECTED {selectedSet.Title}");
            await Shell.Current.Navigation.PushAsync(new EachFlashcardSetPage(_token, _email, _group, _nickname, selectedSet));

        }

        private async void ExecuteEditSet(Set? selectedSet)
        {
            // Check for nullability before performing actions on selectedSet
            if (selectedSet != null)
            {
                Console.WriteLine($"Editing set: {selectedSet.Title}");
                await Shell.Current.Navigation.PushAsync(new EditTitleSetPage(_token, _email, _group, _nickname, selectedSet));
            }
        }

        private async void ExecuteDeleteSet(Set? selectedSet)
        {
            // Check for nullability before performing actions on selectedSet
            if (selectedSet != null)
            {
                Console.WriteLine($"Deleting set: {selectedSet.Title}");

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                var response = await client.DeleteAsync($"https://assistant-gateway.azurewebsites.net/api/flash-card-sets/{selectedSet.Id}");

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"DELETED \"{selectedSet.Title}\".");
                    await Shell.Current.Navigation.PushAsync(new FlashcardsPage(_token, _email, _group, _nickname));

                }
                else
                {
                    Console.WriteLine($"Failed to delete set. Status code: {response.StatusCode}");
                    // Display an error message if request fails
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to delete set.", "OK");
                }
            }
        }

        private async void ExecuteGoBack()
        {
            await Shell.Current.Navigation.PushAsync(new HomePage(_token, _email, _group, _nickname));
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
                FilteredSets = new ObservableCollection<Set>(Sets.Where(s => s.Title.ToLower().Contains(searchText.ToLower())));
            }
        }

    }

    public class Flashcard
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int SetId { get; set; }
    }

    public class Set
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Flashcard> Flashcards { get; set; }
    }
    public class GetEntityResponse<T>
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public Entity<T> Entity { get; set; }
    }
    public class Entity<T>
    {
        public List<T> Items { get; set; }
    }
}