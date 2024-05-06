using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashQuizTextPage : ContentPage
{
	public EachFlashQuizTextPage(string token, Set selectedSet)
	{
		InitializeComponent();
        BindingContext = new EachFlashQuizTextViewModel(token, selectedSet);
    }
    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is Flashcard flashcard && BindingContext is EachFlashQuizTextViewModel viewModel)
        {
            var index = viewModel.Flashcards.IndexOf(flashcard);
            viewModel.UpdateUserAnswer(index, e.NewTextValue);
        }
    }
}