using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Media;
using MauiApp1.NewFolder1;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG

            builder.Services.AddSingleton<SignUpPage>();
            builder.Services.AddSingleton<SignUpViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<LoginViewModel>();

            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HomeViewModel>();

            builder.Services.AddSingleton<FlashcardsPage>();
            builder.Services.AddSingleton<FlashcardsViewModel>();

            builder.Services.AddTransient<EachFlashcardSetPage>();
            builder.Services.AddTransient<EachFlashcardSetViewModel>();



            builder.Services.AddTransient<EachFlashQuizPage>();
            builder.Services.AddTransient<EachFlashQuizViewModel>();

            builder.Services.AddTransient<EachFlashQuizVoicePage>();
            builder.Services.AddTransient<EachFlashQuizVoiceViewModel>();
            //#if __ANDROID__
            //           builder.Services.AddSingleton<ISpeechToText, SpeechToTextImplementation>();
            //#endif
            builder.Services.AddSingleton<ISpeechToText>(SpeechToText.Default);



            builder.Services.AddTransient<EachFlashQuizTextPage>();
            builder.Services.AddTransient<EachFlashQuizTextViewModel>();



            builder.Services.AddTransient<EachFlashLearnPage>();
            builder.Services.AddTransient<EachFlashLearnViewModel>();

            builder.Services.AddTransient<EachFlashLearnTextPage>();
            builder.Services.AddTransient<EachFlashLearnTextViewModel>();

            builder.Services.AddTransient<EachFlashLearnVoicePage>();
            builder.Services.AddTransient<EachFlashLearnVoiceViewModel>();


            builder.Services.AddTransient<ScorePage>();
            builder.Services.AddTransient<ScoreViewModel>();


            builder.Services.AddSingleton<SchedulePage>();
            builder.Services.AddSingleton<ScheduleViewModel>();

            builder.Services.AddSingleton<PersonalCabinetPage>();
            builder.Services.AddSingleton<PersonalCabinetViewModel>();

            builder.Services.AddSingleton<ChatGPTPage>();
            builder.Services.AddSingleton<ChatGPTViewModel>();


            builder.Services.AddTransient<DetailPage>();
            builder.Services.AddTransient<DetailViewModel>();

            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
