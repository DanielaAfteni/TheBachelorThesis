using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashQuizPage : ContentPage
{
	public EachFlashQuizPage(Set selectedSet)
	{
		InitializeComponent();
        BindingContext = new EachFlashQuizViewModel(selectedSet);
    }
}