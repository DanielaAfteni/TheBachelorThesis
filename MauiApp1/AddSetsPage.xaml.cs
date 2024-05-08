using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class AddSetsPage : ContentPage
{
    public AddSetsPage(string token, string setId)
	{
		InitializeComponent();
        BindingContext = new AddSetsViewModel(token, setId);
    }
}