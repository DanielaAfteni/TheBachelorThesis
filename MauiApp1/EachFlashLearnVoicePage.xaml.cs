using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashLearnVoicePage : ContentPage
{
	public EachFlashLearnVoicePage(string token, Set selectedSet)
	{
		InitializeComponent();
		BindingContext = new EachFlashLearnVoiceViewModel(token, selectedSet);
    }
}