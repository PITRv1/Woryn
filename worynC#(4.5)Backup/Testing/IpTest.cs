using Godot;
using System;

public partial class IpTest : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var ipAdresses = IP.GetLocalAddresses();
		string ipv4Adress = "";
		for(int i = 0; i< ipAdresses.Length; i++)
		{
			if(ipAdresses[i].StartsWith("192.") || ipAdresses[i].StartsWith("172.") || ipAdresses[i].StartsWith("10."))
			{
				ipv4Adress = ipAdresses[i];
				GD.Print($"Your IP: {ipv4Adress}");
			}
			// GD.Print(ipAdresses[i]);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
