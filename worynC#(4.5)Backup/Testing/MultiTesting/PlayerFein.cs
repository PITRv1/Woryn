using Godot;
using System;

public partial class PlayerFein : Panel
{
	[Export] Label text;

	public void SetText(string newText, bool addPlayerPrefix = true)
	{
		if (addPlayerPrefix) newText = "Player" + newText;
		text.Text = newText;
	}
}
