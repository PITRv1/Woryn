using Godot;
using System;

public partial class Credits : Control
{
	[Export] MainUI mainUI;

	public void Back()
	{
		mainUI.ResetToMainMenu();
	}
}
