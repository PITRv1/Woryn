using Godot;
using System.Collections.Generic;

public partial class LanDiscovery : Node
{
    private UdpServer _udpServer;  // For server broadcasting
    private PacketPeerUdp _udpClient;  // For client listening
    
    private const int DiscoveryPort = 6768;  // Different from game port
    private const string BroadcastMessage = "GAME_SERVER";
    
    public string ServerName = "Game Server bryh";
    
    private Timer _broadcastTimer;
    private readonly List<ServerInfo> _discoveredServers = new();
    
    [Signal]
    public delegate void ServerDiscoveredEventHandler(string ip, string serverName);
    
    public class ServerInfo
    {
        public string Ip;
        public string ServerName;
        public double LastSeen;
    }
    
    public override void _Ready()
    {
        _broadcastTimer = new Timer();
        _broadcastTimer.WaitTime = 1.0f;  // Broadcast every second
        _broadcastTimer.Timeout += BroadcastServer;
        AddChild(_broadcastTimer);
    }
    
    // SERVER: Start broadcasting presence
    public void StartServerBroadcast(string serverName = "Game Server")
    {
        _udpServer = new UdpServer();
        // ServerName = serverName;
        _udpServer.Listen(DiscoveryPort, "0.0.0.0");
        _broadcastTimer.Start();
    }
    
    private void BroadcastServer()
    {
        if (_udpServer == null) return;
        
        var message = $"GAME_SERVER|{ServerName}";
        
        // Broadcast to LAN
        var peer = new PacketPeerUdp();
        peer.SetBroadcastEnabled(true);
        peer.SetDestAddress("255.255.255.255", DiscoveryPort);
        peer.PutPacket(message.ToUtf8Buffer());
        peer.Close();
    }
    
    public void StopServerBroadcast()
    {
        if (_udpClient != null)
        {
            _udpClient.Close();
            _udpClient = null;
        }
    }
    
    // CLIENT: Start listening for servers
    public void StartClientDiscovery()
    {
        StopClientDiscovery();
        
        _udpClient = new PacketPeerUdp();
        var err = _udpClient.Bind(DiscoveryPort);
    
        if (err != Error.Ok)
        {
            GD.PrintErr($"Failed to bind UDP: {err}");
            return;
        }
        
        _udpClient.SetBroadcastEnabled(true);
        _discoveredServers.Clear();
    }
    
    public void StopClientDiscovery()
    {
        _udpClient?.Close();
        _udpClient = null;
    }
    
    public override void _Process(double delta)
    {
        if (_udpClient != null && _udpClient.GetAvailablePacketCount() > 0)
        {
            var packet = _udpClient.GetPacket();
            var message = packet.GetStringFromUtf8();
            
            var parts = message.Split('|');
            
            if (parts[0] == BroadcastMessage)
            {
                var serverIp = _udpClient.GetPacketIP();
                var serverName = parts[1];
                
                // Check if we already know about this server
                var existingServer = _discoveredServers.Find(s => s.Ip == serverIp);
                
                if (existingServer == null)
                {
                    var serverInfo = new ServerInfo
                    {
                        Ip = serverIp,
                        ServerName = serverName,
                        LastSeen = Time.GetTicksMsec()
                    };
                    
                    _discoveredServers.Add(serverInfo);
                    EmitSignal(SignalName.ServerDiscovered, serverIp, serverName);
                }
                else
                {
                    existingServer.LastSeen = Time.GetTicksMsec();
                }
            }
        }
        
        // Remove stale servers (not seen in 5 seconds)
        _discoveredServers.RemoveAll(s => Time.GetTicksMsec() - s.LastSeen > 5000);
    }
    
    public List<ServerInfo> GetDiscoveredServers()
    {
        return new List<ServerInfo>(_discoveredServers);
    }
}