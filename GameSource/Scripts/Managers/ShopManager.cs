using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ShopManager : Node
{
	// generate modif card
    // generate fortnite battlepass balls (passive spells)
    // remove from ui (both modif and spells)
    // add card to turn peterventory

    private int _amountOfPublicItems = 8;
    private int _amountOfPrivateItems = 8;
    private List<ItemType> _currentPublicShopItems;
    private Dictionary<int, List<MODIFIER_TYPES>> _currentPrivateShopItemsPerPlayer;
    
    private static readonly List<ItemType> WeakActives = Enum.GetValues(typeof(ItemType))
	    .Cast<ItemType>()
	    .Where(e => e.ToString().EndsWith("_WEAK_ACTIVE") || e.ToString().EndsWith("_PASSIVE"))
	    .ToList();
    
    private static readonly List<MODIFIER_TYPES> Modifiers = Enum.GetValues(typeof(MODIFIER_TYPES))
	    .Cast<MODIFIER_TYPES>()
	    .ToList();
    
    public override void _Ready()
    {
	    Global.shopManagerInstance = this;
    }
    
    private static ItemType GetRandomWeakActive()
    {
	    var rng = new RandomNumberGenerator();
	    rng.Randomize();
	    
	    return WeakActives[rng.RandiRange(0, WeakActives.Count - 1)];
    }

    private static MODIFIER_TYPES GetRandomModifier()
    {
	    var rng = new RandomNumberGenerator();
	    rng.Randomize();
	    
	    return Modifiers[rng.RandiRange(0, Modifiers.Count - 1)];
    }

    private void GeneratePublicShop()
    {
	    var publicItems = new List<ItemType>();
	    for (var i = 0; i < _amountOfPublicItems; i++)
	    {
		    publicItems.Add(GetRandomWeakActive());
	    }

	    var privateItems = new Dictionary<int, List<MODIFIER_TYPES>>();
	    foreach (var player in Global.turnManagerInstance.Players.Keys)
	    {
		    privateItems[player] ??= new List<MODIFIER_TYPES>();
		    privateItems[player].Add(GetRandomModifier());
	    }
	    
	    _currentPublicShopItems = publicItems;
	    _currentPrivateShopItemsPerPlayer = privateItems;
	    
		foreach (var player in Global.turnManagerInstance.Players.Keys)
		{
			Global.networkHandler.ClientPeers.TryGetValue(player, out var peer);
			var packet = new ShopItems
			{
				ItemTypes = publicItems.ToArray(),
				ModifierTypes = privateItems[player].ToArray()
			};
			if (peer != null)
			{
				packet.Send(peer);
			}
		}
    }
}
