using Godot;
using System;

public static class ItemTypeConverter
{

	// public static ItemType ClassToType(IActiveItem item)
	// {
	// 	return item switch
	// 	{
	// 		ModifierCardMultiplier => MODIFIER_TYPES.MULTIPLIER,
	// 		ModifierCardAddition => MODIFIER_TYPES.ADDITION,
	// 		ModifierCardChangeDeck => MODIFIER_TYPES.CHANGE_DECK,
	// 		ModifierCardNextPlayer => MODIFIER_TYPES.GIVE_DECK_AROUND,
	// 		ModifierCardReversePlay => MODIFIER_TYPES.REVERSE,
	// 		ModifierCardSkip => MODIFIER_TYPES.SKIP,
	// 		_ => MODIFIER_TYPES.NONE
	// 	};
	// }
	//
	public static IActiveItem TypeToClass(ItemType item)
	{
		return item switch
		{
			ItemType.ALCHEMIST_WEAK_ACTIVE => new AlchemistWeakActive(),
			ItemType.GAMBLER_WEAK_ACTIVE => new AlchemistWeakActive(),
			ItemType.MAIDEN_WEAK_ACTIVE => new AlchemistWeakActive(),
			ItemType.POLITICIAN_WEAK_ACTIVE => new AlchemistWeakActive(),
	
			_ => null
		};
	}

	// public static List<MODIFIER_TYPES> ClassListToTypeList(List<ModifierCard> cards)
	// {
	// 	List<MODIFIER_TYPES> types = new List<MODIFIER_TYPES>();
	//
	// 	foreach (ModifierCard card in cards)
	// 	{
	// 		types.Add(ClassToType(card));
	// 	}
	//
	// 	return types;
	// }
}

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

public interface IActiveItem
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
