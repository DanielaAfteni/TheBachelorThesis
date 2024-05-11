using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class ScorePage : ContentPage
{
    public ScorePage(string token, string email, string group, string nickname, int correctCount, int totalQuestions)
    {
        InitializeComponent();
        BindingContext = new ScoreViewModel(token, email, group, nickname, correctCount, totalQuestions);
    }
}