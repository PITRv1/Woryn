 using System;
using Godot;

public partial class ModifierCard3d : Node3D
{
    [Export] private Sprite3D sprite3D;
    [Export] private Godot.Collections.Array<CompressedTexture2D> modifierIcons;

    private ModifierCard _modifierCard;
    public ModifierCard ModifierCard
    {
        get => _modifierCard;
        set
        {
            _modifierCard = value;
			if (sprite3D == null) return;
            SetIcon();
        }
    }

    public override void _Ready()
    {
        SetIcon();
    }

	public void SetIcon()
	{
		sprite3D.Texture = modifierIcons[(int)_modifierCard.ModifierType-1];
	}

}
