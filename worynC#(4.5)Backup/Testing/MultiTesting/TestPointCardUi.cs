using Godot;
using System;

public partial class TestPointCardUi : Control
{
	[Export] public Label text;
	public PointCard pointCard;
	public PlayerClass playerClass;
	bool isClicked = false;

	public void HandleSelection()
	{
		playerClass.chosenPointCard = pointCard;
		GD.Print(playerClass.chosenPointCard);
	}

	private void ChangeTextColor()
	{
		isClicked = !isClicked;

		if (isClicked) {
			text.Modulate = Colors.Purple;
			return;
		}
		
		text.Modulate = Colors.White;
	}
}
