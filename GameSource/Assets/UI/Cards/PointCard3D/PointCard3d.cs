using Godot;
using System;

public partial class PointCard3d : Node3D
{
	[Export] Label3D label;
	[Export] MeshInstance3D outlineMesh;

	Color[] outlineColors = [Colors.Green, Colors.Blue, Colors.Red];

	private PointCard _pointCard;
	public PointCard PointCard
	{
		get {
			return _pointCard;
		}
		set
		{
			PointCard = _pointCard;

			GD.Print($"Card rarity: {_pointCard.CardRarity}");

			if (label == null) return;
			UpdateText();

			if (outlineMesh == null) return;
			UpdateMeshColor();
		}	
	}

    public override void _Ready()
	{
		// UpdateText();
		UpdateMeshColor();
	}

	private void UpdateMeshColor()
	{
		StandardMaterial3D material = (StandardMaterial3D)outlineMesh.GetSurfaceOverrideMaterial(0);
		// material.AlbedoColor = outlineColors[(int)PointCard.CardRarity-1];
		material.AlbedoColor = outlineColors[0];
		outlineMesh.SetSurfaceOverrideMaterial(0, material);
	}

	private void UpdateText()
	{
        label.Text = _pointCard.PointValue.ToString();
	}
}
