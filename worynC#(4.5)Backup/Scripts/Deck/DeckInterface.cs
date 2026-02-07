using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class DeckInterface
{
    [Signal]
    public delegate void OutOfCards();
	public List<CardInterface> Cards { get; }
    public void GenerateDeck()
    {
        
    }
    public CardInterface Drawcard()
    {
        if (Cards.Count!=0){
            CardInterface card = Cards[0];
            Cards.RemoveAt(0);
            return card;
        }
        else
        {
            //EmitSignal(OutOfCards); // i have no idea hov signals work :(
            return null;
        }
        
    }
}