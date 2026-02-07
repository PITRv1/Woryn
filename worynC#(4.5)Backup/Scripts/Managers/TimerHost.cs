using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class TimerHost : Node
{
	public static TimerHost Instance;

    public override void _Ready()
    {
        Instance = this;
    }
}
