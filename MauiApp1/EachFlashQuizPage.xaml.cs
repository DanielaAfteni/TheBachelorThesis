using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashQuizPage : ContentPage
{
	public EachFlashQuizPage(string token, Set selectedSet)
	{
		InitializeComponent();
        BindingContext = new EachFlashQuizViewModel(token, selectedSet);
    }
}