using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class TurnManager : Node
{
	private int _currentMaxValue;
	private PointCardDeck _pointCardDeck;
	private int _currentPlayer;
	private int _lastPlayer;
	private int _playerCount = 3;
	private int _currentRound = 1;
	private Dictionary<int, MultiplayerPlayerClass> _players;
	private int _throwDeckValue;
	private int _roundDirection = 1;
	private int _skipAmount;
	private bool _throwDeckPulled;
	private Timer _foldTimer;

	public override void _Ready()
	{
		_foldTimer = new Timer
		{
			WaitTime = 30,
			OneShot = true
		};

		_foldTimer.Timeout += FoldTurn;
		AddChild(_foldTimer);

		Global.turnManagerInstance = this;
	}
	public void Setup(List<int> playerIds)
	{
		foreach (var id in playerIds)
		{
			AddToMultiplayerList(id);
		}

		_playerCount = playerIds.Count;
		_pointCardDeck = new PointCardDeck();
		_pointCardDeck.GenerateDeck();
	}

	private int[] GetPlayerIds()
	{
		return _players.Keys.ToArray();
	}

	public void PrepareGame()
	{
		GetRandomPlayer();

		foreach (var player in _players.Keys)
			DealCards(player);

		var turnInfoPacket = new SetupPacket
		{
			PlayerCount = _playerCount
		};

		BroadCast(turnInfoPacket);
	}

	private void AddToMultiplayerList(int id)
	{
		var newPlayer = new MultiplayerPlayerClass
		{
			Id = id,
		};
		
		newPlayer.PlayerClass = new PlayerClass();
		
		_players ??= new Dictionary<int, MultiplayerPlayerClass>();
		_players.Add(id, newPlayer);
	}

	private void GetRandomPlayer()
	{
		var rng = new RandomNumberGenerator();
		rng.RandiRange(0, _playerCount);
	}

	private static int CalculateCardValue(int value, ModifierCard[] cards)
	{
		return cards.Aggregate(value, (current, card) => card.Calculate(current));
	}

	private void DealCards(int id)
	{
		var playerClass = _players[id].PlayerClass;
		_pointCardDeck.PrintCards();

		var pointCards = _pointCardDeck.PullCards(playerClass.PointCardList.Count);
		var modifierCards = playerClass.ModifierCardDeck.PullCards(playerClass.ModifierCardList.Count);

		playerClass.PointCardList.AddRange(pointCards);
		playerClass.ModifierCardList.AddRange(modifierCards);

		var packet = new PickUpCardAnswer
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
		var packet = Fold.CreateFromData(data);

		if (packet.SenderId != _currentPlayer)
			return;

		FoldTurn();
	}

	private void FoldTurn()
	{
		_players[_lastPlayer].PlayerClass.Points += _throwDeckValue;
		_throwDeckValue = 0;
		_currentMaxValue = 0;

		foreach (var player in _players.Keys)
		{

			var turnInfoPacket = new TurnInfoPacket
			{
				LastPlayer = _lastPlayer,
				CurrentPlayerId = _currentPlayer,
				CurrentRound = _currentRound,
				MaxValue = _currentMaxValue,
				CurrentPointValue = _players[player].PlayerClass.Points,
				ThrowDeckValue = _throwDeckValue,
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
		var packet = PlayAbility.CreateFromData(data);

		if (packet.SenderId != _currentPlayer)
		{
			GD.Print("NOT YOUR TURN");
			return;
		}
	}

	public void PickUpCards(int id)
	{
		if (_currentPlayer != id)
		{
			GD.Print("NOT YOUR TURN");
			return;
		}

		var playerClass = _players[id].PlayerClass;

		_pointCardDeck.PrintCards();

		var pointCards = _pointCardDeck.PullCards(playerClass.PointCardList.Count);
		var modifierCards = playerClass.ModifierCardDeck.PullCards(playerClass.ModifierCardList.Count);

		playerClass.PointCardList.AddRange(pointCards);
		playerClass.ModifierCardList.AddRange(modifierCards);

		var packet = new PickUpCardAnswer
		{
			PointCards = pointCards,
			ModifierCards = modifierCards,
		};

		Global.networkHandler._clientPeers.TryGetValue(id, out var peer);

		if (peer != null)
			packet.Send(peer);
	}

	private bool DoPlayersHaveCards()
	{
		return _players.Values.Any(player => player.PlayerClass.PointCardList.Count > 0);
	}

	private void GoToShopScene()
	{
		Global.shopManager ??= new ShopManager();

		var packet = new ShopSceneChange();

		foreach (var player in _players.Keys)
		{
			Global.networkHandler._clientPeers.TryGetValue(player, out var peer);
			if (peer != null)
				packet.Send(peer);
		}
	}

	private bool CheckForEndGame(int value)
	{
		if (DoPlayersHaveCards())
			return false;
		
		_throwDeckValue += value;
		_players[_lastPlayer].PlayerClass.Points += _throwDeckValue;
		_throwDeckValue = 0;
		_currentMaxValue = 0;
		GoToShopScene();
		return true;
	}

	private void SwitchToNextPlayer()
	{
		do
		{
			_currentPlayer += _roundDirection + (_roundDirection * _skipAmount);

			_skipAmount = 0;

			if (_playerCount - 1 < _currentPlayer)
				_currentPlayer = 0;

			if (_currentPlayer < 0)
				_currentPlayer = _playerCount - 1;

		} while(_players[_currentPlayer].PlayerClass.PointCardList.Count == 0);
	}

	private void StartNewTurn(int pointCardIndex, byte[] modifierCardIndexes, List<ModifierCard> usedCards, int value)
	{
		_lastPlayer = _currentPlayer;

		foreach (var card in usedCards.Where(card => !card.IsCardModifier))
			DealWithModifiers(card);

		if (CheckForEndGame(value))
			return;

		SwitchToNextPlayer();

		_throwDeckPulled = false;
		_throwDeckValue += value;
		_currentMaxValue = value;

		foreach (var player in _players.Keys)
		{
			var packet = new TurnInfoPacket
			{
				LastPlayer = _lastPlayer,
				CurrentPlayerId = _currentPlayer,
				CurrentRound = _currentRound,
				MaxValue = _currentMaxValue,
				CurrentPointValue = _players[player].PlayerClass.Points,
				ThrowDeckValue = _throwDeckValue,
				DeletePointCards = [pointCardIndex],
				DeleteModifierCards = modifierCardIndexes
			};

			Global.networkHandler._clientPeers.TryGetValue(player, out var peer);
			if (peer != null)
				packet.Send(peer);
		}

		_foldTimer.Start();
	}

	private void SwapDeck(int playerOne, int playerTwo = -1)
	{
		var plOne = _players[playerOne].PlayerClass;
		var tempPointCards = plOne.PointCardList;
		var tempModifierCards = plOne.ModifierCardList;

		if (playerTwo == -1)
		{
			var rng = new RandomNumberGenerator();
			do
				playerTwo = rng.RandiRange(0, _playerCount - 1);
			while (playerTwo == playerOne);
		}

		var plTwo = _players[playerTwo].PlayerClass;

		plOne.PointCardList = plTwo.PointCardList;
		plOne.ModifierCardList = plTwo.ModifierCardList;

		plTwo.PointCardList = tempPointCards;
		plTwo.ModifierCardList = tempModifierCards;
	}

	private void SwapDeckAround()
	{
		var start = _roundDirection == 1 ? 0 : _playerCount - 1;
		var end   = _roundDirection == 1 ? _playerCount - 1 : 0;

		var savedPointCards = _players[start].PlayerClass.PointCardList;
		var savedModifierCards = _players[start].PlayerClass.ModifierCardList;

		var curr = start;

		while (curr != end)
		{
			var next = curr + _roundDirection;

			_players[curr].PlayerClass.PointCardList =
				_players[next].PlayerClass.PointCardList;

			_players[curr].PlayerClass.ModifierCardList =
				_players[next].PlayerClass.ModifierCardList;

			curr = next;
		}

		_players[end].PlayerClass.PointCardList = savedPointCards;
		_players[end].PlayerClass.ModifierCardList = savedModifierCards;
	}


	public void ProcessEndGameRequest(byte[] data)
	{
		var packet = EndTurnRequest.CreateFromData(data);

		GD.Print(_currentPlayer + " --- " + packet.SenderId);

		if (_currentPlayer != packet.SenderId)
			return;

		var currPlayer = _players[_currentPlayer].PlayerClass;

		var pointCard = packet.PointCard;
		var modifierCards = packet.ModifierCards;

		if (currPlayer.PointCardList[packet.PointCardIndex].PointValue != pointCard.PointValue)
			return;

		var usedCards = new List<ModifierCard>();

		for (var i = 0; i < modifierCards.Length; i++)
		{
			var tempCard = currPlayer.ModifierCardList[packet.ModifCardIndexes[i]];
			
			if (tempCard.ModifierType != modifierCards[i].ModifierType)
				return;

			usedCards.Add(currPlayer.ModifierCardList[packet.ModifCardIndexes[i]]);
		}

		var turnValue = CalculateCardValue(pointCard.PointValue, usedCards.ToArray());

		if (turnValue <= _currentMaxValue)
			return;

		currPlayer.PointCardList.RemoveAt(packet.PointCardIndex);

		var sortedIndexes = packet.ModifCardIndexes.ToList();
		sortedIndexes.Sort();
		sortedIndexes.Reverse();

		foreach (var index in sortedIndexes)
		{
			currPlayer.ModifierCardList.RemoveAt(index);			
		}

		PickUpCards(_currentPlayer);

		StartNewTurn(packet.PointCardIndex, packet.ModifCardIndexes, usedCards, turnValue);
	}

	private void SendOutNewDecks()
	{
		foreach (var player in _players.Keys)
		{
			var packet = new DeckSwap
			{
				PointCards = _players[player].PlayerClass.PointCardList.ToArray(),
				ModifierCards = _players[player].PlayerClass.ModifierCardList.ToArray()
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
				_skipAmount += 1;
				break;
			case MODIFIER_TYPES.REVERSE:
				_roundDirection *= -1;
				break;
			case MODIFIER_TYPES.GIVE_DECK_AROUND:
				SwapDeckAround();
				SendOutNewDecks();
				break;
			case MODIFIER_TYPES.CHANGE_DECK:
				SwapDeck(_currentPlayer);
				SendOutNewDecks();
				break;
			default:
				GD.Print("Something went wrong!");
				break;
		}
	}

	private void BroadCast(PacketInfo packet)
	{
		foreach (var player in _players.Keys)
		{
			Global.networkHandler._clientPeers.TryGetValue(player, out var peer);
			if (peer != null)
				packet.Send(peer);
		}
	}
}
