using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class HomePage : ContentPage
{
    public HomePage(string userId)
    {
        InitializeComponent();
        BindingContext = new HomeViewModel(userId);
    }
}