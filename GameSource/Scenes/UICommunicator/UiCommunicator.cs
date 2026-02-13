using Godot;
using System;
using System.Collections.Generic;

public partial class UiCommunicator : Node
{
    [Export] public CardPlacementHandler pointCards {private set; get;}
    [Export] public CardPlacementHandler modifierCards {private set; get;}
    [Export] CardPlacementHandler shopCards;
    [Export] MultiplayerPlayerClass multiplayerPlayer;
    [Export] PackedScene modifierCard3D;
    [Export] PlayerVisualController playerVisualController;
    [Export] GoldConverterController goldConverterController;
    



    public PointCard3d selectedPointCard3D {private set; get;}
    public List<ModifierCard3d> selectedModifierCard3Ds {private set; get;} = new();
    public bool PlayerSelectionMode = false;

    public override void _Ready()
    {
        Global.multiplayerClientGlobals.HandleRoundSuccess += HandleRoundSuccess;
    }

    private void HandleRoundSuccess(byte[] data)
    {
        var packet = RoundSuccessPacket.CreateFromData(data);

        if (packet.PlayerId != multiplayerPlayer.Id)
        {
            return;
        }

        RemoveSelectedCard();
    }
 
    public void SelectPointCard(PointCard3d pointCard)
    {
        if (selectedPointCard3D != null) selectedPointCard3D.isSelected = false;
        GD.Print("CARD SELCTED");
        selectedPointCard3D = pointCard;
        selectedPointCard3D.isSelected = true;
    }

    public void DeselectPointCard()
    {
        GD.Print("CARD DESELCTED");
        selectedPointCard3D.isSelected = false;
        selectedPointCard3D = null;

        foreach (var modifierCard in selectedModifierCard3Ds)
        {
            RemoveModifierCard(modifierCard);
        }
    }

    public void AddModifierCard(ModifierCard3d modifierCard)
    {
        if (selectedPointCard3D == null) return;
        
        if (selectedModifierCard3Ds.Count >= (int)selectedPointCard3D.PointCard.CardRarity) return;

        modifierCard.isSelected = true;
        selectedModifierCard3Ds.Add(modifierCard);
    }

    public void RemoveModifierCard(ModifierCard3d modifierCard)
    {
        modifierCard.isSelected = false;
        selectedModifierCard3Ds.Remove(modifierCard);
    }

    public void PlayCards()
    {
        if (PlayerSelectionMode)
        {
            return;
        }
        GD.Print("PlayCard called");
        if (selectedPointCard3D == null)
        {
            GD.Print("selectedPointCard3D is null");
            return;
        }
        
        multiplayerPlayer.PlayerClass.ChosenPointCard = selectedPointCard3D.PointCard;

        foreach (var modifierCard in selectedModifierCard3Ds)
        {
            multiplayerPlayer.PlayerClass.AddToChosenModifierCards(modifierCard.ModifierCard);
        }

        multiplayerPlayer.PlayCard();
    }

    private void RemoveSelectedCard()
    {
        pointCards.RemoveCard(selectedPointCard3D);
        selectedPointCard3D = null;
    
        foreach (var modifierCard in selectedModifierCard3Ds)
        {
            modifierCards.RemoveCard(modifierCard);
        }
        selectedModifierCard3Ds.Clear();
    }

    public async void StartShop()
    {
        GD.Print("Shop started!");
        Random random = new();
        goldConverterController.OpenGoldConverter();
        goldConverterController.goldConverterUi.timerObject.Start();

        for (int i = 0; i < 4; i++)
        {
            ModifierCard3d modifierCard3DInstance = modifierCard3D.Instantiate<ModifierCard3d>();
            modifierCard3DInstance.isShopCard = true;
            modifierCard3DInstance.ModifierCard = ModifierCardTypeConverter.TypeToClass((MODIFIER_TYPES)random.Next(1,7));
            shopCards.AddCard(modifierCard3DInstance);
            await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
        }

        playerVisualController.moveCamera(1);
    }

    public async void CloseShop()
    {
        foreach (Node3D modifCard in shopCards.GetChildren())
        {
            shopCards.RemoveCard(modifCard);
            await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);

        }

        playerVisualController.moveCamera(0);

    }

    public void AddShopModifierCardToPlayerCards(ModifierCard modifierCard)
    {
        ModifierCard3d modifierCard3DInstance = modifierCard3D.Instantiate<ModifierCard3d>();
        modifierCard3DInstance.ModifierCard = modifierCard;

        modifierCards.AddCard(modifierCard3DInstance);
    }
}
