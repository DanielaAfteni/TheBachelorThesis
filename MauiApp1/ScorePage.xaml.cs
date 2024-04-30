using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class ScorePage : ContentPage
{
    public ScorePage(string userId, int correctCount, int totalQuestions)
    {
        InitializeComponent();
        BindingContext = new ScoreViewModel(userId, correctCount, totalQuestions);
    }
}