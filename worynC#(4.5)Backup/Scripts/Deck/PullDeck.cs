using Godot;
using System;
using System.Linq;

public partial class PullDeck : Button
{
	private ModifierCardDeck modifierCardDeck;
	private PointCardDeck pointCardDeck;

	public override void _Ready()
	{
		modifierCardDeck = new ModifierCardDeck();
		pointCardDeck = new PointCardDeck();

		modifierCardDeck.GenerateDeck();
		pointCardDeck.GenerateDeck();
	}

    public override void _Pressed()
    {
		GD.Print("Pressed");
		
    }  


}
