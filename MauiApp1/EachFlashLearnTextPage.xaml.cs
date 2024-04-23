using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashLearnTextPage : ContentPage
{
	public EachFlashLearnTextPage(Set selectedSet)
	{
		InitializeComponent();
        BindingContext = new EachFlashLearnTextViewModel(selectedSet);
    }
}