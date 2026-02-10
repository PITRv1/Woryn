using Godot;
using System;

public partial class PlayerVisualController : Node3D
{
    [Export] AnimationTree animationTree;

    public void playBellHit()
    {
        animationTree.Set("parameters/OneShot/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
    }
}
