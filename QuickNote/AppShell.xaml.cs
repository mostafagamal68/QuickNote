﻿using QuickNote.ViewModels;

namespace QuickNote;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(MainPage),
                typeof(MainPage));

        Routing.RegisterRoute(nameof(NoteDetails),
                typeof(NoteDetails));
    }
}
