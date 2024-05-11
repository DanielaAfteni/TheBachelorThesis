using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EditTitleSetPage : ContentPage
{
	public EditTitleSetPage(string token, string email, string group, string nickname, Set selectedSet)
	{
		InitializeComponent();
		BindingContext = new EditTitleSetViewModel(token, email, group, nickname, selectedSet);

    }
}