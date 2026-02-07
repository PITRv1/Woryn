using Godot;
using System;
using System.Collections.Generic;

public class DummyPlayer
{
	public List<PointCard> PointCardList { get; }
    public List<ModifierCard> ModifCardList { get; }
    public PlayerClassInterface ChoosenClass { get; }
    public int Points { get; set; }

}
