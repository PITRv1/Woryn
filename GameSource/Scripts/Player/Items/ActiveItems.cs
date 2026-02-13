using Godot;
using System;

public enum ItemType
{
	POLITICIAN_ACTIVE,
	POLITICIAN_WEAK_ACTIVE,
	POLITICIAN_PASSIVE,
	GAMBLER_ACTIVE,
	GAMBLER_WEAK_ACTIVE,
	GAMBLER_PASSIVE,
	ALCHEMIST_ACTIVE,
	ALCHEMIST_WEAK_ACTIVE,
	ALCHEMIST_PASSIVE,
	MAIDEN_ACTIVE,
	MAIDEN_WEAK_ACTIVE,
	MAIDEN_PASSIVE,
	CLAIRVOYANT_ACTIVE,
	CLAIRVOYANT_WEAK_ACTIVE,
	CLAIRVOYANT_PASSIVE,
}

public interface IActiveItems
{
	public bool MultiUse { get; set; }
	public int Amount { get; set; }
	public ItemType ItemType { get; set; }
	public void PlayAbility()
	{
	}
	public void ReduceCooldown()
	{
	}
}
