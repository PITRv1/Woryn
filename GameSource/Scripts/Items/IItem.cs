using Godot;
using System;

public interface IItem
{
    public int price { get; set; }
    public string itemName { get; set; }
    public void KMS();
}