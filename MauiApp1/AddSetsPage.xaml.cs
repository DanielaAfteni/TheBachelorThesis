using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class AddSetsPage : ContentPage
{
    public AddSetsPage(string userId, string setId)
	{
		InitializeComponent();
        BindingContext = new AddSetsViewModel(userId, setId);
    }
}