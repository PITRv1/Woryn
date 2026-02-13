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
    private Godot.Collections.Array<string> materials = 
        ["res://Assets/Shaders/materials/playerMats/playerMat1.material", 
        "res://Assets/Shaders/materials/playerMats/playerMat2.material",
        "res://Assets/Shaders/materials/playerMats/playerMat3.material",
        "res://Assets/Shaders/materials/playerMats/playerMat4.material"];
    [Export] private Godot.Collections.Array<MeshInstance3D> bodyparts;

    private const string BlendPath = "parameters/BlendSpace1D/blend_position";

    float target = 0.0f;

    public override void _Ready()
    {
    SetColor();    
    }


    public void SetColor()
    {
        GD.Print("PLAYER INDEXXXXXXX: " + PlayerIndex);
        var material = GD.Load<StandardMaterial3D>(materials[PlayerIndex]);

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
