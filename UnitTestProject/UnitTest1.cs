using Xunit;
using MauiApp1; // Adjust the namespace as per your project structure
using MauiApp1.NewFolder1; // Adjust the namespace as per your project structure

namespace UnitTestProject
{
    public class UnitTest1
    {
        [Fact]
        public void TestLoginPageElements()
        {
            // Arrange
            var viewModel = new LoginViewModel(); // Instantiate your LoginViewModel
            var loginPage = new LoginPage(viewModel); // Pass the view model instance to the LoginPage constructor

            // Act
            var stackLayout = (VerticalStackLayout)loginPage.Content; // Cast the Content to VerticalStackLayout for validation

            // Assert
            Assert.NotNull(stackLayout); // Ensure the Content of the page is not null
            Assert.Equal(2, stackLayout.Children.Count); // Ensure there are 2 direct children in the VerticalStackLayout

        }

        [Fact]
        public async Task TestLoginPageElements2()
        {
            // Arrange
            var viewModel = new LoginViewModel(); // Instantiate your LoginViewModel
            var loginPage = new LoginPage(viewModel); // Pass the view model instance to the LoginPage constructor

            // Act
            var stackLayout = (VerticalStackLayout)loginPage.Content; // Cast the Content to VerticalStackLayout for validation

            // Assert
            Assert.NotNull(stackLayout); // Ensure the Content of the page is not null
            Assert.Equal(2, stackLayout.Children.Count); // Ensure there are 2 direct children in the VerticalStackLayout

            // Assuming the second child is the content stack layout
            var contentStackLayout = (StackLayout)stackLayout.Children[1];
            Assert.Equal(4, contentStackLayout.Children.Count); // Ensure there are 4 children in the content stack layout

            // Assert the properties of the email entry field
            var emailEntry = (Entry)contentStackLayout.Children[0];
            Assert.Equal("Email", emailEntry.Placeholder); // Ensure the placeholder text is correct
            Assert.Equal(viewModel.UsernameOrEmail, emailEntry.Text); // Ensure the Text property is bound to the view model

            // Assert the properties of the password entry field
            var passwordEntry = (Entry)contentStackLayout.Children[1];
            Assert.Equal("Password", passwordEntry.Placeholder); // Ensure the placeholder text is correct
            Assert.Equal(viewModel.Password, passwordEntry.Text); // Ensure the Text property is bound to the view model
            Assert.True(passwordEntry.IsPassword); // Ensure the IsPassword property is set to true

            // Assert the properties of the login button
            var loginButton = (Button)contentStackLayout.Children[2];
            Assert.Equal("Log In", loginButton.Text); // Ensure the button text is correct
            
            Assert.Equal(FontAttributes.Bold, loginButton.FontAttributes); // Ensure the font attributes are bold

            // Assert the properties of the forgot password label
            var forgotPasswordLabel = (Label)contentStackLayout.Children[3];
            Assert.Equal("Forgot Password?", forgotPasswordLabel.Text); // Ensure the text of the label is correct
            
            Assert.Equal(FontAttributes.Bold, forgotPasswordLabel.FontAttributes); // Ensure the font attributes are bold
        }
    }
}