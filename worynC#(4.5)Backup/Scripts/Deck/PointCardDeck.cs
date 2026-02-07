using Godot;
using System.Collections.Generic;
using System.ComponentModel;

public partial class PointCardDeck
{
    public List<PointCard> pointCards;

    // private int MaxNumber = 9; For later use
    // private int NumberOfCards = 36;
    private int NumberOfCards = 10;
    
    public PointCardDeck()
    {
        pointCards = new List<PointCard>();
    }

    public int GetCount()
    {
        return pointCards.Count;
    }

    public void GenerateDeck()
    {
        pointCards.Clear();

        for (int i = 0; i < NumberOfCards; i++)
        {
            pointCards.Add(new PointCard(i / 4 + 1));
        }

        ShuffleCards();
    }

    // Medve made this, I hope it works
    private void ShuffleCards()
    {
        var rng = new RandomNumberGenerator(); 
        rng.Randomize();

        for (int i = pointCards.Count - 1; i > 0; i--)
        {
            int j = rng.RandiRange(0, i);
            (pointCards[i], pointCards[j]) = (pointCards[j], pointCards[i]);
        }
    }

    public PointCard[] PullCards(int count)
    {
        count = 4 - count;

        count = pointCards.Count < count ? pointCards.Count : count;

        PointCard[] cards = new PointCard[count];

        for (int i = 0; i < count; i++)
        {
            cards[i] = pointCards[0];
            pointCards.RemoveAt(0);
        }

        return cards;
    }

    public void PrintCards()
    {
        string points = "";
        int buh = 0;
        foreach (PointCard pointCard in pointCards)
        {
            points += pointCard.PointValue + " ";
            buh++;
        }
    }
}
