extends Node

signal on_peer_connected(peer_id : int)
signal on_peer_disconnected(peer_id : int)
signal on_server_packet(peer_id : int, data : PackedByteArray)

signal on_connected_to_server()
signal on_disconnected_from_server()
signal on_client_packet(data : PackedByteArray)


var available_peer_ids : Array = range(255, -1, -1) 
var client_peers : Dictionary[int, ENetPacketPeer]
var server_peer : ENetPacketPeer
var connection : ENetConnection
var is_server : bool = false


func _process(_delta: float) -> void:
	if connection == null: return
	
	_handle_events()
	

func _handle_events() -> void:
	var packet_event : Array = connection.service()
	var event_type : ENetConnection.EventType = packet_event[0]
	
	while event_type != ENetConnection.EVENT_NONE:
		var peer : ENetPacketPeer = packet_event[1]
		
		match event_type:
			ENetConnection.EVENT_ERROR:
				push_warning("NetworkHandler: Error occured")
			
			ENetConnection.EVENT_CONNECT:
				if is_server:
					_peer_connected(peer)
				else:
					_connected_to_server()
				
			ENetConnection.EVENT_DISCONNECT:
				if is_server:
					_peer_disconnected(peer)
				else:
					_disconnected_from_server()
					return
				
			ENetConnection.EVENT_RECEIVE:
				if is_server:
					on_server_packet.emit(peer.get_meta("id"), peer.get_packet())
				else:
					on_client_packet.emit(peer.get_packet())
				
			
		packet_event = connection.service()
		event_type = packet_event[0]
		
	


# Peter, I really hope you will se this message. This is Dani from 2025.11.12 20:51, 
# MÁV Nyírség IC. I despise you. YOU'VE MADE ME DO THIS!!!.

#region Server Functions
func start_server(ip_address : String = "127.0.0.1", port : int = 42069) -> void:
	connection = ENetConnection.new()
	var error : Error = connection.create_host_bound(ip_address, port)
	
	if error:
		print("Server failed to start:", error_string(error))
		connection = null
		return
	
	is_server = true
	print("Server started")
	

func _peer_connected(peer : ENetPacketPeer) -> void:
	var peer_id : int = available_peer_ids.pop_back()
	peer.set_meta("id", peer_id)
	client_peers[peer_id] = peer
	
	print("Peer connect with id :", peer_id)
	on_peer_connected.emit(peer_id)
	

func _peer_disconnected(peer : ENetPacketPeer) -> void:
	var peer_id : int = peer.get_meta("id")
	available_peer_ids.push_back(peer_id)
	client_peers.erase(peer_id)
	
	print("Client ", peer_id, " disconnected")
	on_peer_disconnected.emit(peer_id)
	

#endregion

#region Client Functions
func start_client(ip_address : String = "127.0.0.1", port : int = 42069) -> void:
	connection = ENetConnection.new()
	var error : Error = connection.create_host(1)
	
	if error:
		print("Client failed to connect:", error_string(error))
		connection = null
		return
	
	server_peer = connection.connect_to_host(ip_address, port)
	print("Client connected")
	

func disconnect_client() -> void:
	if is_server: return
	
	server_peer.peer_disconnect()
	

func _disconnected_from_server() -> void:
	print("Disconnected from server")
	on_disconnected_from_server.emit()
	connection = null
	

func _connected_to_server() -> void:
	print("Connected to server")
	on_connected_to_server.emit()
	

#endregion
