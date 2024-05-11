using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashQuizPage : ContentPage
{
	public EachFlashQuizPage(string token, string email, string group, string nickname, Set selectedSet)
	{
		InitializeComponent();
        BindingContext = new EachFlashQuizViewModel(token, email, group, nickname, selectedSet);
    }
}