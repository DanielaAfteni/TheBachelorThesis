using MauiApp1.NewFolder1;

namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

            Routing.RegisterRoute(nameof(SchedulePage), typeof(SchedulePage));
            Routing.RegisterRoute(nameof(ScheduleGroupPage), typeof(ScheduleGroupPage));
            Routing.RegisterRoute(nameof(ScheduleDayPage), typeof(ScheduleDayPage));


            Routing.RegisterRoute(nameof(FlashcardsPage), typeof(FlashcardsPage));
            Routing.RegisterRoute(nameof(AddSetsPage), typeof(AddSetsPage));
            Routing.RegisterRoute(nameof(EditTitleSetPage), typeof(EditTitleSetPage));

            Routing.RegisterRoute(nameof(PersonalCabinetPage), typeof(PersonalCabinetPage));

            Routing.RegisterRoute(nameof(EachFlashcardSetPage), typeof(EachFlashcardSetPage));

            Routing.RegisterRoute(nameof(EachFlashQuizPage), typeof(EachFlashQuizPage));
            Routing.RegisterRoute(nameof(EachFlashQuizVoicePage), typeof(EachFlashQuizVoicePage));
            Routing.RegisterRoute(nameof(EachFlashQuizTextPage), typeof(EachFlashQuizTextPage));

            Routing.RegisterRoute(nameof(EachFlashLearnPage), typeof(EachFlashLearnPage));
            Routing.RegisterRoute(nameof(EachFlashLearnTextPage), typeof(EachFlashLearnTextPage));
            Routing.RegisterRoute(nameof(EachFlashLearnVoicePage), typeof(EachFlashLearnVoicePage));


            Routing.RegisterRoute(nameof(ScorePage), typeof(ScorePage));

            Routing.RegisterRoute(nameof(ChatGPTPage), typeof(ChatGPTPage));
            Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
        }
    }
}
