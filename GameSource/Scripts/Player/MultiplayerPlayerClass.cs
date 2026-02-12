using Godot;
using System.Linq;

public partial class MultiplayerPlayerClass : Node
{
	public PlayerClass PlayerClass;
	public int Id;
	// List<int> ids = new();

	[Export] private CardPlacementHandler _pointCards;
	[Export] private CardPlacementHandler _modifierCards;
	[Export] private PackedScene _pointCardUi;
	[Export] private PackedScene _modifierCardUi;
	[Export] private Node3D _playerSeatsHolder;
	[Export] private PackedScene _buddy;
	[Export] private Node3D _evilCards;

	[Export] private UiCommunicator uiCommunicator;

	[Export] private Deck3d deck3D;

	[ExportGroup("Deprecated")]
	[Export] private Label _maxPoints;
	[Export] private Label _points;
	[Export] private Label _throwDeckValue;

	
	public override void _Ready()
	{
		Id = Global.multiplayerClientGlobals.Id;
		Global.multiplayerClientGlobals.SetupPlace += Setup;
		Global.multiplayerClientGlobals.HandleTurnInfo += PlayerClass.ProcessTurnInfoPacket;
		Global.multiplayerClientGlobals.HandlePickUpCardAnswer += PlayerClass.ProcessPickUpAnswer;
		Global.multiplayerClientGlobals.HandleDeckSwap += PlayerClass.HandleDeckSwap;
		Global.multiplayerClientGlobals.ShopScene += uiCommunicator.StartShop;
		Global.turnManagerInstance.GoToShopScene();
		GD.Print("Dani: Shop will start with the first round for testing purposes. \nComment out line 36 in MultiplayerPlayerClass.cs");
		
		// REMOVES EVIL CARDS
		_evilCards.QueueFree();

		Global.multiplayerPlayerClass = this;
		ClientReady();
	}

	private void Setup(byte[] data)
	{
		var packet = SetupPacket.CreateFromData(data);
		var seats = _playerSeatsHolder.GetChildren();
		for (var i = 0; i < packet.PlayerCount; i++)
		{
			var bud = _buddy.Instantiate() as Node3D;
			seats[i].AddChild(bud);
		}
	}

	private static void ClientReady()
	{
		var packet = new ClientReady();
		Global.networkHandler.ServerPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagReliable);
	}

	public MultiplayerPlayerClass()
	{
		PlayerClass = new PlayerClass
		{
			Parent = this
		};
	}

	private void Local(int id)
	{
		Id = id;
	}

	public void SetUi(int mPoints, int plrPoints, int throwValue)
	{
		deck3D.UpdateTotalValueText(throwValue);
		deck3D.UpdateCurrentValueText(mPoints);
		GD.Print("Dani will update the SetUI method.");
		// _maxPoints.Text = mPoints.ToString();
		// _points.Text = plrPoints.ToString();
		// _throwDeckValue.Text = throwValue.ToString();
	}

	public void PlayCard()
	{
		GD.Print("Play card called");
		if (!PlayerClass.CanEndTurn())
		{
			GD.Print("Player cannot end turn");			
			return;
		}

		var packet = new EndTurnRequest
		{
			SenderId = Id,
			PointCard = PlayerClass.ChosenPointCard,
			PointCardIndex = PlayerClass.PointCardList.IndexOf(PlayerClass.ChosenPointCard),
			ModifierCards = PlayerClass.ChosenModifierCards.ToArray(),
			ModifCardIndexes = PlayerClass.ChosenModifierCards.Select(card => (byte)PlayerClass.ModifierCardList.IndexOf(card)).ToArray()
		};

		Global.networkHandler.ServerPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagReliable);
	}

	private void PickUpCards()
	{
		var packet = new PickUpCardRequest
		{
			SenderId = Id,
		};
		Global.networkHandler.ServerPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagReliable);
	}

	private void PlayAbilityRequest()
	{
		
	}

	public void SendFoldRequest()
	{
		var packet = new Fold
		{
			SenderId = Id	
		};

		Global.networkHandler.ServerPeer?.Send(0, packet.Encode(), (int)ENetPacketPeer.FlagReliable);
	}

	public void AddPointToContainer(PointCard pointCard)
	{
		if (_pointCardUi.Instantiate() is not PointCard3d card) return;
		card.PointCard = pointCard;
		
		_pointCards.AddCard(card);
	}

	public void AddModifierToContainer(ModifierCard modifierCard)
	{
		if (_modifierCardUi.Instantiate() is not ModifierCard3d card) return;
		card.ModifierCard = modifierCard;

		_modifierCards.AddCard(card);
	}

	public void ResetContainers()
	{
		for (var i = 0; i < _modifierCards.GetChildCount(); i++)
		{
			_modifierCards.RemoveChild(_modifierCards.GetChild(1));
		}

		for (var i = 0; i < _pointCards.GetChildCount(); i++)
		{
			_pointCards.RemoveChild(_pointCards.GetChild(1));
		}
	}
}
