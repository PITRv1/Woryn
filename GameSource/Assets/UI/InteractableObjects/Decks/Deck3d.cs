using Godot;
using System;

public partial class Deck3d : Node3D, InteractableObjectInterface
{
	[Export] UiCommunicator UiCommunicator;
	[Export] Label3D currentValue;
	[Export] Label3D totalValue;
	[Export] ToolTipInfo toolTipInfo;

	public void UseObject()
	{
		UiCommunicator.PlayCards();
	}

	public void ShowMenu()
	{
		Global.toolTipMenu.ShowMenu(toolTipInfo);
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
