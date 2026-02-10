using Godot;
using System;

public partial class RingBell : Node3D, InteractableObjectInterface
{
	[Export] MultiplayerPlayerClass multiplayerPlayerClass;
	
	public void UseObject()
	{
		multiplayerPlayerClass.SendFoldRequest();
	}
	
}
