using Godot;
using System;
using System.Collections.Generic;

public partial class UiCommunicator : Node
{
    [Export] CardPlacementHandler pointCards;
    [Export] CardPlacementHandler modifierCards;
    [Export] MultiplayerPlayerClass multiplayerPlayer;

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
        selectedPointCard3D = pointCard;
        selectedPointCard3D.isSelected = true;
    }

    public void DeselectPointCard()
    {
        selectedPointCard3D.isSelected = false;
        selectedPointCard3D = null;

        foreach (ModifierCard3d modifierCard in selectedModifierCard3Ds)
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
        if (selectedPointCard3D == null) return;

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
}
