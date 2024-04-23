using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class SignUpPage : ContentPage
{
	public SignUpPage(SignUpViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}