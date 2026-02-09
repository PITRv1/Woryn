using Godot;
using System;

public partial class GoldConverterUi : Control
{
    [Export] Label timerLabel;
	[Export] Timer timerObject;
	[Export] SpinBox spinBox;
	[Export] Label goldConversionLabel;

    [Signal] public delegate void GoldTimerTimeoutEventHandler();

	public void StartGoldUI()
	{
		Visible = true;
		timerObject.Start();
	}

    public override void _PhysicsProcess(double delta)
    {
		// USE PLAYERCLASS CONVERT VALUE
		// USE PLAYER WITH TURNMANAGER
        // spinBox.MaxValue = Global.multiplayerPlayerClass.playerClass.Points;
        timerLabel.Text = $"{(int)timerObject.TimeLeft}";
		goldConversionLabel.Text = $"{(int)Math.Round(spinBox.Value * 0.8f)}";
    }

    public void HandleGoldTimerTimeout()
    {
        EmitSignal(SignalName.GoldTimerTimeout);
    }

	public void ConvertPointsToGold()
	{
		Global.multiplayerPlayerClass.PlayerClass.Points -= (int)spinBox.Value;
		Global.multiplayerPlayerClass.PlayerClass.Gold += (int)Math.Round(spinBox.Value * 0.8f);
	}
}
