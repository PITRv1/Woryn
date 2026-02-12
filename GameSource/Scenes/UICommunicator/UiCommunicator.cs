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

    public PointCard3d selectedPointCard3D {private set; get;}
    public List<ModifierCard3d> selectedModifierCard3Ds {private set; get;} = new();

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
        GD.Print("PlayCard called");
        if (selectedPointCard3D == null)
        {
            GD.Print("selectedPointCard3D is null");
            return;
        }
        
        multiplayerPlayer.PlayerClass.ChosenPointCard = selectedPointCard3D.PointCard;
        
        GD.Print("PlayerClass: " + multiplayerPlayer.PlayerClass.ChosenPointCard);
        if (multiplayerPlayer.PlayerClass.ChosenPointCard == null) GD.Print("ChosenPointCard is null here");
        if (selectedPointCard3D.PointCard == null) GD.Print("Selected is null here");
        GD.Print("PlayerClass ChosenCard: " + multiplayerPlayer.PlayerClass.ChosenPointCard.GetType());
        GD.Print("PlayerClass ChosenCard: " + multiplayerPlayer.GetType());

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

    public void StartShop()
    {
        GD.Print("Shop started!");
        Random random = new();

        for (int i = 0; i < 4; i++)
        {
            ModifierCard3d modifierCard3DInstance = modifierCard3D.Instantiate<ModifierCard3d>();
            modifierCard3DInstance.isShopCard = true;
            modifierCard3DInstance.ModifierCard = ModifierCardTypeConverter.TypeToClass((MODIFIER_TYPES)random.Next(1,7));
            shopCards.AddCard(modifierCard3DInstance);
        }
    }

    public void CloseShop()
    {
        foreach (Node3D modifCard in shopCards.GetChildren())
        {
            shopCards.RemoveCard(modifCard);
        }
    }

    public void AddShopModifierCardToPlayerCards(ModifierCard modifierCard)
    {
        ModifierCard3d modifierCard3DInstance = modifierCard3D.Instantiate<ModifierCard3d>();
        modifierCard3DInstance.ModifierCard = modifierCard;

        modifierCards.AddCard(modifierCard3DInstance);
    }
}
