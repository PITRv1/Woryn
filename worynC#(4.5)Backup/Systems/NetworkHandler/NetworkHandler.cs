using Godot;
using System;
using System.Collections.Generic;

public partial class NetworkHandler : Node
{
    public NetworkHandler()
    {
        Global.networkHandler = this;
    }

    [Signal] public delegate void OnPeerConnectedEventHandler(int peerId);
    [Signal] public delegate void OnPeerDisconnectedEventHandler(int peerId);
    [Signal] public delegate void OnServerPacketEventHandler(int peerId, byte[] data);

    [Signal] public delegate void OnConnectedToServerEventHandler();
    [Signal] public delegate void OnDisconnectedFromServerEventHandler();
    [Signal] public delegate void OnClientPacketEventHandler(byte[] data);

    private Stack<int> _availablePeerIds = new();
    public Dictionary<int, ENetPacketPeer> _clientPeers = new();

    public ENetPacketPeer _serverPeer;

    public ENetConnection ServerConnection;
    public ENetConnection ClientConnection;

    public bool _isServer {private set; get;}
    private bool _isClient;

    public override void _Ready()
    {
        for (int i = 255; i >= 0; i--)
            _availablePeerIds.Push(i);

        GD.Print("Network Handler ready!");
    }

    public override void _Process(double delta)
    {
        if (ServerConnection != null)
            HandleServerEvents();

        if (ClientConnection != null)
            HandleClientEvents();
    }

    private void HandleServerEvents()
    {
        var ev = ServerConnection.Service();
        var type = (ENetConnection.EventType)(int)ev[0];

        if (type == ENetConnection.EventType.None)
            return;

        var peer = (ENetPacketPeer)ev[1];

        switch (type)
        {
            case ENetConnection.EventType.Connect:
                PeerConnected(peer);
                break;

            case ENetConnection.EventType.Disconnect:
                PeerDisconnected(peer);
                break;

            case ENetConnection.EventType.Receive:
                int peerId = (int)peer.GetMeta("id");
                EmitSignal(SignalName.OnServerPacket, peerId, peer.GetPacket());
                break;
        }
    }

    public void StopServer()
    {
        if (!_isServer || ServerConnection == null) return;

        GD.Print("Stopping server...");

        foreach (var peer in _clientPeers.Values) peer.PeerDisconnect();
        

        _clientPeers.Clear();
        _availablePeerIds.Clear();

        for (int i = 255; i >= 0; i--) _availablePeerIds.Push(i);

        ServerConnection.Destroy();
        ServerConnection = null;

        _isServer = false;

        GD.Print("Server stopped");
    }

    public void StartServer(string ip = "127.0.0.1", int port = 6767)
    {
        if (ServerConnection != null)
        {
            GD.Print("Server is already running!");
            return;
        }

        ServerConnection = new ENetConnection();
        Error err = ServerConnection.CreateHostBound(ip, port);

        if (err != Error.Ok)
        {
            GD.PrintErr("Server failed to start: ", err);
            ServerConnection = null;
            return;
        }

        _isServer = true;
        GD.Print("Server started");
    }

    private void PeerConnected(ENetPacketPeer peer)
    {
        int peerId = _availablePeerIds.Pop();
        peer.SetMeta("id", peerId);
        _clientPeers[peerId] = peer;

        EmitSignal(SignalName.OnPeerConnected, peerId);
        GD.Print("Peer connected: ", peerId);
    }

    private void PeerDisconnected(ENetPacketPeer peer)
    {
        int peerId = (int)peer.GetMeta("id");

        _availablePeerIds.Push(peerId);
        _clientPeers.Remove(peerId);

        EmitSignal(SignalName.OnPeerDisconnected, peerId);
        GD.Print("Peer disconnected: ", peerId);
    }

    private void HandleClientEvents()
    {
        var ev = ClientConnection.Service();
        var type = (ENetConnection.EventType)(int)ev[0];

        if (type == ENetConnection.EventType.None)
            return;

        var peer = (ENetPacketPeer)ev[1];

        switch (type)
        {
            case ENetConnection.EventType.Connect:
                ConnectedToServer();
                break;

            case ENetConnection.EventType.Disconnect:
                DisconnectedFromServer();
                break;

            case ENetConnection.EventType.Receive:
                EmitSignal(SignalName.OnClientPacket, peer.GetPacket());
                break;
        }
    }

    public void StartClient(string ip = "127.0.0.1", int port = 6767)
    {
        ClientConnection = new ENetConnection();
        Error err = ClientConnection.CreateHost(1);

        if (err != Error.Ok)
        {
            GD.PrintErr("Client failed to connect: ", err);
            ClientConnection = null;
            return;
        }

        _serverPeer = ClientConnection.ConnectToHost(ip, port);
        _isClient = true;

        GD.Print("Client connecting...");
    }

    public void DisconnectClient()
    {
        if (!_isClient || _serverPeer == null) return;

        _serverPeer.PeerDisconnect();
    }

    private void ConnectedToServer()
    {
        EmitSignal(SignalName.OnConnectedToServer);
        GD.Print("Connected to server");
    }

    private void DisconnectedFromServer()
    {
        EmitSignal(SignalName.OnDisconnectedFromServer);
        ClientConnection = null;
        _isClient = false;

        GD.Print("Disconnected from server");
    }
}
