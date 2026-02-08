using Godot;
using System;

public partial class Deck3d : Node3D, InteractableObjectInterface
{
	[Export] Area3D area3D;

	public void UseObject()
	{
		GD.Print("Haro, im deku.");
	}

}
