using MauiApp1.NewFolder1;

namespace MauiApp1;

public partial class ChatGPTPage : ContentPage
{
	public ChatGPTPage(ChatGPTViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}