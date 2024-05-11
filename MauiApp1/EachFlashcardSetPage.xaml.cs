using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashcardSetPage : ContentPage
{
    public EachFlashcardSetPage(string token, string email, string group, string nickname, Set selectedSet)
    {
        InitializeComponent();
        BindingContext = new EachFlashcardSetViewModel(token, email, group, nickname, selectedSet); 
    }
}