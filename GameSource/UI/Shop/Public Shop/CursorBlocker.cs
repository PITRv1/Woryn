using Godot;
using System;

public partial class CursorBlocker : CenterContainer
{
	[Export] Label timerLabel;
	[Export] Timer timerObject;
	[Export] AnimationPlayer animationPlayer;

	public void StartCursorBlocker()
	{
		timerObject.Start();
		animationPlayer.Play("Shake");
	}

    public override void _PhysicsProcess(double delta)
    {
		if ((int)timerObject.TimeLeft == 0) timerLabel.Text = "Go!";
        else timerLabel.Text = $"{(int)timerObject.TimeLeft}";
    }

	public void HandleTimeout()
	{
		Visible = false;
		animationPlayer.Stop();
	}

}
