using Godot;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public partial class ToolTipMenu : Control
{
	[Export] ColorRect colorRect;
	[Export] Label titleLabel;
	[Export] RichTextLabel descriptionLabel;

	Vector2 positionOffsetVector = new(-10, -10);


    public ToolTipMenu()
	{
		Global.toolTipMenu = this;
	}

    public override void _Ready()
    {
		Visible = false;
    }

	public void ShowMenu(ToolTipInfo toolTipInfo)
	{
		colorRect.Color = toolTipInfo.titleColor;
		titleLabel.Text = toolTipInfo.objectName;
		descriptionLabel.Text = toolTipInfo.objectDescription;

		Position = GetViewport().GetMousePosition() - positionOffsetVector;
		Visible = true;
	}

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion && Visible) Visible = false;
    }

}
