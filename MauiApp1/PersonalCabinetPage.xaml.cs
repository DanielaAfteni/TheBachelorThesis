using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class PersonalCabinetPage : ContentPage
{
	public PersonalCabinetPage(PersonalCabinetViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}