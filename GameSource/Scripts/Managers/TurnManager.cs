using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public partial class TurnManager : Node
{
	private int currentMaxValue = 0;
	// private List<ModifierCard> modifierCardsPlayed;
	private PointCardDeck pointCardDeck;
	private int currentPlayer = 0;
	private int lastPlayer = 0;
	private int playerCount = 3;
	private int CurrentRound = 1;
	private Dictionary<int, MultiplayerPlayerClass> players;
	private int ThrowDeckValue = 0;
	private int roundDirection = 1;
	private int skipAmount = 0;
	// private bool RoundOver
	public bool throwDeckPulled = false;
	private Timer foldTimer;

    public override void _Ready()
    {
		foldTimer = new Timer
		{
			WaitTime = 30,
			OneShot = true
		};

		foldTimer.Timeout += () => FoldTurn();
		AddChild(foldTimer);

        Global.turnManagerInstance = this;
    }
	public void Setup(List<int> playerIds)
	{
		GD.Print("GUH");
		foreach (int id in playerIds)
		{
			AddToMultiplayerList(id);
		}

		playerCount = playerIds.Count;

		pointCardDeck = new PointCardDeck();
		
		pointCardDeck.GenerateDeck();
	}

	public int[] GetPlayerIds()
	{
		return players.Keys.ToArray();
	}

	public void PrepareGame()
	{
		GetRandomPlayer();

		foreach (int player in players.Keys)
		{
			DealCards(player);
		}

		foreach (int player in players.Keys)
		{

			SetupPacket turnInfoPacket = new SetupPacket
			{
				PlayerCount = playerCount
			};

			Global.networkHandler._clientPeers.TryGetValue(player, out var peer);
			if (peer != null)
			{
				turnInfoPacket.Send(peer);
			}
		}
	}

	public void AddToMultiplayerList(int id)
	{
		MultiplayerPlayerClass newPlayer = new MultiplayerPlayerClass
		{
			ID = id,
		};
		if (players == null)
			players = new Dictionary<int, MultiplayerPlayerClass>();
		players.Add(id, newPlayer);
	}

	private void GetRandomPlayer()
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		// currentPlayer = rng.RandiRange(0, playerCount - 1);
	}

	// public void SetPointCardValue(int value)
	// {
	// 	pointCardValue = value;
	// }

	// public void AddCardToModifierCards(ModifierCard card)
	// {
	// 	modifierCardsPlayed.Add(card);
	// }

	// public void RemoveFromModifierCards(ModifierCard card)
	// {
	// 	modifierCardsPlayed.Remove(card);
	// }

	private int CalculateCardValue(int value, ModifierCard[] cards)
	{
		foreach (ModifierCard card in cards)
		{
			value = card.Calculate(value);
		}

		return value;
	}

	public void DealCards(int id)
	{
		PlayerClass playerClass = players[id].playerClass;
		PointCard[] pointCards;
		ModifierCard[] modifierCards;

		pointCardDeck.PrintCards();

		pointCards = pointCardDeck.PullCards(playerClass.PointCardList.Count);
		modifierCards = playerClass.modifierCardDeck.PullCards(playerClass.ModifCardList.Count);

		playerClass.PointCardList.AddRange(pointCards);
		playerClass.ModifCardList.AddRange(modifierCards);

		PickUpCardAnswer packet = new PickUpCardAnswer
		{
			PointCards = pointCards,
			ModifierCards = modifierCards,
		};

		Global.networkHandler._clientPeers.TryGetValue(id, out var peer);

		if (peer != null)
		{
			packet.Send(peer);
		}
	}

	public void CheckFoldRequest(byte[] data)
	{
		Fold packet = Fold.CreateFromData(data);

		if (packet.SenderId != currentPlayer)
			return;

		FoldTurn();
	}

	public void FoldTurn()
	{
		GD.Print("FOLDING");

		players[lastPlayer].playerClass.Points += ThrowDeckValue;
		ThrowDeckValue = 0;
		currentMaxValue = 0;

		foreach (int player in players.Keys)
		{

			TurnInfoPacket turnInfoPacket = new TurnInfoPacket
			{
				LastPlayer = lastPlayer,
				CurrentPlayerId = currentPlayer,
				CurrentRound = CurrentRound,
				MaxValue = currentMaxValue,
				CurrentPointValue = players[player].playerClass.Points,
				ThrowDeckValue = ThrowDeckValue,
				DeletePointCards = [],
				DeleteModifierCards = [],
			};

			Global.networkHandler._clientPeers.TryGetValue(player, out var peer);
			if (peer != null)
			{
				turnInfoPacket.Send(peer);
			}
		}
	}

	public void PlayPlayerAbility(byte[] data)
	{
		PlayAbility packet = PlayAbility.CreateFromData(data);

		if (packet.SenderId != currentPlayer)
		{
			GD.Print("NOT YOUR TURN");
			return;
		}

		

	}

	public void PickUpCards(int id)
	{
		if (currentPlayer != id)
		{
			GD.Print("NOT YOUR TURN");
			return;
		}

		PlayerClass playerClass = players[id].playerClass;
		PointCard[] pointCards;
		ModifierCard[] modifierCards;

		pointCardDeck.PrintCards();

		pointCards = pointCardDeck.PullCards(playerClass.PointCardList.Count);
		modifierCards = playerClass.modifierCardDeck.PullCards(playerClass.ModifCardList.Count);

		playerClass.PointCardList.AddRange(pointCards);
		playerClass.ModifCardList.AddRange(modifierCards);

		PickUpCardAnswer packet = new PickUpCardAnswer
		{
			PointCards = pointCards,
			ModifierCards = modifierCards,
		};

		Global.networkHandler._clientPeers.TryGetValue(id, out var peer);

		if (peer != null)
			packet.Send(peer);
	}

	private bool DoesPlayerOwnModifiers(ModifierCard[] cards)
	{
		List<MODIFIER_TYPES> types = ModifierCardTypeConverter.ClassListToTypeList(players[currentPlayer].playerClass.ModifCardList);
		foreach (ModifierCard card in cards)
		{
			if (!types.Contains(card.ModifierType))
				return false;
		}
		return true;
	}

	private List<int> GetCardListValues(List<PointCard> cards)
	{
		List<int> values = new List<int>();

		foreach (PointCard card in cards)
		{
			values.Add(card.PointValue);
		}

		return values;
	}

	private bool DoPlayersHaveCards()
	{
		foreach (MultiplayerPlayerClass player in players.Values)
			if (player.playerClass.PointCardList.Count > 0)
				return true;

		return false;
	}

	private void GoToShopScene()
	{
		if (Global.shopManager == null)
			Global.shopManager = new ShopManager();

		ShopSceneChange packet = new ShopSceneChange();

		foreach (int player in players.Keys)
		{
			Global.networkHandler._clientPeers.TryGetValue(player, out var peer);
			if (peer != null)
			{
				packet.Send(peer);
			}
		}
	}

	private void StartNewTurn(int pointCardIndex, byte[] modifierCardIndexes, List<ModifierCard> usedCards, int value, bool fold)
	{
		lastPlayer = currentPlayer;

		foreach (ModifierCard card in usedCards)
			if (!card.IsCardModifier)
				DealWithModifiers(card);

		do
		{
			currentPlayer += roundDirection + (roundDirection * skipAmount);

			skipAmount = 0;

			if (playerCount - 1 < currentPlayer)
				currentPlayer = 0;

			if (currentPlayer < 0)
				currentPlayer = playerCount - 1;

		} while(players[currentPlayer].playerClass.PointCardList.Count == 0 && DoPlayersHaveCards());

		if (!DoPlayersHaveCards())
		{
			ThrowDeckValue += value;
			players[lastPlayer].playerClass.Points += ThrowDeckValue;
			ThrowDeckValue = 0;
			currentMaxValue = 0;
			GoToShopScene();
			return;
		}	

		if (fold)
		{
		}
		else
		{
			throwDeckPulled=false;
			ThrowDeckValue += value;
			currentMaxValue = value;
		}

		GD.Print("Point Card Index: " + pointCardIndex);

		foreach (int player in players.Keys)
		{

			TurnInfoPacket packet = new TurnInfoPacket
			{
				LastPlayer = lastPlayer,
				CurrentPlayerId = currentPlayer,
				CurrentRound = CurrentRound,
				MaxValue = currentMaxValue,
				CurrentPointValue = players[player].playerClass.Points,
				ThrowDeckValue = ThrowDeckValue,
				DeletePointCards = new int [] { pointCardIndex },
				DeleteModifierCards = modifierCardIndexes
			};

			Global.networkHandler._clientPeers.TryGetValue(player, out var peer);
			if (peer != null)
			{
				packet.Send(peer);
			}
		}

		foldTimer.Start();
	}

	public void SwapDeck(int playerOne, int playerTwo = -1)
	{
		PlayerClass plOne = players[playerOne].playerClass;
		PlayerClass plTwo;
		List<PointCard> tempPointCards = plOne.PointCardList;
		List<ModifierCard> tempModifierCards = plOne.ModifCardList;

		if (playerTwo == -1)
		{
			RandomNumberGenerator rng = new RandomNumberGenerator();
			do
				playerTwo = rng.RandiRange(0, playerCount - 1);
			while (playerTwo == playerOne);
		}

		plTwo = players[playerTwo].playerClass;

		plOne.PointCardList = plTwo.PointCardList;
		plOne.ModifCardList = plTwo.ModifCardList;

		plTwo.PointCardList = tempPointCards;
		plTwo.ModifCardList = tempModifierCards;
	}

	public void SwapDeckAround()
	{
		int start = roundDirection == 1 ? 0 : playerCount - 1;
		int end   = roundDirection == 1 ? playerCount - 1 : 0;

		var savedPointCards = players[start].playerClass.PointCardList;
		var savedModifierCards = players[start].playerClass.ModifCardList;

		int curr = start;

		while (curr != end)
		{
			int next = curr + roundDirection;

			players[curr].playerClass.PointCardList =
				players[next].playerClass.PointCardList;

			players[curr].playerClass.ModifCardList =
				players[next].playerClass.ModifCardList;

			curr = next;
		}

		players[end].playerClass.PointCardList = savedPointCards;
		players[end].playerClass.ModifCardList = savedModifierCards;
	}


	public void ProccessEndGameRequest(byte[] data)
	{
		EndTurnRequest packet = EndTurnRequest.CreateFromData(data);

		GD.Print(currentPlayer + " --- " + packet.SenderId);

		if (currentPlayer != packet.SenderId)
			return;

		PlayerClass currPlayer = players[currentPlayer].playerClass;

		PointCard pointCard = packet.PointCard;
		ModifierCard[] modifierCards = packet.ModifierCards;

		if (currPlayer.PointCardList[packet.PointCardIndex].PointValue != pointCard.PointValue)
			return;

		List<ModifierCard> usedCards = new List<ModifierCard>();

		ModifierCard tempCard;
		for (int i = 0; i < modifierCards.Length; i++)
		{
			tempCard = currPlayer.ModifCardList[packet.ModifCardIndexes[i]];
			
			if (tempCard.ModifierType != modifierCards[i].ModifierType)
				return;

			usedCards.Add(currPlayer.ModifCardList[packet.ModifCardIndexes[i]]);
		}

		int turnValue = CalculateCardValue(pointCard.PointValue, usedCards.ToArray());

		if (turnValue <= currentMaxValue)
			return;

		currPlayer.PointCardList.RemoveAt(packet.PointCardIndex);

		List<byte> sortedIndexes = packet.ModifCardIndexes.ToList();
		sortedIndexes.Sort();
		sortedIndexes.Reverse();

		foreach (byte index in sortedIndexes)
		{
			currPlayer.ModifCardList.RemoveAt(index);			
		}

		PickUpCards(currentPlayer);

		StartNewTurn(packet.PointCardIndex, packet.ModifCardIndexes, usedCards, turnValue, false);
	}

	private void SendOutNewDecks()
	{
		foreach (int player in players.Keys)
		{

			DeckSwap packet = new DeckSwap
			{
				PointCards = players[player].playerClass.PointCardList.ToArray(),
				ModifierCards = players[player].playerClass.ModifCardList.ToArray()
			};

			Global.networkHandler._clientPeers.TryGetValue(player, out var peer);
			if (peer != null)
			{
				packet.Send(peer);
			}
		}
	}

	private void DealWithModifiers(ModifierCard card)
	{
		switch (card.ModifierType)
		{
			case MODIFIER_TYPES.SKIP:
				skipAmount += 1;
				break;
			case MODIFIER_TYPES.REVERSE:
				roundDirection *= -1;
				break;
			case MODIFIER_TYPES.GIVE_DECK_AROUND:
				SwapDeckAround();
				SendOutNewDecks();
				break;
			case MODIFIER_TYPES.CHANGE_DECK:
				SwapDeck(currentPlayer);
				SendOutNewDecks();
				break;

		}

	}

}
