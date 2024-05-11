using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class PersonalCabinetPage : ContentPage
{
	public PersonalCabinetPage(string token, string email, string group, string nickname)
	{
		InitializeComponent();
		BindingContext = new PersonalCabinetViewModel(token, email, group, nickname);
	}
}