using Godot;
using System;

public partial class PoliticanActive : IActiveItem
{
	public bool MultiUse { get; set; }
	public int Amount { get; set; }
	public ItemType ItemType { get; set; }
    private const int MaxCooldown = 3;
	public int Cooldown { get; set; }
}
