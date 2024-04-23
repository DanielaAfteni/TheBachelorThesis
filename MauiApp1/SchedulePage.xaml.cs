using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class SchedulePage : ContentPage
{
	public SchedulePage(ScheduleViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}