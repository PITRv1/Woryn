using Godot;
using System;

public partial class PointCard3d : Node3D
{
	[Export] Label3D label;

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
			label.Text = _pointCard.PointValue.ToString();
		}	
	}

    public override void _Ready()
    {
        label.Text = _pointCard.PointValue.ToString();
    }
}
