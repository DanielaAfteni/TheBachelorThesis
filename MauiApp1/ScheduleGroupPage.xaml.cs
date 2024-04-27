using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class ScheduleGroupPage : ContentPage
{
	public ScheduleGroupPage(string group)
	{
		InitializeComponent();
        BindingContext = new ScheduleGroupViewModel(group);
    }
}