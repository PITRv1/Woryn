using Godot;
using System;

public partial class PlayerHud : Control
{
	[Export] Label timerLabel;
	[Export] Timer timer;
	[Export] RichTextLabel goldLabel;
	[Export] RichTextLabel pointLabel;

	public void StartCountdownTimer()
	{
		timer.Start();
	}

	public void UpdateGoldAmount(int amount)
	{
		goldLabel.Text =$"[wave freq=1] Gold: {amount}";
	}

	public void UpdatePointsAmount(int amount)
	{
		pointLabel.Text =$"[rainbow freq=0.2][wave freq=1] Points: {amount}";
	}

    public override void _Process(double delta)
    {
        timerLabel.Text = Convert.ToInt32(timer.TimeLeft).ToString();
    }


}
