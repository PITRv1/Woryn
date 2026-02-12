using Godot;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public partial class ToolTipMenu : Control
{
	[Export] Timer timer;
	Vector2 lastMousePos;

	public ToolTipMenu()
	{
		Global.toolTipMenu = this;
	}

	public void ShowMenu()
	{
		lastMousePos = GetViewport().GetMousePosition();
	}

	public void StartTimer()
	{
		timer.Start();
	}

    public override void _Process(double delta)
    {
        if (GetViewport().GetMousePosition() == lastMousePos) return;
		
		if (!timer.IsStopped()) timer.Stop();
		if (Visible) Visible = false;
    }
}
