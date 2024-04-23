using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashLearnPage : ContentPage
{
	public EachFlashLearnPage(Set selectedSet)
	{
		InitializeComponent();
        BindingContext = new EachFlashLearnViewModel(selectedSet);
    }
}