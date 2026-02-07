using Godot;
using System;

public class MErchantActive : IItem
{
    public string itemName { get; set; } = "Merchant's Mark";
    public int price { get; set; } = 10 ;//?

    public void KMS()
    {

    }
    public void UseItem() // ugye itt választhat a public shopba először
    {
        
    }
}