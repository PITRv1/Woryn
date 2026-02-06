using Godot;
using System;

public class AlchemistActive : IItem
{
	public string itemName { get; set; } = "Transformation";
    public void KMS()
    {
        //itt removolni kéne magától a playertől vayg az inventorybol (?)
    }
    public PlayerClass Parent { get; set; }
    public void UseItem()
    {
        if (Parent.chosenPointCard!=null)
            Parent.chosenPointCard.CardRarity = CardRaritiesEnum.LEGENDARY;
    }
}