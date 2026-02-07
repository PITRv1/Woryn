using Godot;
using System.Collections.Generic;

public partial class PlayPile : Node
{
    public int TotalValue { get; private set; } = 0;

    public PointCard CurrentCard { get; private set; }

    private readonly List<ModifierCard> modifierCards = new();

    public override void _Ready()
    {
        CurrentCard = new PointCard(1);
    }

    public void NextTurn()
    {
        for (int i = modifierCards.Count - 1; i >= 0; i--)
        {
            if (modifierCards[i] is not ModifierCard card)
                continue;

            card.TurnsUntilActivation--;

            if (card.TurnsUntilActivation <= 0)
            {
                card.ActivateEffect();
                modifierCards.RemoveAt(i);
            }
        }
    }

    public int GetValueAndReset()
    {
        int value = TotalValue;
        TotalValue = 0;
        return value;
    }

    public bool AddCard(PointCard card)
    {
        if (CurrentCard.PointValue > card.PointValue)
            return false;

        CurrentCard = card;
        TotalValue += card.PointValue;

        
        for (int i = card.ModifierList.Count - 1; i >= 0; i--)
        {
            if (card.ModifierList[i] is not ModifierCard modifier)
                continue;

            modifierCards.Add(modifier);
            modifier.ApplyDeckModifier(this);
            card.ModifierList.RemoveAt(i);
        }

        return true;
    }
}
