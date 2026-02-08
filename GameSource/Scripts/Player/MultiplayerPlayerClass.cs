using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MultiplayerPlayerClass : Node
{
	public PlayerClass playerClass;
	public int ID;
	// List<int> ids = new();

	[Export] CardPlacementHandler pointCards;
	[Export] CardPlacementHandler modifCards;
	[Export] PackedScene pointCardUI;
	[Export] PackedScene modifierCardUI;
	[Export] Node3D playerSeats;

	[ExportGroup("Deprecated")]
	[Export] Label maxPoints;
	[Export] Label points;
	[Export] Label throwDeckValue;
	[Export] PackedScene shopScene;
	[Export] PackedScene buddy;

	
	public override void _Ready()
	{
		ID = Global.multiplayerClientGlobals._id;
		Global.multiplayerClientGlobals.SetupPlace += Setup;
		Global.multiplayerClientGlobals.HandleTurnInfo += playerClass.ProccessTurnInfoPacket;
		Global.multiplayerClientGlobals.HandlePickUpCardAnswer += playerClass.ProccessPickUpAnswer;
		Global.multiplayerClientGlobals.HandleDeckSwap += playerClass.HandleDeckSwap;
		Global.multiplayerClientGlobals.HandleDeckSwap += Test;

		Global.multiplayerClientGlobals.ShopScene += GoToShop;
		Global.multiplayerPlayerClass = this;
	}

	public void Test(byte[] data)
	{
		GD.Print("Help");
	}

	private void Setup(byte[] data)
	{
		SetupPacket packet = SetupPacket.CreateFromData(data);
		for (int i = 0; i < packet.PlayerCount - 1; i++)
		{
			Node3D bud = buddy.Instantiate() as Node3D;
			playerSeats.GetChild(i).AddChild(bud);
		}
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
		GetTree().CurrentScene.AddChild(shopScene.Instantiate());
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
		// if (lastPlayer != ID)
		// 	return;

		// foreach (int index in selectedPointCard)
		// {
		// 	pointCards.RemoveChild(pointCards.GetChild(index));
		// 	playerClass.PointCardList.RemoveAt(index);
		// }

		// List<int> modifIndexes = new List<int>();

		// foreach (byte card in selectedModifiers)
		// {
		// 	modifIndexes.Add(card);
		// 	playerClass.ModifCardList.RemoveAt(card);
		// }

		// modifIndexes.Sort();
		// modifIndexes.Reverse();

		// foreach (int index in modifIndexes)
		// {
		// 	modifCards.RemoveChild(modifCards.GetChild(index));
		// }

		// playerClass.chosenModifierCards.Clear();
		// playerClass.chosenPointCard = null;
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
		PointCard3d card = pointCardUI.Instantiate() as PointCard3d;
		card.PointCard = pointCard;

		pointCards.AddCard(card);
	}

	public void AddModifierToContainer(ModifierCard modifCard)
	{
		ModifierCard3d card = pointCardUI.Instantiate() as ModifierCard3d;
		card.ModifierCard = modifCard;

		modifCards.AddCard(card);
	}

	public void ResetContainers()
	{
		for (int i = 0; i < modifCards.GetChildCount(); i++)
		{
			modifCards.RemoveChild(modifCards.GetChild(1));
		}

		for (int i = 0; i < pointCards.GetChildCount(); i++)
		{
			pointCards.RemoveChild(pointCards.GetChild(1));
		}
	}
}
