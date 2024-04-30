using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashLearnPage : ContentPage
{
	public EachFlashLearnPage(string userId, Set selectedSet)
	{
		InitializeComponent();
        BindingContext = new EachFlashLearnViewModel(userId, selectedSet);
    }
}