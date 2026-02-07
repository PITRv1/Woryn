using Godot;
using GodotPlugins.Game;
using System;

public partial class CameraController : Node
{
    [Export] private Camera3D animatedCamera {set;get;}
    [Export] private MainUI mainMenuUi {set;get;}

    [Export] private Godot.Collections.Array<Marker3D> focusPoints {set;get;}

    private Tween _tween;

    public override void _Ready()
    {
        mainMenuUi.MenuChanged += _OnMenuChanged;
    }
    
    /// <summary>
    /// Tweens camera to one of its focus points;
    /// To create a focus point make a marker3D in Godot, set its position and rotation and then link it to this node's focus point list.
    /// </summary>
    /// <param name="focusPointIndex"></param>
    public void FocusOnPoint(int focusPointIndex)
    {
        Marker3D focusPoint = focusPoints[focusPointIndex];        
        ResetTween();

        _tween.SetParallel(true);
        _tween.SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Quad);

        _tween.TweenProperty(animatedCamera,"global_position", focusPoint.GlobalPosition, 1.0f);
        _tween.TweenProperty(animatedCamera,"global_rotation", focusPoint.GlobalRotation, 1.0f);
    }

    /// <summary>
    /// Connects to a signal from the main menu fired when the menu changes.
    /// The signal is loaded with the new menu.
    /// To add a new case (linked focus point) follow the patter defined below.
    /// </summary>
    /// <param name="newMenu"></param>
    private void _OnMenuChanged(int newMenuID)
    {
        FocusOnPoint(newMenuID);
    }

    private void ResetTween()
    {
        if (_tween != null)
            _tween.Kill();
        _tween = CreateTween();
    }

}
