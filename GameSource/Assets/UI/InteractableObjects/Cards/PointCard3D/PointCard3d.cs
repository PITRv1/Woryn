using Godot;
using System;

public partial class PointCard3d : Node3D, InteractableObjectInterface, ICard3D
{
	[Export] Label3D label;
	[Export] MeshInstance3D outlineMesh;
	[Export] AnimationPlayer animationPlayer;
	[Export] Area3D area3D;
	[Export] ToolTipInfo toolTipInfo;

	Color[] outlineColors = [Colors.Green, Colors.DeepSkyBlue, Colors.Yellow];

	public bool isSelected = false;
	public UiCommunicator UiCommunicatorInstance { get; set; }


	private PointCard _pointCard;
	public PointCard PointCard
	{
		get => _pointCard;
		set
		{
			_pointCard = value;

			if (label == null) return;
			UpdateText();

			if (outlineMesh == null) return;
			UpdateMeshColor();
		}	
	}

    public override void _Ready()
	{
		UiCommunicatorInstance = (UiCommunicator)GetTree().GetFirstNodeInGroup("UICommunicator");
		UpdateText();
		UpdateMeshColor();

		area3D.MouseEntered += () => {
			animationPlayer.Play("Float");
		};
	
		area3D.MouseExited += () => {
			animationPlayer.Stop();
		};
	}

	private void UpdateMeshColor()
	{
		if (PointCard == null) return;

		var material = (StandardMaterial3D)outlineMesh.GetSurfaceOverrideMaterial(0).DuplicateDeep();
		material.AlbedoColor = outlineColors[(int)PointCard.CardRarity-1];
		outlineMesh.SetSurfaceOverrideMaterial(0, material);
	}

	private void UpdateText()
	{
		if (PointCard == null) return;
        label.Text = _pointCard.PointValue.ToString();
	}

	public void UseObject()
	{
		if (isSelected) UiCommunicatorInstance.DeselectPointCard();
		else UiCommunicatorInstance.SelectPointCard(this);
	}

	public void ShowMenu()
	{
		Global.toolTipMenu.ShowMenu(toolTipInfo);
	}
}
