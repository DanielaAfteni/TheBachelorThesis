/*using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.NewFolder1
{

    [QueryProperty("Text", "Text")]
    public partial class SignUpViewModel : ObservableObject
    {
        [ObservableProperty]
        string text;

        [ICommand]
        async Task GoMainPage()
        {
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }


    }
}
*/


/*using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public class SignUpViewModel : ObservableRecipient
    {
        private string _username;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private RelayCommand _signUpCommand;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public ICommand SignUpCommand => _signUpCommand ??= new RelayCommand(SignUp);

        private async void SignUp()
        {
            // Perform sign-up validation
            bool isValid = ValidateSignUp();

            if (isValid)
            {
                // Navigate to the main page
                await GoMainPage();
            }
            else
            {
                // Send a message to show an error message
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid sign-up information.", "OK");
                // WeakReferenceMessenger.Default.Send(new ErrorMessage("Invalid sign-up information."));
            }
        }

        private bool ValidateSignUp()
        {
            // Perform sign-up validation logic here
            // For demonstration purposes, let's assume validation is successful if all fields are non-empty
            return !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(Password) &&
                   !string.IsNullOrEmpty(ConfirmPassword) &&
                   Password == ConfirmPassword;
        }

        private async Task GoMainPage()
        {
            await Shell.Current.GoToAsync($"{nameof(MainPage)}");
        }
    }
}
*/

/*using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class SignUpViewModel : ObservableRecipient
    {
        private string _username;
        private string _group;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private bool _receiveNewsletter;
        private RelayCommand _signUpCommand;
        private ICommand _goToLoginPageCommand;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Group
        {
            get => _group;
            set => SetProperty(ref _group, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public bool ReceiveNewsletter
        {
            get => _receiveNewsletter;
            set => SetProperty(ref _receiveNewsletter, value);
        }

        public ICommand SignUpCommand => _signUpCommand ??= new RelayCommand(SignUp);

        public ICommand GoToLoginPageCommand => _goToLoginPageCommand ??= new Command(GoToLoginPage);

        private async void SignUp()
        {
            // Perform sign-up validation
            bool isValid = ValidateSignUp();

            if (isValid)
            {
                // Navigate to the main page
                await GoMainPage();
            }
            else
            {
                // Display an error message
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid sign-up information.", "OK");
            }
        }

        private bool ValidateSignUp()
        {
            // Perform sign-up validation logic here
            // For demonstration purposes, let's assume validation is successful if all fields are non-empty
            return !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(Password) &&
                   !string.IsNullOrEmpty(ConfirmPassword) &&
                   Password == ConfirmPassword;
        }

        private async Task GoMainPage()
        {
            await Shell.Current.GoToAsync($"{nameof(HomePage)}");
        }

        private async void GoToLoginPage()
        {
            // Navigate to the login page
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
*/

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
//using Org.Apache.Http.Protocol;

namespace MauiApp1.NewFolder1
{
    public partial class SignUpViewModel : ObservableRecipient
    {
        private string _username;
        private string _group;
        private string _email;
        private string _password;
        private string _confirmPassword;
        private bool _receiveNewsletter;
        private RelayCommand _signUpCommand;
        private ICommand _goToLoginPageCommand;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Group
        {
            get => _group;
            set => SetProperty(ref _group, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public bool ReceiveNewsletter
        {
            get => _receiveNewsletter;
            set => SetProperty(ref _receiveNewsletter, value);
        }

        public ICommand SignUpCommand => _signUpCommand ??= new RelayCommand(SignUp);

        public ICommand GoToLoginPageCommand => _goToLoginPageCommand ??= new Command(GoToLoginPage);

        //public ICommand SignUpCommand => _signUpCommand ??= new RelayCommand(async () => await SignUp());

        //public ICommand GoToLoginPageCommand => _goToLoginPageCommand ??= new Command(async () => await GoToLoginPage());


        private async void SignUp()
        {
            // Perform sign-up validation
            bool isValid = ValidateSignUp();

            if (isValid)
            {
                // Prepare the payload
                var payload = new
                {
                    email = Email,
                    password = Password,
                    group = Group,
                    nickname = Username
                };
                Console.WriteLine($"email: {Email}");
                Console.WriteLine($"password: {Password}");
                Console.WriteLine($"group: {Group}");
                Console.WriteLine($"nickname: {Username}");


                var userId = "ac92088d - 083c - 4a1d - a5be - fe9325d2961b";

                // Display userId in the console
                Console.WriteLine($"The userID is {userId}");
                await Shell.Current.Navigation.PushAsync(new HomePage(userId));
                // Serialize the payload
                /*var jsonPayload = JsonConvert.SerializeObject(payload);

                // Send POST request to the API
                using var client = new HttpClient();
                var response = await client.PostAsync("https://users-indentity-api.azurewebsites.net/api/User/register",
                                                       new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read and parse the response
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);

                    // Extract the userId from the response
                    var userId = responseData.userId;

                    // Display userId in the console
                    Console.WriteLine($"The userID is {userId}");
                    await Shell.Current.Navigation.PushAsync(new HomePage(userId));
                }
                else
                {
                    Console.WriteLine($"Failed to register user. Status code: {response.StatusCode}");
                    // Display an error message if request fails
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to register user.", "OK");
                }*/
            }
            else
            {
                // Display an error message for invalid sign-up information
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid sign-up information.", "OK");
            }
        }

        private bool ValidateSignUp()
        {
            // Perform sign-up validation logic here
            // For demonstration purposes, let's assume validation is successful if all fields are non-empty
            return !string.IsNullOrEmpty(Username) &&
                   !string.IsNullOrEmpty(Email) &&
                   !string.IsNullOrEmpty(Password) &&
                   !string.IsNullOrEmpty(ConfirmPassword) &&
                   Password == ConfirmPassword;
        }

        private async Task GoMainPage()
        {
            await Shell.Current.GoToAsync($"{nameof(HomePage)}");
        }

        private async void GoToLoginPage()
        {
            // Navigate to the login page
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}

