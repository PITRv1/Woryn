using Godot;
using System;

public partial class JoinableGamePlaque : Control
{
	[Export] private Label _gameName;
	[Export] private Label _ipAddress;
	[Export] private Label _playerCount;
	[Export] private Button _joinButton;
	[Signal]
	public delegate void JoinPressedEventHandler();

	public override void _Ready()
	{
		_joinButton.Pressed += () => EmitSignal(SignalName.JoinPressed);
	}
	public void SetMenu(string gameName, string ipAddress, int numberOfPlayers, int maxPlayers)
	{
		_gameName.Text = gameName;
		_ipAddress.Text = ipAddress;
		_playerCount.Text = $"Players: {numberOfPlayers}/{maxPlayers}";
	}
}
