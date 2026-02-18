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
	POLITICIAN_PASSIVE,
	GAMBLER_ACTIVE,
	GAMBLER_PASSIVE,
	ALCHEMIST_ACTIVE,
	ALCHEMIST_PASSIVE,
	MAIDEN_ACTIVE,
	MAIDEN_PASSIVE,
	DRUNKARD_ACTIVE,
	DRUNKARD_PASSIVE,
	// CLAIRVOYANT_ACTIVE,
	// CLAIRVOYANT_PASSIVE,
}

public interface IActiveItem
{
	public bool MultiUse { get; set; }
	public int Amount { get; set; }
	public ItemType ItemType { get; set; }
	private const int MaxCooldown = 3;
	public int Cooldown { get; set; }

	public void PlayAbility()
	{
		if (Cooldown > 0)
		{
			return;
		}
		Cooldown = MaxCooldown;
		
		var packet = new PlayAbility
		{
			SenderId = Global.multiplayerPlayerClass.Id,
			Ability = ItemType
		};

		Global.networkHandler.ServerPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagReliable);
	}

	public void ReduceCooldown()
	{
		if (Cooldown > 0)
			Cooldown--;
	}

}
