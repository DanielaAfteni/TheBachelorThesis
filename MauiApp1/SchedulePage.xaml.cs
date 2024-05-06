using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class SchedulePage : ContentPage
{
	public SchedulePage(string token)
	{
		InitializeComponent();
		BindingContext = new ScheduleViewModel(token);
	}
}