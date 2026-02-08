using Godot;
using System;

public partial class Multiplayer : Control
{
	[Export] MainUI mainUI;
	[Export] Control multiTypeMenu;
	[Export] Control multiHostMenu;
	[Export] Control multiJoinMenu;
	[Export] Control playerListMenu;
	[Export] LineEdit gameNameText;
	[Export] HSlider numberOfPlayersValue;
    [Export] ButtonGroup optionGroup;
	[Export] LineEdit ipAddressInputServer;
	[Export] LineEdit ipAddressInputClient;


	private Control _currentMenu;
	public Control CurrentMenu {
		get
		{
			return _currentMenu;
		}
		set {
			if (_currentMenu != null) _currentMenu.Visible = false;
			_currentMenu = value;
			_currentMenu.Visible = true;
		}
	}


    public override void _Ready()
    {
		string[] ipAdresses = IP.GetLocalAddresses();

		string ipAddressOnLocalNetwork = ipAdresses[ipAdresses.Length-1];

        CurrentMenu = multiTypeMenu;

		ipAddressInputClient.Text = ipAddressOnLocalNetwork;
		// ipAddressInputServer.Text = ipAddressOnLocalNetwork;
		ipAddressInputServer.Text = "0.0.0.0";
    }

	public void ChangeMenu(string option)
	{
		switch(option)
		{
			case "host":
				CurrentMenu = multiHostMenu;
				break;
			case "join":
				CurrentMenu = multiJoinMenu;
				break;
			case "select":
				CurrentMenu = multiTypeMenu;
				break;
			case "main":
				mainUI.CurrentMenu = MainUI.MENUS.MAIN;
				
				CurrentMenu = multiTypeMenu;
				break;
			case "player":
				CurrentMenu = playerListMenu;
				break;
			case "select&stop":
				ChangeMenu("select");
				Global.networkHandler.DisconnectClient();
				if (Global.networkHandler._isServer) Global.networkHandler.StopServer();
				break;
		}
	}

	public void HostGame()
	{
		Global.networkHandler.StartServer(ipAddressInputServer.Text);
        Global.networkHandler.StartClient("localhost");

		ChangeMenu("player");
	}

	public void JoinGame()
	{
        Global.networkHandler.StartClient(ipAddressInputClient.Text);
		ChangeMenu("player");
	}
	
}
