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

        /*public FlashcardsViewModel(string userId)
        {
            _userId = userId;
            Console.WriteLine($"USER {_userId}");
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
        }*/

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
                dynamic setsEntity = JsonConvert.DeserializeObject(setsData); // Deserialize dynamically
                Console.WriteLine($"Sets Entity: {setsEntity}");

                // Check if the required properties exist in the JSON response
                if (setsEntity?.entity?.items != null)
                {
                    var setItems = setsEntity.entity.items;
                    Console.WriteLine($"Set Items: {setItems}");

                    // Initialize setTitles as a new list of tuples
                    //Sets = new ObservableCollection<Set>(setsList);
                    var sets = new List<Set>(); // Initialize a list of Set objects

                    foreach (var item in setItems)
                    {
                        var setId = (int)item.id; // Extract set Id
                        var setTitle = (string)item.title; // Extract set Title
                        Console.WriteLine($"Fetching flashcards for set with Id: {setId} and Title: {setTitle}");

                        // Fetch flashcards data for the current set
                        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                        var flashcardsResponse = _httpClient.GetAsync($"https://assistant-gateway.azurewebsites.net/api/flash-card-sets/flash-cards/{setId}?pageSize=100&pageNumber=1").Result;

                        Console.WriteLine($"Fetch response for set with Id: {setId} and Title: {setTitle}: StatusCode: {flashcardsResponse.StatusCode}, ReasonPhrase: {flashcardsResponse.ReasonPhrase}, Version: {flashcardsResponse.Version}");

                        if (flashcardsResponse.IsSuccessStatusCode)
                        {
                            // Read and parse the flashcards response
                            var flashcardsResponseContent = flashcardsResponse.Content.ReadAsStringAsync().Result;
                            dynamic flashcardsResponseData = JsonConvert.DeserializeObject(flashcardsResponseContent); // Deserialize dynamically
                            Console.WriteLine($"Flashcards response data: {flashcardsResponseData}");

                            // Extract relevant data from the response
                            var flashcardItems = flashcardsResponseData?.entity?.items;

                            if (flashcardItems != null)
                            {
                                // Initialize a list of Flashcard objects
                                var flashcards = new List<Flashcard>();

                                // Process each flashcard item
                                foreach (var flashcardItem in flashcardItems)
                                {
                                    // Extract flashcard details
                                    var flashcardId = (int)flashcardItem.id;
                                    var flashcardQuestion = (string)flashcardItem.question;
                                    var flashcardAnswer = (string)flashcardItem.answer;

                                    // Create a new Flashcard object
                                    var flashcard = new Flashcard
                                    {
                                        Id = flashcardId,
                                        Question = flashcardQuestion,
                                        Answer = flashcardAnswer
                                    };

                                    // Add the flashcard to the list
                                    flashcards.Add(flashcard);

                                    // Output flashcard details
                                    Console.WriteLine($"Flashcard Question: {flashcard.Question}");
                                    Console.WriteLine($"Flashcard Answer: {flashcard.Answer}");
                                }

                                // Create a new Set object
                                var set = new Set
                                {
                                    Id = setId,
                                    Title = setTitle,
                                    Flashcards = flashcards // Assign the list of flashcards to the set
                                };

                                // Add the set to the list of sets
                                sets.Add(set);
                            }
                            else
                            {
                                Console.WriteLine($"No flashcards found for set with Id: {setId} and Title: {setTitle}.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Failed to fetch flashcards for set with Id: {setId} and Title: {setTitle}. StatusCode: {flashcardsResponse.StatusCode}, ReasonPhrase: {flashcardsResponse.ReasonPhrase}");
                        }
                    }
                    Sets = new ObservableCollection<Set>(sets);
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






            //LoadSets();
            if (Sets != null)
            {
                foreach (var set in Sets)
                {
                    Console.WriteLine($"Set Id: {set.Id}");
                    Console.WriteLine($"Set Title: {set.Title}");
                }
            }
        }


        /*private async void LoadSets()
        {
            // Fetch sets data from the API
            var setsResponse = await _httpClient.GetAsync($"https://flash-cards-api.azurewebsites.net/api/flash-card-sets/{_userId}?pageNumber=1&pageSize=4");

            // Inside the LoadSets method
            // Inside the LoadSets method
            if (setsResponse.IsSuccessStatusCode)
            {
                var setsData = await setsResponse.Content.ReadAsStringAsync();
                var setsEntity = JObject.Parse(setsData);
                Console.WriteLine($"Sets Entity: {setsEntity}");

                // Check if the required properties exist in the JSON response
                if (setsEntity["entity"] != null && setsEntity["entity"]["items"] != null)
                {
                    var setItems = setsEntity["entity"]["items"];
                    Console.WriteLine($"Set Items: {setItems}");

                    // Initialize setTitles as a new list of tuples
                    var setTitles = setItems.Select(item => new { Id = (int)item["id"], Title = (string)item["title"] }).ToList();


                    foreach (var setTitle in setTitles)
                    {
                        var setId = setTitle.Id.ToString(); ;
                        Console.WriteLine($"Fetching flashcards for set with Id: {setId} and Title: {setTitle.Title}");

                        try
                        {
                            var flashcardsResponse = await _httpClient.GetAsync($"https://flash-cards-api.azurewebsites.net/api/flash-card-sets/flash-cards/{_userId}/{setId}?pageNumber=1&pageSize=4");
                            Console.WriteLine($"Fetch response for set with Id: {setId} and Title: {setTitle.Title}: {flashcardsResponse}");

                            if (flashcardsResponse.IsSuccessStatusCode)
                            {
                                var flashcardsData = await flashcardsResponse.Content.ReadAsStringAsync();
                                var flashcardsEntity = JObject.Parse(flashcardsData);

                                // Check if the required properties exist in the flashcards response
                                if (flashcardsEntity != null && flashcardsEntity["entity"] != null && flashcardsEntity["entity"]["items"] != null)
                                {
                                    var flashcardItems = flashcardsEntity["entity"]["items"];
                                    if (flashcardItems != null && flashcardItems.HasValues)
                                    {
                                        var flashcards = flashcardItems.Select(item => new Flashcard
                                        {
                                            question = item["question"]?.ToString(), // Add null check
                                            answer = item["answer"]?.ToString() // Add null check
                                        }).ToList();

                                        // Create a new set with the fetched flashcards
                                        var set = new Set
                                        {
                                            title = setTitle.Title,
                                            flashcards = flashcards
                                        };

                                        Sets.Add(set);
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Flashcard items are null or empty for set with Id: {setId} and Title: {setTitle.Title}.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Required properties are missing in the flashcards response for set with Id: {setId} and Title: {setTitle.Title}.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Failed to fetch flashcards for set with Id: {setId} and Title: {setTitle.Title}. Status code: {flashcardsResponse.StatusCode}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occurred while fetching flashcards for set with Id: {setId} and Title: {setTitle.Title}: {ex.Message}");
                        }
                    }


                }
            }

        }*/

        /*private async void LoadSets()
        {
            // Fetch sets data from the API
            var setsResponse = await _httpClient.GetAsync($"https://flash-cards-api.azurewebsites.net/api/flash-card-sets/{_userId}?pageNumber=1&pageSize=4");

            // Inside the LoadSets method
            if (setsResponse.IsSuccessStatusCode)
            {
                var setsData = await setsResponse.Content.ReadAsStringAsync();
                dynamic setsEntity = JsonConvert.DeserializeObject(setsData); // Deserialize dynamically
                Console.WriteLine($"Sets Entity: {setsEntity}");

                // Check if the required properties exist in the JSON response
                if (setsEntity?.entity?.items != null)
                {
                    var setItems = setsEntity.entity.items;
                    Console.WriteLine($"Set Items: {setItems}");

                    // Initialize setTitles as a new list of tuples
                    var setTitles = new List<(int Id, string Title)>();
                    foreach (var item in setItems)
                    {
                        setTitles.Add(((int)item.id, (string)item.title)); // Add tuple to list
                    }

                    foreach (var setTitle in setTitles)
                    {
                        var setId = setTitle.Id.ToString(); // Convert Id to string
                        Console.WriteLine($"Fetching flashcards for set with Id: {setId} and Title: {setTitle.Title}");

                        var flashcardsResponse = await _httpClient.GetAsync($"https://flash-cards-api.azurewebsites.net/api/flash-card-sets/flash-cards/{_userId}/{setId}?pageNumber=1&pageSize=4");

                        Console.WriteLine($"Fetch response for set with Id: {setId} and Title: {setTitle.Title}: StatusCode: {flashcardsResponse.StatusCode}, ReasonPhrase: {flashcardsResponse.ReasonPhrase}, Version: {flashcardsResponse.Version}");

                        if (flashcardsResponse.IsSuccessStatusCode)
                        {
                            // Read and parse the response
                            var flashcardsResponseContent = await flashcardsResponse.Content.ReadAsStringAsync();
                            dynamic flashcardsResponseData = JsonConvert.DeserializeObject(flashcardsResponseContent); // Deserialize dynamically
                            Console.WriteLine($"Flashcards response data: {flashcardsResponseData}");

                            // Extract relevant data from the response
                            var flashcards = flashcardsResponseData?.entity?.items;

                            if (flashcards != null)
                            {
                                // Process the flashcards data
                                foreach (var flashcard in flashcards)
                                {
                                    // Extract question and answer from each flashcard
                                    var question = flashcard.question?.ToString();
                                    var answer = flashcard.answer?.ToString();
                                    Console.WriteLine($"Flashcard Question: {question}");
                                    Console.WriteLine($"Flashcard Answer: {answer}");
                                    // Process the question and answer...
                                }
                            }
                            else
                            {
                                Console.WriteLine($"No flashcards found for set with Id: {setId} and Title: {setTitle.Title}.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Failed to fetch flashcards for set with Id: {setId} and Title: {setTitle.Title}. StatusCode: {flashcardsResponse.StatusCode}, ReasonPhrase: {flashcardsResponse.ReasonPhrase}");
                        }
                    }
                }
            }
        }*/

        private async void ExecuteGoToAddSetCommand()
        {
            bool isValid = ValidateTitle();

            if(isValid)
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
                    //await Shell.Current.Navigation.PushAsync(new FlashcardsPage(_userId));
                    //await Shell.Current.GoToAsync($"{nameof(AddSetsPage)}");
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

        /*private async Task LoadSets()
        {
            // Fetch sets data from the API
            var setsResponse = await _httpClient.GetAsync($"https://flash-cards-api.azurewebsites.net/api/flash-card-sets/{_userId}?pageNumber=1&pageSize=4");

            // Inside the LoadSets method
            if (setsResponse.IsSuccessStatusCode)
            {
                var setsData = await setsResponse.Content.ReadAsStringAsync();
                dynamic setsEntity = JsonConvert.DeserializeObject(setsData); // Deserialize dynamically
                Console.WriteLine($"Sets Entity: {setsEntity}");

                // Check if the required properties exist in the JSON response
                if (setsEntity?.entity?.items != null)
                {
                    var setItems = setsEntity.entity.items;
                    Console.WriteLine($"Set Items: {setItems}");

                    // Initialize setTitles as a new list of tuples
                    //Sets = new ObservableCollection<Set>(setsList);
                    var sets = new List<Set>(); // Initialize a list of Set objects

                    foreach (var item in setItems)
                    {
                        var setId = (int)item.id; // Extract set Id
                        var setTitle = (string)item.title; // Extract set Title
                        Console.WriteLine($"Fetching flashcards for set with Id: {setId} and Title: {setTitle}");

                        // Fetch flashcards data for the current set
                        var flashcardsResponse = await _httpClient.GetAsync($"https://flash-cards-api.azurewebsites.net/api/flash-card-sets/flash-cards/{_userId}/{setId}?pageNumber=1&pageSize=4");

                        Console.WriteLine($"Fetch response for set with Id: {setId} and Title: {setTitle}: StatusCode: {flashcardsResponse.StatusCode}, ReasonPhrase: {flashcardsResponse.ReasonPhrase}, Version: {flashcardsResponse.Version}");

                        if (flashcardsResponse.IsSuccessStatusCode)
                        {
                            // Read and parse the flashcards response
                            var flashcardsResponseContent = await flashcardsResponse.Content.ReadAsStringAsync();
                            dynamic flashcardsResponseData = JsonConvert.DeserializeObject(flashcardsResponseContent); // Deserialize dynamically
                            Console.WriteLine($"Flashcards response data: {flashcardsResponseData}");

                            // Extract relevant data from the response
                            var flashcardItems = flashcardsResponseData?.entity?.items;

                            if (flashcardItems != null)
                            {
                                // Initialize a list of Flashcard objects
                                var flashcards = new List<Flashcard>();

                                // Process each flashcard item
                                foreach (var flashcardItem in flashcardItems)
                                {
                                    // Extract flashcard details
                                    var flashcardId = (int)flashcardItem.id;
                                    var flashcardQuestion = (string)flashcardItem.question;
                                    var flashcardAnswer = (string)flashcardItem.answer;

                                    // Create a new Flashcard object
                                    var flashcard = new Flashcard
                                    {
                                        Id = flashcardId,
                                        Question = flashcardQuestion,
                                        Answer = flashcardAnswer
                                    };

                                    // Add the flashcard to the list
                                    flashcards.Add(flashcard);

                                    // Output flashcard details
                                    Console.WriteLine($"Flashcard Question: {flashcard.Question}");
                                    Console.WriteLine($"Flashcard Answer: {flashcard.Answer}");
                                }

                                // Create a new Set object
                                var set = new Set
                                {
                                    Id = setId,
                                    Title = setTitle,
                                    Flashcards = flashcards // Assign the list of flashcards to the set
                                };

                                // Add the set to the list of sets
                                sets.Add(set);
                            }
                            else
                            {
                                Console.WriteLine($"No flashcards found for set with Id: {setId} and Title: {setTitle}.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Failed to fetch flashcards for set with Id: {setId} and Title: {setTitle}. StatusCode: {flashcardsResponse.StatusCode}, ReasonPhrase: {flashcardsResponse.ReasonPhrase}");
                        }
                    }
                    Sets = new ObservableCollection<Set>(sets);
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
        }*/




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
                // Perform edit action here
                Console.WriteLine($"Editing set: {selectedSet.Title}");
                // Navigate to the editing page
                // Example: await Shell.Current.Navigation.PushAsync(new EditingPage(selectedSet));

                // You can add your logic here to navigate to the editing page
                // For now, let's just update the title of the selected set
                //string old_title = selectedSet.Title;
                //selectedSet.Title = "Updated Set Title"; // Update the title as per your requirements
                //Console.WriteLine($"The title of the set \"{old_title}\" CHANGED \"{selectedSet.Title}\".");
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
            // Navigate back to the previous page
            //await Shell.Current.Navigation.PopAsync();
            //await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
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

    /*public class Flashcard
    {
        public string question { get; set; }
        public string answer { get; set; }
    }

    // Model for representing a set
    public class Set
    {
        public string title { get; set; }
        public List<Flashcard> flashcards { get; set; }
    }*/
    public class Flashcard
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    // Model for representing a set
    public class Set
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Flashcard> Flashcards { get; set; }
    }
}
