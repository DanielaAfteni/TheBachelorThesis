using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class ScheduleDayPage : ContentPage
{
	public ScheduleDayPage(string day)
	{
		InitializeComponent();
        BindingContext = new ScheduleDayViewModel(day);
    }
}