using Godot;
using System;

public class CMerchant : PlayerClassInterface
{
	public string ClassName { get; set; } = "Merchant" ;
    public int ActiveCooldown { get; set; } = 3 ;
    public int PassiveCooldown { get; set; } = 0 ;
    public PlayerClass Parent { get; set; }
    public void UseActive()
    {
        
    }
    public void UsePassive()
    {
        Parent.pointsToGoldRatio = 2;
    }
}