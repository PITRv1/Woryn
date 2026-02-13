 using System;
using Godot;

public partial class ModifierCard3d : Node3D, InteractableObjectInterface, ICard3D
{
    [Export] Sprite3D sprite3D;
    [Export] Godot.Collections.Array<CompressedTexture2D> modifierIcons;
    [Export] MeshInstance3D outlineMesh;
    [Export] AnimationPlayer animationPlayer;
    [Export] Area3D area3D;
    [Export] ToolTipInfo toolTipInfo;

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

    StandardMaterial3D defaultMaterial;

    public override void _Ready()
    {
        defaultMaterial = (StandardMaterial3D)outlineMesh.GetSurfaceOverrideMaterial(0).DuplicateDeep();

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

	private void UpdateMeshColor(Color color)
	{
		var material = (StandardMaterial3D)outlineMesh.GetSurfaceOverrideMaterial(0).DuplicateDeep();
		material.AlbedoColor = color;
		outlineMesh.SetSurfaceOverrideMaterial(0, material);
	}

    private void UpdateMeshColor()
	{
		outlineMesh.SetSurfaceOverrideMaterial(0, defaultMaterial);
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

        if (isSelected) {
            UiCommunicatorInstance.RemoveModifierCard(this);
            UpdateMeshColor();
        }
        else if (UiCommunicatorInstance.selectedPointCard3D != null) {
            UiCommunicatorInstance.AddModifierCard(this);
            UpdateMeshColor(Colors.Purple);
        }
    }

    public void ShowMenu()
	{
		Global.toolTipMenu.ShowMenu(toolTipInfo);
	}
}
