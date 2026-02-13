using Godot;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public partial class ToolTipMenu : Control
{
	[Export] Timer timer;
	[Export] ColorRect colorRect;
	[Export] Label titleLabel;
	[Export] RichTextLabel descriptionLabel;
	Vector2 lastMousePos;

	private Area3D _currentArea3d;

	public Area3D currentArea3d
	{
		get
		{
			return _currentArea3d;
		}
		set
		{
			if (currentArea3d != null) _currentArea3d.AreaExited -= HideMenu;
			_currentArea3d = value;

			// _currentArea3d;
		}
	}

    public ToolTipMenu()
	{
		Global.toolTipMenu = this;
	}

    public override void _Ready()
    {
		Visible = false;
    }

	public void ShowMenu(ToolTipInfo toolTipInfo, Area3D area3D)
	{
		colorRect.Color = toolTipInfo.titleColor;
		titleLabel.Text = toolTipInfo.objectName;
		descriptionLabel.Text = toolTipInfo.objectDescription;


		currentArea3d = area3D;
		StartTimer();
	}

	private void HideMenu(Area3D area)
	{
		Visible = false;
	}

	public void StartTimer()
	{
		timer.Start(6);
	}

	public void ShowMenuOnScreen()
	{
		Position = lastMousePos;
		Visible = true;
	}

    public override void _Process(double delta)
	{
		if (GetViewport().GetMousePosition() - lastMousePos > new Vector2())
		{
			GD.Print("We are good");
			return;
		}

		GD.Print(timer.TimeLeft);

		// if (!timer.IsStopped()) timer.Stop();
		if (Visible) Visible = false;
	}
}
