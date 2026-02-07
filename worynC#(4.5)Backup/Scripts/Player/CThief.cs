using Godot;
using System;
using System.Linq;

public class CThief : PlayerClassInterface
{
	public string ClassName { get; set; } = "Thief" ;
    public int ActiveCooldown { get; set; } = 32;
    public int PassiveCooldown { get; set; } = 0 ;
    public ModifierCardDeck modifierCardDeck {get; set;}
    public PointCardDeck pointCardDeck {get; set;}
    public PlayerClass Parent { get; set; }
    public void UseActive()
    {
        //steal points
        //choose player here (UI)
        PlayerClass choosenPlayer=null;
        
        Parent.Points+= choosenPlayer.Points;
        choosenPlayer.Points = 0;
    }
    public void UsePassive()
    {
        // Legyen megmutatva az első 3 káryta a pakliból a játékosnak és válasszon (UI)
        PointCard choosenPCard=null;
        ModifierCard choosenMCard=null;
        
        Parent.AddCardToModifierCards(choosenMCard);
        Parent.AddCardToPointCards(choosenPCard);
        
    }
}