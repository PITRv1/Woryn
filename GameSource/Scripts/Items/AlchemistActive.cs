using Godot;
using System;

public partial class AlchemistActive : IItem
{
	public string itemName { get; set; } = "Transformation";
    public int price { get; set; } = 10;//(?)

    public void KMS()
    {
        //itt removolni kéne magától a playertől vayg az inventorybol (?)
    }
    public PlayerClass Parent { get; set; }
    public void UseItem() // i need ui here
    {
        if (Parent.ChosenPointCard!=null)
            Parent.ChosenPointCard.CardRarity = CardRaritiesEnum.LEGENDARY;
    }
}