using Godot;
using System;

public partial class Deck3d : Node3D, InteractableObjectInterface
{
	[Export] Area3D area3D;
	[Export] UiCommunicator UiCommunicator;

	public void UseObject()
	{
		UiCommunicator.PlayCards();
	}

}
