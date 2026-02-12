using Godot;
using System;

public partial class PlayerVisualController : Node3D
{
    [Export] public int PlayerIndex = 1;
    [Export] AnimationTree animationTree;

    [ExportGroup("Bodyparts")]
    [Export] private Godot.Collections.Array<Material> materials;
    [Export] private Godot.Collections.Array<MeshInstance3D> bodyparts;

    public override void _Ready()
    {
        var material = materials[PlayerIndex];

        foreach (var item in bodyparts)
        {
            item.MaterialOverlay = material;
        }
    }
 
    public void playBellHit()
    {
        animationTree.Set("parameters/OneShot/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
    }
}
