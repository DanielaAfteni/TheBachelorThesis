using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MauiApp1; // Adjust the namespace as per your project structure
using MauiApp1.NewFolder1; // Adjust the namespace as per your project structure


namespace UnitTestProject
{
    public class SignUpPageTests
    {
        [Fact]
        public void TestSignUpPageElements()
        {
            // Arrange
            var viewModel = new SignUpViewModel(); // Instantiate your SignUpViewModel
            var signUpPage = new SignUpPage(viewModel); // Pass the view model instance to the SignUpPage constructor

            // Act
            var stackLayout = (VerticalStackLayout)signUpPage.Content; // Cast the Content to VerticalStackLayout for validation

            // Assert
            Assert.NotNull(stackLayout); // Ensure the Content of the page is not null
            Assert.Equal(2, stackLayout.Children.Count); // Ensure there are 2 direct children in the VerticalStackLayout

            // Assuming the first child is the grid
            var grid = (Grid)stackLayout.Children[0];
            Assert.NotNull(grid); // Ensure the grid is not null

            // Assuming the second child is the content stack layout
            var contentStackLayout = (StackLayout)stackLayout.Children[1];
            Assert.NotNull(contentStackLayout);
        }

        [Fact]
        public async Task TestSignUpPageElements2()
        {
            // Arrange
            var viewModel = new SignUpViewModel(); // Instantiate your SignUpViewModel
            var signUpPage = new SignUpPage(viewModel); // Pass the view model instance to the SignUpPage constructor

            // Act
            var stackLayout = (VerticalStackLayout)signUpPage.Content; // Cast the Content to VerticalStackLayout for validation

            // Assert
            Assert.NotNull(stackLayout); // Ensure the Content of the page is not null
            Assert.Equal(2, stackLayout.Children.Count); // Ensure there are 2 direct children in the VerticalStackLayout

            // Assuming the second child is the content stack layout
            var contentStackLayout = (StackLayout)stackLayout.Children[1];
            Assert.Equal(7, contentStackLayout.Children.Count); // Ensure there are 7 children in the content stack layout

            // Assert the properties of the username entry field
            var usernameEntry = (Entry)contentStackLayout.Children[0];
            Assert.Equal("Username", usernameEntry.Placeholder); // Ensure the placeholder text is correct
            Assert.Equal(viewModel.Username, usernameEntry.Text); // Ensure Text property is bound to the view model

            // Assert the properties of the group entry field
            var groupEntry = (Entry)contentStackLayout.Children[1];
            Assert.Equal("Group", groupEntry.Placeholder); // Ensure the placeholder text is correct
            Assert.Equal(viewModel.Group, groupEntry.Text); // Ensure Text property is bound to the view model

            // Assert the properties of the email entry field
            var emailEntry = (Entry)contentStackLayout.Children[2];
            Assert.Equal("Email", emailEntry.Placeholder); // Ensure the placeholder text is correct
            Assert.Equal(viewModel.Email, emailEntry.Text); // Ensure Text property is bound to the view model

            // Assert the properties of the password entry field
            var passwordEntry = (Entry)contentStackLayout.Children[3];
            Assert.Equal("Password", passwordEntry.Placeholder); // Ensure the placeholder text is correct
            Assert.Equal(viewModel.Password, passwordEntry.Text); // Ensure Text property is bound to the view model
            Assert.True(passwordEntry.IsPassword); // Ensure IsPassword property is set to true

            // Assert the properties of the confirm password entry field
            var confirmPasswordEntry = (Entry)contentStackLayout.Children[4];
            Assert.Equal("Confirm Password", confirmPasswordEntry.Placeholder); // Ensure the placeholder text is correct
            Assert.Equal(viewModel.ConfirmPassword, confirmPasswordEntry.Text); // Ensure Text property is bound to the view model
            Assert.True(confirmPasswordEntry.IsPassword); // Ensure IsPassword property is set to true

            // Assert the properties of the checkbox
            var checkboxStackLayout = (StackLayout)contentStackLayout.Children[5];
            Assert.NotNull(checkboxStackLayout); // Ensure the checkbox stack layout is not null

            // Assert the properties of the sign up button
            var signUpButton = (Button)contentStackLayout.Children[6];
            Assert.Equal("Sign Up", signUpButton.Text); // Ensure the button text is correct
            
            Assert.Equal(FontAttributes.Bold, signUpButton.FontAttributes); // Ensure the font attributes are bold
        }
    }

}
