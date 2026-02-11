using Godot;
using System;

public partial class RingBell : Node3D, InteractableObjectInterface
{
	[Export] MultiplayerPlayerClass multiplayerPlayerClass;
	[Export] private Area3D _area3D;

	public override void _Ready()
	{
		// _area3D.
	}
	
	public void UseObject()
	{
		multiplayerPlayerClass.SendFoldRequest();
	}
	
}
