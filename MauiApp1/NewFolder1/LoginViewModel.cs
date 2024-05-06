/*using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.NewFolder1
{
    public partial class LoginViewModel : ObservableObject
    {
        
    }
}
*/

/*using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text;

namespace MauiApp1.NewFolder1
{
    public partial class LoginViewModel : ObservableObject
    { 
        private string _usernameOrEmail;
        private string _password;
        private RelayCommand _logInCommand;
        private RelayCommand _goToSignUpPageCommand;

        public string UsernameOrEmail
        {
            get => _usernameOrEmail;
            set => SetProperty(ref _usernameOrEmail, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LogInCommand => _logInCommand ??= new RelayCommand(LogIn);

        public ICommand GoToSignUpPageCommand => _goToSignUpPageCommand ??= new RelayCommand(GoToSignUpPage);


        private async void LogIn()
        {
            // Perform login validation
            bool isValid = ValidateLogin();

            if (isValid)
            {
                var payload = new
                {
                    email = UsernameOrEmail,
                    password = Password,
                    twoFactorCode = "string",
                    twoFactorRecoveryCode = "string"
                };
                Console.WriteLine($"email: {UsernameOrEmail}");
                Console.WriteLine($"password: {Password}");
                Console.WriteLine($"twoFactorCode: string");
                Console.WriteLine($"twoFactorRecoveryCode: string");

                string userId = "5be4d3be-0e16-4ad7-ab26-2717fa8087d1";
                //string userId = "587d9ff9-9279-419e-b093-813027fd1bf0";

                // Display userId in the console
                Console.WriteLine($"The LOGGED IN userID is {userId}");
                await Shell.Current.Navigation.PushAsync(new HomePage(userId));
                var jsonPayload = JsonConvert.SerializeObject(payload);

                // Send POST request to the API
                using var client = new HttpClient();
                var response = await client.PostAsync("https://users-indentity-api.azurewebsites.net/login",
                                                       new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    // Read and parse the response
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    // Extract the userId from the response
                    string userId = "587d9ff9-9279-419e-b093-813027fd1bf0";

                    // Display userId in the console
                    Console.WriteLine($"The LOGGED IN userID is {userId}");
                    await Shell.Current.Navigation.PushAsync(new HomePage(userId));
                }
                else
                {
                    Console.WriteLine($"Failed to login user. Status code: {response.StatusCode}");
                    // Display an error message if request fails
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to register user.", "OK");
                }

            }
            else
            {
                // Display an error message
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid username or password.", "OK");
            }
        }

        private bool ValidateLogin()
        {
            // Perform login validation logic here
            // For demonstration purposes, let's assume validation is successful if both fields are non-empty
            return !string.IsNullOrEmpty(UsernameOrEmail) && !string.IsNullOrEmpty(Password);
        }

        private async void GoToSignUpPage()
        {
            // Navigate to the sign-up page using absolute routing
            await Shell.Current.GoToAsync($"{nameof(SignUpPage)}");
        }
    }
}*/

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class LoginViewModel : ObservableObject
    {
        private string _usernameOrEmail;
        private string _password;
        private RelayCommand _logInCommand;
        private RelayCommand _goToSignUpPageCommand;

        public string UsernameOrEmail
        {
            get => _usernameOrEmail;
            set => SetProperty(ref _usernameOrEmail, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ICommand LogInCommand => _logInCommand ??= new RelayCommand(LogIn);

        public ICommand GoToSignUpPageCommand => _goToSignUpPageCommand ??= new RelayCommand(GoToSignUpPage);

        private async void LogIn()
        {
            // Perform login validation
            bool isValid = ValidateLogin();

            if (isValid)
            {
                var payload = new
                {
                    email = UsernameOrEmail,
                    password = Password
                };

                var jsonPayload = JsonConvert.SerializeObject(payload);

                // Send POST request to the API
                using var client = new HttpClient();
                var response = await client.PostAsync("https://users-indentity-api.azurewebsites.net/api/user/login",
                                                       new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    // Read and parse the response
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    // Extract the token from the response
                    string token = responseData.token;

                    // Display token in the console
                    Console.WriteLine($"The received token is {token}");

                    // Navigate to the home page with the token
                    await Shell.Current.Navigation.PushAsync(new HomePage(token));
                }
                else
                {
                    Console.WriteLine($"Failed to login user. Status code: {response.StatusCode}");
                    // Display an error message if request fails
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to login user.", "OK");
                }

            }
            else
            {
                // Display an error message
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid username or password.", "OK");
            }
        }

        private bool ValidateLogin()
        {
            // Perform login validation logic here
            // For demonstration purposes, let's assume validation is successful if both fields are non-empty
            return !string.IsNullOrEmpty(UsernameOrEmail) && !string.IsNullOrEmpty(Password);
        }

        private async void GoToSignUpPage()
        {
            // Navigate to the sign-up page using absolute routing
            await Shell.Current.GoToAsync($"{nameof(SignUpPage)}");
        }
    }
}

