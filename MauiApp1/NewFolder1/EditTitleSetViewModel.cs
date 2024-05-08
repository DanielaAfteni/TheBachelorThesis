using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class EditTitleSetViewModel : ObservableRecipient
    {
        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;
        private RelayCommand _editTitleCommand;

        private string _token;
        private string _newTitle;
        private string _oldTitle;
        private string _setTitle;

        private Set _selectedSet;

        public Set SelectedSet
        {
            get => _selectedSet;
            set => SetProperty(ref _selectedSet, value);
        }
        public string Title { get; private set; }
        public int Id { get; private set; }
        public List<Flashcard> Flashcards { get; private set; }

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);
        public ICommand EditTitleCommand => _editTitleCommand ??= new RelayCommand(ExecuteEditTitle);


        public string NewTitle
        {
            get => _newTitle;
            set => SetProperty(ref _newTitle, value);
        }
        public string OldTitle
        {
            get => _oldTitle;
            set => SetProperty(ref _oldTitle, value);
        }
        public string SetTitle
        {
            get => _setTitle;
            set => SetProperty(ref _setTitle, value);
        }

        public EditTitleSetViewModel(string token, Set selectedSet)
        {
            _token = token;
            
            SelectedSet = selectedSet;
            Id = selectedSet.Id;
            Title = selectedSet.Title;
            Flashcards = selectedSet.Flashcards;

            Console.WriteLine($"_userId: {_token}");
            Console.WriteLine($"Title: {Title}");
            Console.WriteLine($"Id: {Id}");

            OldTitle = selectedSet.Title;
        }

        private async void ExecuteLogOut()
        {
            // Navigate to the login page
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async void ExecuteGoBack()
        {
            // Navigate back to the previous page
            //await Shell.Current.Navigation.PopAsync();
            await Shell.Current.Navigation.PushAsync(new FlashcardsPage(_token));
        }

        private async void ExecuteEditTitle()
        {
            bool isValid = ValidateTitle();

            if (isValid)
            {
                // Prepare the payload
                var payload = new
                {
                    title = NewTitle
                };

                // Serialize the payload
                var jsonPayload = JsonConvert.SerializeObject(payload);

                // Send PATCH request to the API
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                var response = await client.PatchAsync($"https://assistant-gateway.azurewebsites.net/api/flash-card-sets/{Id}",
                                                       new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and parse the response
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    // Extract the title from the response and set it to SetTitle
                    SetTitle = responseData.entity.title;

                    Console.WriteLine($"SetTitle: {SetTitle}");

                    // Display success message or navigate to another page if needed
                    // For example:
                    // await Shell.Current.Navigation.PushAsync(new SuccessPage());

                    // Display a success message
                    //await Application.Current.MainPage.DisplayAlert("Success", "Title updated successfully.", "OK");
                }
                else
                {
                    Console.WriteLine($"Failed to update title. Status code: {response.StatusCode}");
                    // Display an error message if request fails
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to edit title.", "OK");
                }
            }
            else
            {
                // Display an error message for invalid title
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid title.", "OK");
            }
        }

        private bool ValidateTitle()
        {
            // Perform sign-up validation logic here
            // For demonstration purposes, let's assume validation is successful if all fields are non-empty
            return !string.IsNullOrEmpty(NewTitle);
        }
    }
}
