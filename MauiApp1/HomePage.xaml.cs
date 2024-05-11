using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class HomePage : ContentPage
{
    public HomePage(string token, string email, string group, string nickname)
    {
        InitializeComponent();
        BindingContext = new HomeViewModel(token, email, group, nickname);
    }
}