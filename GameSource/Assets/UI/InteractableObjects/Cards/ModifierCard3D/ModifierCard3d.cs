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
    [Export] Label3D modifCardValueLabel;
    [Export] public Label3D modifCardPriceLabel;
    [Export] public GpuParticles3D expEffect;

    public bool isShopCard;
    public bool ClickAble = true;

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
        if(_modifierCard.IsCardModifier) modifCardValueLabel.Text = _modifierCard.Amount.ToString();
        if(isShopCard)
        {
            modifCardPriceLabel.Show();
        }
        else
        {
            modifCardPriceLabel.Hide();
        }
            

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
    
    public void EmitEffect()
    {
        expEffect.Emitting = true;
    }


	private void UpdateMeshColor(Color color)
	{
		var material = (StandardMaterial3D)outlineMesh.GetSurfaceOverrideMaterial(0).DuplicateDeep().DuplicateDeep();
		material.AlbedoColor = color;
		outlineMesh.SetSurfaceOverrideMaterial(0, material);
	}

    private void UpdateMeshColor()
	{
		outlineMesh.SetSurfaceOverrideMaterial(0, defaultMaterial);
	}


    public void UseObject()
    {
        if (!ClickAble)
        {
            return;
        }
        if (isShopCard)
        {
            var packet = new ShopItemBuy
            {
                SenderId = Global.multiplayerPlayerClass.Id,
                CardIndex = GetParent().GetChildren().IndexOf(this),
                IsPublicShop = 0
            };

		    Global.networkHandler.ServerPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagReliable);

            return;
        }

        if (isSelected) {
            UiCommunicatorInstance.RemoveModifierCard(this);
            UpdateMeshColor();
        }
        else if (UiCommunicatorInstance.selectedPointCard3D != null && UiCommunicatorInstance.selectedModifierCard3Ds.Count < (int)UiCommunicatorInstance.selectedPointCard3D.PointCard.CardRarity) {
            UiCommunicatorInstance.AddModifierCard(this);
            UpdateMeshColor(Colors.Yellow);
        }
    }

    public void ShowMenu()
	{
		Global.toolTipMenu.ShowMenu(toolTipInfo);
	}
}
