using Godot;
using System;
using System.ComponentModel;

public partial class GoldConverterUi : Control
{
    [Signal] public delegate void GoldTimerTimeoutEventHandler();
    [Export] Label timerLabel;
	[Export] public Timer timerObject;
	[Export] SpinBox spinBox;
	[Export] Label goldConversionLabel;

	[Export] PlayerHud playerHud;
	


	public void StartGoldUI()
	{
		Visible = true;
	}

    public override void _PhysicsProcess(double delta)
    {
        spinBox.MaxValue = Global.multiplayerPlayerClass.PlayerClass.Points;
        timerLabel.Text = $"{(int)timerObject.TimeLeft}";
		goldConversionLabel.Text = $"{(int)Math.Round(spinBox.Value * Global.multiplayerPlayerClass.PlayerClass.PlayerStats.PointsToGoldRatio)}";
    }

    public void HandleGoldTimerTimeout()
    {
        EmitSignal(SignalName.GoldTimerTimeout);
    }

	public void ConvertPointsToGold()
	{
		Global.multiplayerPlayerClass.PlayerClass.Points -= (int)spinBox.Value;
		Global.multiplayerPlayerClass.PlayerClass.Gold += (int)Math.Round(spinBox.Value * 0.8f);
		
		
		playerHud.UpdatePointsAmount(Global.multiplayerPlayerClass.PlayerClass.Points);
		playerHud.UpdateGoldAmount(Global.multiplayerPlayerClass.PlayerClass.Gold);

	}
}
