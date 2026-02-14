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

	private int _amountOfPublicItems = 4;
	private int _amountOfPrivateItems = 8;
	private List<ItemType> _currentPublicShopItems;
	private Dictionary<int, List<MODIFIER_TYPES>> _currentPrivateShopItemsPerPlayer;
	private List<int> _currentPublicPrices;
	private Dictionary<int, List<int>> _currentPrivatePrices;
	private Timer _shopTimer;


	private static readonly List<ItemType> WeakActives = Enum.GetValues(typeof(ItemType))
		.Cast<ItemType>()
		.Where(e => e.ToString().EndsWith("_PASSIVE"))
		.ToList();

	private static readonly List<MODIFIER_TYPES> Modifiers = Enum.GetValues(typeof(MODIFIER_TYPES))
		.Cast<MODIFIER_TYPES>()
		.Skip(1)
		.ToList();

	public override void _Ready()
	{
		_shopTimer = new Timer
		{
			WaitTime = 10,
			OneShot = true
		};

		_shopTimer.Timeout += StopShop;
		AddChild(_shopTimer);
		Global.shopManagerInstance = this;
		_shopTimer.Stop();
	}

	private void StopShop()
	{
		var packet = new ClientReady();

		Global.turnManagerInstance.BroadCast(packet);

		Global.turnManagerInstance.Reset();
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

	public void GeneratePublicShop()
	{
		GD.Print("Generate shop called!!");
		var rng = new RandomNumberGenerator();
		var publicItems = new List<ItemType>();
		var publicPrices = new List<int>();
		for (var i = 0; i < _amountOfPublicItems; i++)
		{
			publicItems.Add(GetRandomWeakActive());
			publicPrices.Add(rng.RandiRange(8, 12));
		}

		var privateItems = new Dictionary<int, List<MODIFIER_TYPES>>();
		var privatePrices = new Dictionary<int, List<int>>();
		foreach (var player in Global.turnManagerInstance.Players.Keys)
		{
			if (!privateItems.TryGetValue(player, out List<MODIFIER_TYPES> value))
			{
				value = new List<MODIFIER_TYPES>();
				privateItems.Add(player, value);
				privatePrices.Add(player, new List<int>());
			}
			for (var i = 0; i < _amountOfPrivateItems; i++)
			{
				value.Add(GetRandomModifier());
				privatePrices[player].Add(rng.RandiRange(3, 7));
			}
		}

		_currentPublicShopItems = publicItems;
		_currentPrivateShopItemsPerPlayer = privateItems;
		_currentPublicPrices = publicPrices;
		_currentPrivatePrices = privatePrices;

		foreach (var player in Global.turnManagerInstance.Players.Keys)
		{
			Global.networkHandler.ClientPeers.TryGetValue(player, out var peer);
			var packet = new ShopItems
			{
				ItemTypes = publicItems.ToArray(),
				ModifierTypes = privateItems[player].ToArray(),
				itemPrices = publicPrices,
				modifierPrices = privatePrices[player]
			};
			if (peer != null)
			{
				packet.Send(peer);
			}
		}

		_shopTimer.Start();
	}

	public void HandleShopItemBuy(byte[] data)
	{
		var packet = ShopItemBuy.CreateFromData(data);

		var player = Global.turnManagerInstance.Players[packet.SenderId].PlayerClass;
		var card = ModifierCardTypeConverter.TypeToClass(_currentPrivateShopItemsPerPlayer[packet.SenderId][packet.CardIndex]);
		var price = _currentPrivatePrices[packet.SenderId][packet.CardIndex];

		GD.Print("Card index shop manger: " + packet.CardIndex);

		if (player.Gold < price)
		{
			return;
		}

		player.ModifierCardDeck.modifierCards.Add(card);
		_currentPrivateShopItemsPerPlayer[packet.SenderId].RemoveAt(packet.CardIndex);
		_currentPrivatePrices[packet.SenderId].RemoveAt(packet.CardIndex);
		player.Gold -= price;

		Global.networkHandler.ClientPeers.TryGetValue(packet.SenderId, out var peer);
		var returnPacket = new ShopItemBuy
		{
			CardIndex = packet.CardIndex,
			GoldAmount = player.Gold

		};
		if (peer != null)
		{
			returnPacket.Send(peer);
		}

	}
	
}
