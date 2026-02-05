using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MultiplayerPlayerClass : Node
{
	public PlayerClass playerClass;
	public int ID;
	// List<int> ids = new();

	[Export] HBoxContainer pointCards;
	[Export] HBoxContainer modifCards;
	[Export] PackedScene pointCardUI;
	[Export] PackedScene modifierCardUI;
	[Export] Label maxPoints;
	[Export] Label points;
	[Export] Label throwDeckValue;
	[Export] PackedScene shopScene;
	
	public override void _Ready()
	{
		ID = Global.multiplayerClientGlobals._id;
		Global.multiplayerClientGlobals.HandleTurnInfo += playerClass.ProccessTurnInfoPacket;
		Global.multiplayerClientGlobals.HandlePickUpCardAnswer += playerClass.ProccessPickUpAnswer;
		Global.multiplayerClientGlobals.HandleDeckSwap += playerClass.HandleDeckSwap;

		Global.multiplayerClientGlobals.ShopScene += GoToShop;
		Global.multiplayerPlayerClass = this;
	}

	public void ClientReady()
	{
		ClientReady packet = new ClientReady();

		Global.networkHandler._serverPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagReliable);
	}

	public MultiplayerPlayerClass()
	{
		playerClass = new PlayerClass();
		playerClass.parent = this;
	}

	private void Local(int id)
	{
		ID = id;
	}

	private void GoToShop()
	{
		GetTree().ChangeSceneToPacked(shopScene);
	}

	// private void Remote(int id)
	// {
	//     ids.Add(id);
	// }

	public void SetUI(int mPoints, int plrPoints, int throwValue)
	{
		maxPoints.Text = mPoints.ToString();
		points.Text = plrPoints.ToString();
		throwDeckValue.Text = throwValue.ToString();
	}

	public void PlayCard()
	{
		if (playerClass.CanEndTurn())
			return;

		List<byte> modifIndexes = new List<byte>();

		foreach (ModifierCard card in playerClass.chosenModifierCards)
		{
			modifIndexes.Add((byte)playerClass.ModifCardList.IndexOf(card));
		}

		EndTurnRequest packet = new EndTurnRequest
		{
			SenderId = ID,
			PointCard = playerClass.chosenPointCard,
			PointCardIndex = playerClass.PointCardList.IndexOf(playerClass.chosenPointCard),
			ModifierCards = playerClass.chosenModifierCards.ToArray(),
			ModifCardIndexes = modifIndexes.ToArray()
		};

		Global.networkHandler._serverPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagReliable);
	}

	public void RemoveSelectedCards(int lastPlayer, int[] selectedPointCard, byte[] selectedModifiers)
	{
		if (lastPlayer != ID)
			return;

		foreach (int index in selectedPointCard)
		{
			pointCards.RemoveChild(pointCards.GetChild(index));
			playerClass.PointCardList.RemoveAt(index);
		}

		List<int> modifIndexes = new List<int>();

		foreach (byte card in selectedModifiers)
		{
			modifIndexes.Add(card);
			playerClass.ModifCardList.RemoveAt(card);
		}

		modifIndexes.Sort();
		modifIndexes.Reverse();

		foreach (int index in modifIndexes)
		{
			modifCards.RemoveChild(modifCards.GetChild(index));
		}

		playerClass.chosenModifierCards.Clear();
		playerClass.chosenPointCard = null;
	}

	public void PickUpCards()
	{
		PickUpCardRequest packet = new PickUpCardRequest
		{
			SenderId = ID,
		};

		Global.networkHandler._serverPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagReliable);
	}

	public void PlayAbilityRequest()
	{
		
	}

	public void SendFoldRequest()
	{
		Fold packet = new Fold
		{
			SenderId = ID	
		};

		Global.networkHandler._serverPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagReliable);
	}

	public void AddPointToContainer(PointCard pointCard)
	{
		TestPointCardUi test = pointCardUI.Instantiate() as TestPointCardUi;
		test.text.Text = pointCard.PointValue.ToString();
		test.pointCard = pointCard;
		test.playerClass = playerClass;
		pointCards.AddChild(test);
	}

	public void AddModifierToContainer(ModifierCard card)
	{
		TestModifierCardUi test = modifierCardUI.Instantiate() as TestModifierCardUi;
		test.modifierCard = card;
		test.playerClass = playerClass;


		modifCards.AddChild(test);
	}
}
