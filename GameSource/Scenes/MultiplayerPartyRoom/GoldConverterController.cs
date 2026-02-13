using Godot;
using System;

public partial class GoldConverterController : Node
{
    [Export] public GoldConverterUi goldConverterUi;
    [Export] public DiegeticUI gold3DUi;

    public override void _Ready()
    {
        goldConverterUi.GoldTimerTimeout += CloseGoldConverterUI; 

    }

    public void CloseGoldConverterUI()
    {
        gold3DUi.GlobalPosition = new Vector3(gold3DUi.GlobalPosition.X, 999.0f, gold3DUi.GlobalPosition.Z);
    }

    public void OpenGoldConverter()
    {
        gold3DUi.GlobalPosition = new Vector3(-0.254f, 6.9f, 4.3f);
    }

}
