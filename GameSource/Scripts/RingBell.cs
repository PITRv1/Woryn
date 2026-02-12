using Godot;
using System;

public partial class RingBell : Node3D, InteractableObjectInterface
{
	[Export] MultiplayerPlayerClass multiplayerPlayerClass;
	[Export] private Area3D _area3D;
	[Export] private PlayerVisualController playerVisualController;
	[Export] private AnimationPlayer ringAnimPlayer;


	public void UseObject()
	{
		multiplayerPlayerClass.SendFoldRequest();
		playerVisualController.playBellHit();
		ringAnimPlayer.Play("ring");
	}
	
}
