 using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class ModifierCard3dShopVersion : Node3D, ICard3D
{
    [Export] Sprite3D sprite3D;
    [Export] Godot.Collections.Array<CompressedTexture2D> _icons;
    [Export] MeshInstance3D outlineMesh;
    [Export] AnimationPlayer animationPlayer;
    [Export] Area3D area3D;
    [Export] ToolTipInfo toolTipInfo;
    [Export] public Label3D PriceLabel;
    [Export] public GpuParticles3D expEffect;


    public bool isShopCard;

	public bool isSelected = false;
    public UiCommunicator UiCommunicatorInstance { get; set; }

    private ItemType _itemType;
    public ItemType ItemType
    {
        get => _itemType;
        set
        {
            _itemType = value;
			if (sprite3D == null) return;

            SetIcon();
        }
    }
    private static readonly List<ItemType> _items = Enum.GetValues(typeof(ItemType))
		.Cast<ItemType>()
		.Where(e => e.ToString().EndsWith("_PASSIVE"))
		.Where(e => e.ToString() != "DRUNKARD_PASSIVE")
		.ToList();

    StandardMaterial3D defaultMaterial;

    public override void _Ready()
    {
        // if(_modifierCard.IsCardModifier) modifCardValueLabel.Text = _modifierCard.Amount.ToString();

        defaultMaterial = (StandardMaterial3D)outlineMesh.GetSurfaceOverrideMaterial(0).DuplicateDeep();

		UiCommunicatorInstance = (UiCommunicator)GetTree().GetFirstNodeInGroup("UICommunicator");
        SetIcon();

        area3D.MouseEntered += () => animationPlayer.Play("Move");
        
        area3D.MouseExited += () => animationPlayer.Play("RESET");
    }

	public void SetIcon()
	{
        // if (ItemType == null) return;
		sprite3D.Texture = _icons[_items.IndexOf(_itemType)];
	}

    public void EmitEffect()
    {
        expEffect.Emitting = true;
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
		var packet = new ShopItemBuy
		{
			SenderId = Global.multiplayerPlayerClass.Id,
			CardIndex = GetParent().GetChildren().IndexOf(this),
			IsPublicShop = 1,
			Item = ItemType,
		};
		
		Global.networkHandler.ServerPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagReliable);

		return;
	}


    // public void UseObject()
    // {
    //     if (isShopCard)
    //     {
    //         UiCommunicatorInstance.AddShopModifierCardToPlayerCards(ModifierCard);
    //         UiCommunicatorInstance.modifierCards.RemoveCard(this);
    //         UiCommunicatorInstance.CloseShop();
    //         return;
    //     }

    //     if (isSelected) {
    //         UiCommunicatorInstance.RemoveModifierCard(this);
    //         UpdateMeshColor();
    //     }
    //     else if (UiCommunicatorInstance.selectedPointCard3D != null) {
    //         UiCommunicatorInstance.AddModifierCard(this);
    //         UpdateMeshColor(Colors.Purple);
    //     }
    // }

    // public void UseObject()
    // {
	   //  
    // }

    public void ShowMenu()
	{
		Global.toolTipMenu.ShowMenu(toolTipInfo);
	}
}
