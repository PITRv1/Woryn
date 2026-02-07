using Godot;
using System;

public partial class Singleplayer : Control
{
	[Export] MainUI mainUI;

	public void Back()
	{
		mainUI.ResetToMainMenu();
	}
}
