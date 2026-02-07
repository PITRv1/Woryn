using Godot;
using System;

public class CAlchemist : PlayerClassInterface
{
	public string ClassName { get; set; } = "Alchemist" ;
    public int ActiveCooldown { get; set; } = 3 ;
    public int PassiveCooldown { get; set; } = 0 ;
    public PlayerClass Parent { get; set; }
    public void UseActive()
    {
        //ki kéne választani a handbol egy kartyat ami lega lessz (I need UI Ábel plsss);


        if (Parent.chosenPointCard!=null)
            Parent.chosenPointCard.CardRarity = CardRaritiesEnum.LEGENDARY;
    }
    public void UsePassive()
    {
        Parent.pointsToGoldRatio = 1;
    }
}