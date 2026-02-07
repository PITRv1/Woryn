using Godot;
using System;
using System.Collections;

public partial class Settings : Control
{
	[Export] MainUI mainUI;

	[Export] HSlider masterVolumeSlider;
	[Export] HSlider musicVolumeSlider;
	
	// [Export] Label masterVolumeLabel;
	// [Export] Label musicVolumeLabel;

    public override void _Ready()
    {
        masterVolumeSlider.Value = AudioServer.GetBusVolumeDb(0);
        musicVolumeSlider.Value = AudioServer.GetBusVolumeDb(1);
    }

	public void MasterVolumeChanged(float value)
	{
		AudioManager.SetMasterVolume(value);
        // masterVolumeLabel.Text = (masterVolumeSlider.Value / masterVolumeSlider.MaxValue * 100).ToString();
	}

	public void MusicVolumeChanged(float value)
	{
		AudioManager.SetMusicVolume(value);
        // musicVolumeLabel.Text = (musicVolumeSlider.Value / musicVolumeSlider.MaxValue * 100).ToString();
	}

	public void Back()
	{
		mainUI.ResetToMainMenu();
	}

	public void SetViewportSize(int option)
	{
		switch (option)
		{
			case 0:
				EditAndSaveViewportSize(new Vector2I(640, 480));
				break;
			case 1:
				EditAndSaveViewportSize(new Vector2I(1280, 720));
				break;
			case 2:
				EditAndSaveViewportSize(new Vector2I(1920, 1080));
				break;
			case 3:
				EditAndSaveViewportSize(new Vector2I(2560, 1440));
				break;
			case 4:
				EditAndSaveViewportSize(new Vector2I(3480, 2160));
				break;
		}
	}

	public void SetWindowMode(int option)
	{
		switch (option)
		{
			case 0:
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
				break;
			case 1:
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
				break;
			case 2:
				DisplayServer.WindowSetMode(DisplayServer.WindowMode.Maximized);
				break;
		}
	}

	private void EditAndSaveViewportSize(Vector2I size)
	{
		GetViewport().Set("size", size);
	}
}
