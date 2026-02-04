using Godot;
using System;

public partial class PublicShop : Control
{
    [Export] Label shopTimerLabel;
	[Export] Timer shopTimerObject;
    [Export] CursorBlocker cursorBlocker;

    [Signal] public delegate void PublicShopTimerTimeoutEventHandler();

    public void StartPublicShop()
    {
        Visible = true;

        cursorBlocker.StartCursorBlocker();
    }

    public override void _PhysicsProcess(double delta)
    {
        shopTimerLabel.Text = $"{(int)shopTimerObject.TimeLeft}";
    }

    public void HandleShopTimerTimeout()
    {
        EmitSignal(SignalName.PublicShopTimerTimeout);
    }

    public void StartShopTimer()
    {
        shopTimerObject.Start();
    }
}
