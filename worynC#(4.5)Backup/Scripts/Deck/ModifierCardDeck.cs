using Godot;
using System.Collections.Generic;

public partial class ModifierCardDeck
{
    List<ModifierCard> modifierCards;

    [Export]
    public int Amount { get; set; } = 28;

    public ModifierCardDeck()
    {
        modifierCards = new List<ModifierCard>();
    }

    public void GenerateDeck()
    {
        modifierCards.Clear();

        // for (int i = 0; i < Amount; i++)
        // {
        //     ModifierCardMultiplier modifierCard = new ModifierCardMultiplier();
        //     modifierCard.RandomizeProperties();

        //     modifierCards.Add(modifierCard);
        // }
        // modifierCards.Add(new ModifierCardMultiplier());
        // modifierCards.Add(new ModifierCardAddition());
        modifierCards.Add(new ModifierCardSkip());
        modifierCards.Add(new ModifierCardReversePlay());
        modifierCards.Add(new ModifierCardNextPlayer());
        modifierCards.Add(new ModifierCardChangeDeck());
    }

    public ModifierCard[] PullCards(int count)
    {
        if (modifierCards.Count == 0)
            return [];

        count = 4 - count;

        count = modifierCards.Count < count ? modifierCards.Count : count;

        ModifierCard[] cards = new ModifierCard[count];
        for (int i = 0; i < count; i++)
        {
            cards[i] = modifierCards[0];
            modifierCards.RemoveAt(0);
        }

        return cards;
    }
}
