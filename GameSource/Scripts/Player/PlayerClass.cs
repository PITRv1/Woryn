using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
public class PlayerClass
{
    public List<PointCard> PointCardList { get; set; }
    public List<ModifierCard> ModifCardList { get; set; }
    public PlayerClassInterface ChoosenClass { get; }
    public ModifierCardDeck modifierCardDeck { get; }
    public MultiplayerPlayerClass parent;
    public int Points { get; set; } = 0;
    public int Gold { get; set; } = 0;
    public float pointsToGoldRatio = 0.5f;

    public PointCard chosenPointCard ;
    // {
    //     get
    //     {
    //         return chosenPointCard;
    //     }
    //     set
    //     {
    //         chosenModifierCards.Clear();
    //         chosenPointCard = value;
    //     }
    // }
    public readonly List<ModifierCard> chosenModifierCards = new();

    public string EffectStatus { get; }
    
    public PlayerClass()
    {
        PointCardList = new List<PointCard>();
        ModifCardList = new List<ModifierCard>();

        modifierCardDeck = new ModifierCardDeck();
        modifierCardDeck.GenerateDeck();
    }

    public void HandleDeckSwap(byte[] data)
    {
        DeckSwap packet = DeckSwap.CreateFromData(data);

        SetPointCardDeck(packet.PointCards);
        SetModifierCards(packet.ModifierCards);
    }

    public void HandleDeckSwap(List<PointCard> pointCards, List<ModifierCard> modifierCards)
    {
        SetPointCardDeck(pointCards);
        SetModifierCards(modifierCards);
    }

    public void SetPointCardDeck(PointCard[] cards)
    {
        PointCardList.Clear();

        foreach (PointCard pointCard in cards)
            PointCardList.Add(pointCard);
    }

    public void SetModifierCards(ModifierCard[] cards)
    {
        ModifCardList.Clear();

        foreach (ModifierCard modifierCard in cards)
            ModifCardList.Add(modifierCard);
    }

    public void SetPointCardDeck(List<PointCard> cards)
    {
        PointCardList.Clear();

        foreach (PointCard pointCard in cards)
            PointCardList.Add(pointCard);
    }

    public void SetModifierCards(List<ModifierCard> cards)
    {
        ModifCardList.Clear();

        foreach (ModifierCard modifierCard in cards)
            ModifCardList.Add(modifierCard);
    }

    public bool AddToChosenModifierCards(ModifierCard card)
    {
        if (chosenPointCard == null)
            return false;
        if (chosenModifierCards.Count >= (int)chosenPointCard.CardRarity)
            return false;
        
        chosenModifierCards.Add(card);
        return true;
    }

    public void DecreaseCooldown()
    {
        ChoosenClass.ActiveCooldown--;
        ChoosenClass.PassiveCooldown--;
    }

    public void AddCardToPointCards(PointCard card)
    {
        if (PointCardList.Count == 4)
            return;
        PointCardList.Append(card);
    }

    public void AddCardToModifierCards(ModifierCard card)
    {
        if (ModifCardList.Count == 4)
            return;
        ModifCardList.Append(card);
    }

    public bool AddModifierCard(PointCard pointCard, ModifierCard modifCard)
    {
        bool result = pointCard.AddModifier(modifCard);
        
        if (result)
            ModifCardList.Remove(modifCard);
        
        return result;
    }

    public void RemoveModifierCard(PointCard pointCard, ModifierCard modifCard)
    {
        pointCard.RemoveModifier(modifCard);
    }

    public void PlayCard(PointCard card, PlayPile playpile)
    {
        PointCardList.Remove(card);
        playpile.AddCard(card);
    }

    public void ProccessTurnInfoPacket(byte[] data)
    {
        TurnInfoPacket packet = TurnInfoPacket.CreateFromData(data);
        Points = packet.CurrentPointValue;
        parent.SetUI(packet.MaxValue, packet.CurrentPointValue, packet.ThrowDeckValue);
        parent.RemoveSelectedCards(packet.LastPlayer, packet.DeletePointCards, packet.DeleteModifierCards);
    }

    public void ProccessPickUpAnswer(byte[] data)
    {
        PickUpCardAnswer packet = PickUpCardAnswer.CreateFromData(data);
        // if (packet.PointCards.Length == 0) return;

        PointCardList.AddRange(packet.PointCards);
        ModifCardList.AddRange(packet.ModifierCards);

        foreach (PointCard card in packet.PointCards)
        {
            parent.AddPointToContainer(card);
        }

        foreach (ModifierCard card in packet.ModifierCards)
        {
            parent.AddModifierToContainer(card);
        }

    }

    public bool CanEndTurn()
    {
        return chosenPointCard == null;
    }
}