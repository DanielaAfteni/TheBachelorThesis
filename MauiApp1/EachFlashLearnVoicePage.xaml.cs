using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashLearnVoicePage : ContentPage
{
	public EachFlashLearnVoicePage(Set selectedSet)
	{
		InitializeComponent();
		BindingContext = new EachFlashLearnVoiceViewModel(selectedSet);
    }
}