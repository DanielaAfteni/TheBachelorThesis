using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashcardSetPage : ContentPage
{
    public EachFlashcardSetPage(string token, Set selectedSet)
    {
        InitializeComponent();
        BindingContext = new EachFlashcardSetViewModel(token, selectedSet); 
    }
}