using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EditTitleSetPage : ContentPage
{
	public EditTitleSetPage(string token, Set selectedSet)
	{
		InitializeComponent();
		BindingContext = new EditTitleSetViewModel(token, selectedSet);

    }
}