using Godot;
using System;

public partial class Deck3d : Node3D, InteractableObjectInterface
{
	[Export] Area3D area3D;
	[Export] UiCommunicator UiCommunicator;

	[Export] Label3D currentValue;
	[Export] Label3D totalValue;

	public void UseObject()
	{
		UiCommunicator.PlayCards();
	}

	public void UpdateTotalValueText(int data)
	{
		totalValue.Text = data.ToString();
	}

	public void UpdateCurrentValueText(int data)
	{
		currentValue.Text = data.ToString();
	}
}
