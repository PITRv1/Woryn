using Godot;
using System;

public partial class MainMenu : Control
{
	[Export] MainUI mainUI;

	public void MenuSelected(string menuName)
	{
		switch (menuName)
		{
			case "credits":
				mainUI.CurrentMenu = MainUI.MENUS.CREDITS;
				break;
			case "settings":
				mainUI.CurrentMenu = MainUI.MENUS.SETTINGS;
				break;
			case "single":
				mainUI.CurrentMenu = MainUI.MENUS.SINGLEPLAYER;
				break;
			case "multi":
				mainUI.CurrentMenu = MainUI.MENUS.MULTIPLAYER;
				break;
			case "quit":
				mainUI.CurrentMenu = MainUI.MENUS.QUIT;
				break;
		}
	}
}
