using Godot;
using System;

public partial class PlayerVisualController : Node3D
{
    public int PlayerIndex = 1;
    [Export] AnimationTree animationTree;
    [Export] public bool PlayerControlled = true;
    [Export] public Camera3D Camera;
    [Export] public Marker3D TargetMarker;

    [ExportGroup("Bodyparts")]
    [Export] private Godot.Collections.Array<Material> materials;
    [Export] private Godot.Collections.Array<MeshInstance3D> bodyparts;

    private const string BlendPath = "parameters/BlendSpace1D/blend_position";

    float target = 0.0f;

    public void SetColor()
    {
        GD.Print("PLAYER INDEXXXXXXX: " + PlayerIndex);
        var material = materials[PlayerIndex];

         GD.Print(material);

        foreach (var item in bodyparts)
        {
            item.MaterialOverlay = material;
        }
    }
 
    public void playBellHit()
    {
        animationTree.Set("parameters/OneShot/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
    }

    public void moveCamera(int to)
    {
        target = to;
        GD.Print("SHOULD BE L:EEEEEEEEEEEEEEEEEERPING YEAH");
    }

    public override void _Process(double delta)
    {
        animationTree.Set(BlendPath,
        Mathf.Lerp(
            (float)animationTree.Get(BlendPath),
            target,
            delta * 5f));
        
        if (!PlayerControlled)
        {
            
        }
    }

}
