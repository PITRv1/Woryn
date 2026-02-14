using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class MultiplayerServerGlobals : Node
{
    public MultiplayerServerGlobals()
    {
        Global.multiplayerServerGlobals = this;
    }

    private readonly List<int> _peerIds = new();
    private readonly HashSet<int> _readyPlayers = new();

    public override void _Ready()
    {
        var network = Global.networkHandler;

        network.OnPeerConnected += OnPeerConnected;
        network.OnPeerDisconnected += OnPeerDisconnected;
        network.OnServerPacket += OnServerPacket;
        Global.networkHandler.OnServerStopped += () => Global.lobbyManagerInstance.ResetPlayList();
    }

    private void OnPeerConnected(int peerId)
    {
        _peerIds.Add(peerId);

        IDAssignment
            .Create(peerId, _peerIds)
            .Broadcast(Global.networkHandler.ServerConnection);
        
        Global.lobbyManagerInstance ??= new LobbyManager();

        Global.lobbyManagerInstance.AddToMultiplayerList(peerId);
    }

    private void OnPeerDisconnected(int peerId)
    {
        _peerIds.Remove(peerId);
        GD.Print("Peer disconnected");
        Global.lobbyManagerInstance.RemoveFromMultiplayerList(peerId);
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
                {
                    Global.turnManagerInstance.PrepareGame();
                }
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
                Global.turnManagerInstance.ProcessEndGameRequest(data);
                break;
            case PACKET_TYPES.CURSOR_UPDATE:
                foreach (var pair in Global.networkHandler.ClientPeers.Where(pair => pair.Key != peerId))
                {
                    pair.Value.Send(0, data, (int)ENetPacketPeer.FlagUnsequenced);
                }
                break;
            case PACKET_TYPES.PLAY_ABILITY:
                Global.turnManagerInstance.PlayPlayerAbility(data);
                break;
            case PACKET_TYPES.FOLD:
                Global.turnManagerInstance.CheckFoldRequest(data);
                break;
            case PACKET_TYPES.LOOK_AT_PACKET:
                var packet = LookAtPacket.CreateFromData(data);
                foreach (var player in _peerIds)
                {
                    Global.networkHandler.ClientPeers.TryGetValue(player, out var peer);
                    if (peer != null)
                    {
                        packet.Send(peer);
                    }
                }
                break;
            case PACKET_TYPES.GOLD_CONVERT:
                Global.turnManagerInstance.HandleGoldConvert(data);
                break;
            case PACKET_TYPES.SHOP_READY:
                Global.turnManagerInstance.AddToShopReady(data);
                break;
            case PACKET_TYPES.SHOP_ITEM_BUY:
                Global.shopManagerInstance.HandleShopItemBuy(data);
                break;
            default:
                GD.PushError($"Packet type with index {(PACKET_TYPES)data[0]} unhandled");
                break;
        }
    }
}
