using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class EachFlashcardSetPage : ContentPage
{
    public EachFlashcardSetPage(Set selectedSet)
    {
        InitializeComponent();
        BindingContext = new EachFlashcardSetViewModel(selectedSet); 
    }
}