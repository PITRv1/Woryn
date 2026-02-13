using Godot;
using System;

public partial class PlayerVisualController : Node3D
{
    [Export] public int PlayerIndex = 1;
    [Export] AnimationTree animationTree;
    [Export] public bool PlayerControlled = true;
    [Export] public Camera3D Camera;

    [ExportGroup("Bodyparts")]
    [Export] private Godot.Collections.Array<Material> materials;
    [Export] private Godot.Collections.Array<MeshInstance3D> bodyparts;

    private const string BlendPath = "parameters/BlendSpace1d/blend_position";

    float target = 0.0f;

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

    public void moveCamera(int to)
    {
        target = Mathf.Clamp(to, 0.0f, 1.0f);
        GD.Print("SHOULD BE L:EEEEEEEEEEEEEEEEEERPING YEAH");
    }

    public override void _Process(double delta)
    {
        animationTree.Set(BlendPath, Mathf.Lerp(
            (float)animationTree.Get(BlendPath),
            target,
            delta * 5f
        ));

        if (!PlayerControlled)
        {
            
        }
    }

}
