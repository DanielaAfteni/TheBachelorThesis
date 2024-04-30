using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashLearnVoicePage : ContentPage
{
	public EachFlashLearnVoicePage(string userId, Set selectedSet)
	{
		InitializeComponent();
		BindingContext = new EachFlashLearnVoiceViewModel(userId, selectedSet);
    }
}