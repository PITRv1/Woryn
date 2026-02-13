using Godot;
using System;

public partial class ClairvoyantActive : IActiveItem
{
	public bool MultiUse { get; set; }
	public int Amount { get; set; }
	public ItemType ItemType { get; set; }
}
