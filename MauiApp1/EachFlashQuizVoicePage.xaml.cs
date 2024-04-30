using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashQuizVoicePage : ContentPage
{
	public EachFlashQuizVoicePage(string userId, Set selectedSet)
	{
		InitializeComponent();
        BindingContext = new EachFlashQuizVoiceViewModel(userId, selectedSet);
    }
}