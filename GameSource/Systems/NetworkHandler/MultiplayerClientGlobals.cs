using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class MultiplayerClientGlobals : Node
{
    public MultiplayerClientGlobals()
    {
        Global.multiplayerClientGlobals = this;
    }

    [Signal]
    public delegate void HandleLocalIdAssignmentEventHandler(int localId);

    [Signal]
    public delegate void HandleRemoteIdAssignmentEventHandler(int remoteId);
    [Signal]
    public delegate void HandleTurnInfoEventHandler(byte[] data);
    [Signal]
    public delegate void HandlePickUpCardAnswerEventHandler(byte[] data);
    [Signal]
    public delegate void HandleDeckSwapEventHandler(byte[] data);
    [Signal]
    public delegate void NewPlayerEventHandler(byte[] data);
    [Signal]
    public delegate void StartGameEventHandler();
    [Signal]
    public delegate void CursorUpdateEventHandler(byte[] data);
    [Signal]
    public delegate void ShopSceneEventHandler();
    [Signal]
    public delegate void ShopItemsEventHandler();
    [Signal]
    public delegate void SetupPlaceEventHandler(byte[] data);
    [Signal]
    public delegate void HandleRoundSuccessEventHandler(byte[] data);

    public int Id = -1;
    public List<int> RemoteIds = new();

    public override void _Ready()
    {
        Global.networkHandler.OnClientPacket += OnClientPacket;
    }

    private void OnClientPacket(byte[] data)
    {
        var packetType = (PACKET_TYPES)data[0];

        switch (packetType)
        {
            case PACKET_TYPES.ID_ASSIGNMENT:
                ManageIds(IDAssignment.CreateFromData(data));
                break;
            case PACKET_TYPES.START_GAME:
                EmitSignal("StartGame");
                break;
            case PACKET_TYPES.NEW_PLAYER:
                EmitSignal("NewPlayer", data);
                break;
            case PACKET_TYPES.TURN_INFO:
                EmitSignal("HandleTurnInfo", data);
                break;
            case PACKET_TYPES.PICK_UP_CARD_ANSWER:
                EmitSignal("HandlePickUpCardAnswer", data);
                break;
            case PACKET_TYPES.CURSOR_UPDATE:
                EmitSignal("CursorUpdate", data);
                break;
            case PACKET_TYPES.SHOP_SCENE_CHANGE:
                EmitSignal("ShopScene");
                break;
            case PACKET_TYPES.DECK_SWAP:
                EmitSignal("HandleDeckSwap", data);
                break;
            case PACKET_TYPES.SETUP_PLACE:
                EmitSignal("SetupPlace", data);
                break;
            case PACKET_TYPES.ROUND_SUCCESS:
                EmitSignal("HandleRoundSuccess", data);
                break;
            case PACKET_TYPES.SHOP_ITEMS:
                EmitSignal(SignalName.ShopItems, data);
                break;
            default:
                GD.PushError($"Packet type with index {(int)packetType} unhandled!");
                break;
        }
    }

    private void ManageIds(IDAssignment idAssignment)
    {
        // local client ID
        if (Id == -1)
        {
            Id = idAssignment.Id;
            EmitSignal(SignalName.HandleLocalIdAssignment, Id);

            RemoteIds = idAssignment.RemoteIds;

            foreach (var remoteId in RemoteIds.Where(remoteId => remoteId != Id))
            {
                EmitSignal(SignalName.HandleRemoteIdAssignment, remoteId);
            }
        }
        // new remote peers
        else
        {
            RemoteIds.Add(idAssignment.Id);
            EmitSignal(SignalName.HandleRemoteIdAssignment, idAssignment.Id);
        }
    }
}
