using Godot;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
public class PlayerClass
{
    public List<PointCard> PointCardList { get; set; }
    public List<ModifierCard> ModifierCardList { get; set; }
    public PlayerStats PlayerStats { get; set; }
    private readonly List<IActiveItems> _currentItems = new List<IActiveItems>();
    public ModifierCardDeck ModifierCardDeck { get; }
    public MultiplayerPlayerClass Parent;
    public int Points { get; set; } = 0;
    public int Gold { get; set; } = 0;
    

    public PointCard ChosenPointCard { get; set; }
    public readonly List<ModifierCard> ChosenModifierCards = new();

    public PlayerClass()
    {
        PointCardList = new List<PointCard>();
        ModifierCardList = new List<ModifierCard>();

        ModifierCardDeck = new ModifierCardDeck();
        ModifierCardDeck.GenerateDeck();
        
        PlayerStats = new PlayerStats();
    }

    public void HandleDeckSwap(byte[] data)
    {
        GD.Print("Jello HALLO");
        var packet = DeckSwap.CreateFromData(data);

        SetPointCardDeck(packet.PointCards);
        SetModifierCards(packet.ModifierCards);
        Parent.ResetContainers();
    }

    private void SetPointCardDeck(PointCard[] cards)
    {
        PointCardList.Clear();
        PointCardList.AddRange(cards);
    }

    private void SetModifierCards(ModifierCard[] cards)
    {
        ModifierCardList.Clear();
        ModifierCardList.AddRange(cards);
    }

    public void SetPointCardDeck(List<PointCard> cards)
    {
        PointCardList.Clear();
        PointCardList.AddRange(cards);
    }

    public void SetModifierCards(List<ModifierCard> cards)
    {
        ModifierCardList.Clear();
        ModifierCardList.AddRange(cards);
    }

    public bool AddToChosenModifierCards(ModifierCard card)
    {
        if (ChosenPointCard == null)
            return false;
        if (ChosenModifierCards.Count >= (int)ChosenPointCard.CardRarity)
            return false;
        
        ChosenModifierCards.Add(card);
        return true;
    }

    public void AddCardToPointCards(PointCard card)
    {
        if (PointCardList.Count == 4)
            return;
        PointCardList.Add(card);
    }

    public void AddCardToModifierCards(ModifierCard card)
    {
        if (ModifierCardList.Count == 4)
            return;
        ModifierCardList.Add(card);
    }

    public bool AddModifierCard(PointCard pointCard, ModifierCard modifCard)
    {
        var result = pointCard.AddModifier(modifCard);
        if (result)
            ModifierCardList.Remove(modifCard);
        return result;
    }

    public void RemoveModifierCard(PointCard pointCard, ModifierCard modifCard)
    {
        pointCard.RemoveModifier(modifCard);
    }

    public void PlayCard(PointCard card, PlayPile playPile)
    {
        PointCardList.Remove(card);
        playPile.AddCard(card);
    }

    public void ProcessTurnInfoPacket(byte[] data)
    {
        var packet = TurnInfoPacket.CreateFromData(data);
        
        Points = packet.CurrentPointValue;
        Parent.SetUi(packet.MaxValue, packet.CurrentPointValue, packet.ThrowDeckValue);
        
        var sortedPointCards = packet.DeletePointCards.OrderByDescending(x => x).ToList();
        var sortedModifierCards = packet.DeleteModifierCards.OrderByDescending(x => x).ToList();
        
        if (Parent.Id == packet.CurrentPlayerId)
        {
            GD.Print("Client player cards");
            foreach (var card in PointCardList)
                GD.Print(card.PointValue + " ");
            foreach (ModifierCard card in ModifierCardList)
                GD.Print(card.ModifierType + " ");
            
            Parent._playerHud.StartCountdownTimer();
        }

        if (Parent.Id != packet.LastPlayer)
        {
            return;
        }

        foreach (var cardIndex in sortedPointCards)
        {
            PointCardList.RemoveAt(cardIndex);
        }

        foreach (var cardIndex in sortedModifierCards)
        {
            ModifierCardList.RemoveAt(cardIndex);
        }

        
    }

    public void ProcessPickUpAnswer(byte[] data)
    {
        var packet = PickUpCardAnswer.CreateFromData(data);
        // if (packet.PointCards.Length == 0) return;

        PointCardList.AddRange(packet.PointCards);
        ModifierCardList.AddRange(packet.ModifierCards);

        foreach (var card in packet.PointCards)
        {
            Parent.AddPointToContainer(card);
        }

        foreach (var card in packet.ModifierCards)
        {
            Parent.AddModifierToContainer(card);
        }

    }

    public bool CanEndTurn()
    {
        return ChosenPointCard != null;
    }
}