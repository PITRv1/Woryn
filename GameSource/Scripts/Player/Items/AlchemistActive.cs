using Godot;
using System;

public class AlchemistActive : IActiveItem
{
	public bool MultiUse { get; set; }
	public int Amount { get; set; }
	public ItemType ItemType { get; set; }
}

public class AlchemistWeakActive : IActiveItem
{
	public bool MultiUse { get; set; }
	public int Amount { get; set; }
	public ItemType ItemType { get; set; }
}


