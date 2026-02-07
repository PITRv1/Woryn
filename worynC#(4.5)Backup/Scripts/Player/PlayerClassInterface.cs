using Godot;
using System;

public interface PlayerClassInterface
{
	public string ClassName { get; set; }
    public int ActiveCooldown { get; set; }
    public int PassiveCooldown { get; set; }
    public void UseActive();
    public void UsePassive();
}