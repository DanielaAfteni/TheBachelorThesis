using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashQuizVoicePage : ContentPage
{
	public EachFlashQuizVoicePage(string token, string email, string group, string nickname, Set selectedSet)
	{
		InitializeComponent();
        BindingContext = new EachFlashQuizVoiceViewModel(token, email, group, nickname, selectedSet);
    }
}