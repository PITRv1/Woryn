using Godot;
using System;
using System.Net;
using System.Net.Sockets;

public partial class MainMenuController : Node3D
{
    // Reference to the AnimationTree node
    [Export] private AnimationPlayer introAnimator;
    [Export] private RichTextLabel _gameStartHint;
    [Export] private Camera3D _mainMenuCamera;

    private bool hasStarted = false;
    private bool hasSkipped = false;

    public override void _UnhandledInput(InputEvent @event)
    {
        if (hasStarted)
        {
            if (@event.IsPressed() && !hasSkipped) 
            {
                introAnimator.Seek(4.5);
                hasSkipped = true;
            }
            return;
        }

        if (@event.IsPressed())
        {
            hasStarted = true;
            introAnimator.Play("intro");
        }
    }
}
