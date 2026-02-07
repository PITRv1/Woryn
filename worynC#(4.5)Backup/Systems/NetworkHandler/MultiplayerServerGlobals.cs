using Godot;
using System.Collections.Generic;

public partial class MultiplayerServerGlobals : Node
{
    public MultiplayerServerGlobals()
    {
        Global.multiplayerServerGlobals = this;
    }

    private List<int> _peerIds = new();
    private HashSet<int> _readyPlayers = new();

    public override void _Ready()
    {
        NetworkHandler network = Global.networkHandler;

        network.OnPeerConnected += OnPeerConnected;
        network.OnPeerDisconnected += OnPeerDisconnected;
        network.OnServerPacket += OnServerPacket;
        
    }

    private void OnPeerConnected(int peerId)
    {
        _peerIds.Add(peerId);

        IDAssignment
            .Create(peerId, _peerIds)
            .Broadcast(Global.networkHandler.ServerConnection);

        if (Global.lobbyManagerInstance == null)
            Global.lobbyManagerInstance = new LobbyManager();

        Global.lobbyManagerInstance.AddToMultiplayerList(peerId);
    }

    private void OnPeerDisconnected(int peerId)
    {
        _peerIds.Remove(peerId);
    }

    private void OnServerPacket(int peerId, byte[] data)
    {
        switch ((PACKET_TYPES)data[0])
        {
            case PACKET_TYPES.START_GAME:
                if (peerId != 0)
                    return;
                Global.turnManagerInstance.Setup(_peerIds);
                Global.lobbyManagerInstance.StartGameRequest(data);
                break;
            case PACKET_TYPES.CLIENT_READY:
                GD.Print("Client is ready");
                GD.Print(_readyPlayers.Count + " --- Clients --- " + _peerIds.Count);
                _readyPlayers.Add(peerId);
                if (_readyPlayers.Count == _peerIds.Count)
                    Global.turnManagerInstance.PrepareGame();
                break;
            case PACKET_TYPES.TURN_DATA:
                GD.PushError("Dani has no idea how we should handle this kind of packet.");
                break;
            case PACKET_TYPES.TURN_INFO:
                TurnInfoPacket turnPacket = TurnInfoPacket.CreateFromData(data);
                // Global.turnManagerInstance.ProccessTurnInfo(turnPacket);
                break;
            case PACKET_TYPES.PICK_UP_CARD_REQUEST:
                Global.turnManagerInstance.PickUpCards(peerId);
                break;
            case PACKET_TYPES.END_TURN_REQUEST:
                Global.turnManagerInstance.ProccessEndGameRequest(data);
                break;
            case PACKET_TYPES.CURSOR_UPDATE:
                foreach (var pair in Global.networkHandler._clientPeers)
                {
                    if (pair.Key == peerId)
                        continue;

                    pair.Value.Send(0, data, (int)ENetPacketPeer.FlagUnsequenced);
                }
                break;
            case PACKET_TYPES.PLAY_ABLITIY:
                Global.turnManagerInstance.PlayPlayerAbility(data);
                break;
            case PACKET_TYPES.FOLD:
                Global.turnManagerInstance.CheckFoldRequest(data);
                break;
            default:
                GD.PushError($"Packet type with index {(PACKET_TYPES)data[0]} unhandled");
                break;
        }
    }
}
