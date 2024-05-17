using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class ScheduleGroupPage : ContentPage
{
	public ScheduleGroupPage(string token,string group, string day)
	{
		InitializeComponent();
        BindingContext = new ScheduleGroupViewModel(token,group, day);
    }
}