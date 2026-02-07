using Godot;
using System;

public partial class AudioManager : AudioStreamPlayer
{
	public static void SetMasterVolume(float volume)
	{
		AudioServer.SetBusVolumeDb(0, volume);
	}

	public static void SetMusicVolume(float volume)
	{
		AudioServer.SetBusVolumeDb(1, volume);
	}
}
