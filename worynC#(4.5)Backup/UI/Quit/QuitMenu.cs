using Godot;
using System;

public partial class QuitMenu : Control
{
	[Export] MainUI mainUI;

	public void Back()
	{
		mainUI.ResetToMainMenu();
	}

	public void Quit()
	{
		GetTree().Quit();
	}
}
