using Godot;
using System;

public partial class Global : Node
{
    public static NetworkHandler networkHandler;
    public static MultiplayerServerGlobals multiplayerServerGlobals;
    public static MultiplayerClientGlobals multiplayerClientGlobals;
    public static TurnManager turnManagerInstance;
    public static LobbyManager lobbyManagerInstance;
    public static ShopManager shopManagerInstance;
    public static MultiplayerPlayerClass multiplayerPlayerClass;
    public static ToolTipMenu toolTipMenu;
}
