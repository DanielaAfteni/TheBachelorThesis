using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class ScorePage : ContentPage
{
    public ScorePage(string token, int correctCount, int totalQuestions)
    {
        InitializeComponent();
        BindingContext = new ScoreViewModel(token, correctCount, totalQuestions);
    }
}