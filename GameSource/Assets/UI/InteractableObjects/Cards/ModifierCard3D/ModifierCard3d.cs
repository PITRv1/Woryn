 using System;
using Godot;

public partial class ModifierCard3d : Node3D, InteractableObjectInterface, ICard3D
{
    [Export] Sprite3D sprite3D;
    [Export] Godot.Collections.Array<CompressedTexture2D> modifierIcons;
    [Export] AnimationPlayer animationPlayer;
    [Export] Area3D area3D;

    public bool isShopCard;

	public bool isSelected = false;
    public UiCommunicator UiCommunicatorInstance { get; set; }

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
		UiCommunicatorInstance = (UiCommunicator)GetTree().GetFirstNodeInGroup("UICommunicator");
        SetIcon();

        area3D.MouseEntered += () => animationPlayer.Play("Move");
        area3D.MouseExited += () => animationPlayer.Play("RESET");
    }

	public void SetIcon()
	{
        if (ModifierCard == null) return;
		sprite3D.Texture = modifierIcons[(int)_modifierCard.ModifierType-1];
	}


    public void UseObject()
    {
        if (isShopCard)
        {
            UiCommunicatorInstance.AddShopModifierCardToPlayerCards(ModifierCard);
            UiCommunicatorInstance.modifierCards.RemoveCard(this);
            UiCommunicatorInstance.CloseShop();
            return;
        }

        if (isSelected) UiCommunicatorInstance.RemoveModifierCard(this);
        else UiCommunicatorInstance.AddModifierCard(this);
    }
}
