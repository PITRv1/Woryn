using Godot;
using System;

public partial class Deck3d : Node3D, InteractableObjectInterface
{
	[Export] Area3D area3D;
	[Export] UiCommunicator UiCommunicator;
	[Export] Label3D currentValue;
	[Export] Label3D totalValue;
	[Export] ToolTipInfo toolTipInfo;

    public override void _Ready()
	{
		area3D.MouseEntered += () =>
		{
			GD.Print("Yoooo");
			Global.toolTipMenu.ShowMenu(toolTipInfo, area3D);
		};
	}

	public void UseObject()
	{
		UiCommunicator.PlayCards();
	}

	public void UpdateTotalValueText(int data)
	{
		totalValue.Text = data.ToString();
	}

	public void UpdateCurrentValueText(int data)
	{
		currentValue.Text = data.ToString();
	}
}
