using Godot;
using System;

public partial class TestUIMultiPlayer : Control
{   
    [Export] PackedScene scene;

    public async void Host()
    {
        GetTree().ChangeSceneToPacked(scene);
    }
    
    public void Join()
    {
        Global.networkHandler.StartClient();
        GetTree().ChangeSceneToPacked(scene);
    }
}
