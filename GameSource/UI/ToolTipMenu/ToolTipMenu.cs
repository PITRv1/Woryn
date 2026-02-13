using Godot;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public partial class ToolTipMenu : Control
{
	[Export] Timer timer;
	[Export] ColorRect colorRect;
	[Export] Label titleLabel;
	[Export] RichTextLabel descriptionLabel;

	private Area3D _currentArea3d;

	public Area3D currentArea3d
	{
		get
		{
			return _currentArea3d;
		}
		set
		{
			if (currentArea3d != null) _currentArea3d.MouseExited -= HideMenu;
			_currentArea3d = value;

			_currentArea3d.MouseExited += HideMenu;
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

	private void HideMenu()
	{
		Visible = false;
	}

	public void StartTimer()
	{
		timer.Start(6);
	}

	public void ShowMenuOnScreen()
	{
		Position = GetViewport().GetMousePosition();
		Visible = true;
	}
}
