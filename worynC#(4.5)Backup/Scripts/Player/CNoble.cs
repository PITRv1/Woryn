using Godot;
using System;

public class CNoble : PlayerClassInterface
{
	public string ClassName { get; set; } = "Noble" ;
    public int ActiveCooldown { get; set; } = 3 ;
    public int PassiveCooldown { get; set; } = 0 ;
    public PlayerClass Parent { get; set; }
    public void UseActive()
    {
        
    }
    public void UsePassive()
    {
        

    }
}