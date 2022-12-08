using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using QuickNote.Configurations;
using QuickNote.ViewModels;

namespace QuickNote;

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

		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainPageVM>();

        builder.Services.AddTransient<NoteDetails>();
        builder.Services.AddTransient<NoteDetailsVM>();

        builder.Services.AddSingleton<Database>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
