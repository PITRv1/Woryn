using Godot;
using System;
using System.Collections.Generic;

public partial class UiCommunicator : Node
{
    [Export] MultiplayerPlayerClass multiplayerPlayer;

    public PointCard3d selectedPointCard3D {private set; get;}
    public List<ModifierCard3d> selectedModifierCard3Ds {private set; get;} = new();

    
    public void SelectCardPointCard(PointCard3d pointCard)
    {
        
    }
}
