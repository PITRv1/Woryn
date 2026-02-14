using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ModifierCardDeck
{
    public List<ModifierCard> modifierCards;

    [Export]
    public int Amount { get; set; } = 28;

    public ModifierCardDeck()
    {
        modifierCards = new List<ModifierCard>();
    }

    
    public void GenerateDeck()
    {
        // modifierCards.Clear();

        // for (int i = 0; i < Amount; i++)
        // {
        //     ModifierCardMultiplier modifierCard = new ModifierCardMultiplier();
        //     modifierCard.RandomizeProperties();
        //     if(modifierCards.Count < 4)
        //     {
        //         modifierCards.Add(modifierCard);
        //     }
        // }
        modifierCards.Clear();
        //
        // Random rng = new Random();
        
        // List<MODIFIER_TYPES> allTypes = Enum.GetValues(typeof(MODIFIER_TYPES))
        //     .Cast<MODIFIER_TYPES>()
        //     .Where(t => t != MODIFIER_TYPES.NONE)
        //     .ToList();
        
        // for (int i = 0; i < Amount; i++)
        // {
        //     MODIFIER_TYPES randomType = allTypes[rng.Next(allTypes.Count)];
        
        //     ModifierCard modifierCard = ModifierCardTypeConverter.TypeToClass(randomType);
        
        //     modifierCard.RandomizeProperties();
        
        //     modifierCards.Add(modifierCard);
        // }

        // modifierCards.Add(new ModifierCardMultiplier());
        // modifierCards.Add(new ModifierCardAddition());

        // modifierCards.Add(new ModifierCardSkip());
        // modifierCards.Add(new ModifierCardReversePlay());
        // modifierCards.Add(new ModifierCardNextPlayer());
        // modifierCards.Add(new ModifierCardChangeDeck());
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
