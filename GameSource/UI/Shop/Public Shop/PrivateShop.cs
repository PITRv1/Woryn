using Godot;
using System;

public partial class PrivateShop : Control
{
	[Export] Timer shopTimer;
	[Export] Label shopTimerLabel;

	[Signal] public delegate void PrivateShopTimerTimeoutEventHandler();

	public void StartPrivateShop()
	{
		Visible = true;
		shopTimer.Start();
	}

    public override void _PhysicsProcess(double delta)
    {
        shopTimerLabel.Text = $"{(int)shopTimer.TimeLeft}";
    }

	public void HandleTimerTimeout()
	{
		EmitSignal(SignalName.PrivateShopTimerTimeout);
	}

}
