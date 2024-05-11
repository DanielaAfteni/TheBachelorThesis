using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class SchedulePage : ContentPage
{
	public SchedulePage(string token, string email, string group, string nickname)
	{
		InitializeComponent();
		BindingContext = new ScheduleViewModel(token, email, group, nickname);
	}
}