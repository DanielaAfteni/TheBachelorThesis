using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.NewFolder1
{
    public partial class ScheduleViewModel: ObservableRecipient
    {
        private RelayCommand _pickPdfCommand;

        private RelayCommand _goBackCommand;
        private RelayCommand _logOutCommand;

        public ICommand GoBackCommand => _goBackCommand ??= new RelayCommand(ExecuteGoBack);
        public ICommand LogOutCommand => _logOutCommand ??= new RelayCommand(ExecuteLogOut);

        public ICommand PickPdfCommand => _pickPdfCommand ??= new RelayCommand(ExecutePickPdf);

        private async void ExecutePickPdf()
        {
            try
            {
                // Pick a PDF file
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Pdf,
                    PickerTitle = "Pick a PDF file"
                });

                if (result != null)
                {
                    // Process the selected PDF file
                    var stream = await result.OpenReadAsync();
                    // Handle the PDF file stream
                    Console.WriteLine($"PDF was LOADED");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                Console.WriteLine($"Error picking PDF: {ex.Message}");
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
    }
}
