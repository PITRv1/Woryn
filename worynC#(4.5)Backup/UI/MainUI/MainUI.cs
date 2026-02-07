using Godot;
using System;

public partial class MainUI : Control
{
	[Signal] public delegate void MenuChangedEventHandler(int newMenu);

	public enum MENUS
	{
		MAIN,
		SETTINGS,
		QUIT,
		SINGLEPLAYER,
		CREDITS,
		MULTIPLAYER
	}

	private MENUS _currentMenu;

	public MENUS CurrentMenu {
		get
		{
			return _currentMenu;
		}
		set {
			_currentMenu = value;
			Visible = false;
			EmitSignal(SignalName.MenuChanged, (int)_currentMenu);
		}
	}

	public void ResetToMainMenu()
	{
		CurrentMenu = MENUS.MAIN;
		Visible = true;
	}
}
