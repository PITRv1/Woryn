using Godot;
using System;

public partial class PointCard3d : Node3D, InteractableObjectInterface
{
	[Export] Label3D label;
	[Export] MeshInstance3D outlineMesh;
	[Export] AnimationPlayer animationPlayer;
	[Export] Area3D area3D;

	Color[] outlineColors = [Colors.Green, Colors.DeepSkyBlue, Colors.Yellow];

	private PointCard _pointCard;
	public PointCard PointCard
	{
		get {
			return _pointCard;
		}
		set
		{
			_pointCard = value;

			GD.Print($"Card rarity: {_pointCard.CardRarity}");

			if (label == null) return;
			UpdateText();

			if (outlineMesh == null) return;
			UpdateMeshColor();
		}	
	}

    public override void _Ready()
	{
		UpdateText();
		UpdateMeshColor();

		area3D.MouseEntered += () => animationPlayer.Play("Float");
		area3D.MouseExited += () => animationPlayer.Play("RESET");
	}

	private void UpdateMeshColor()
	{
		if (PointCard == null) return;

		StandardMaterial3D material = (StandardMaterial3D)outlineMesh.GetSurfaceOverrideMaterial(0);
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
		GD.Print("Haro, im pointi kard.");
	}
}
