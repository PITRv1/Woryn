using Godot;
using System;

public partial class CameraProjection : Camera3D
{
    private const float RAY_LENGTH = 1000f;

    [Export] public float MaxYawDeg = 45f;
    [Export] public float MaxPitchDeg = 30f;
    [Export] public float RotationSpeed = 5f;

    private Vector3 _baseRotation;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        _baseRotation = Rotation;
    }

    public override void _Process(double delta)
    {
        Vector2 viewportSize = GetViewport().GetVisibleRect().Size;
        Vector2 mousePos = GetViewport().GetMousePosition();

        Vector2 normalized = (mousePos / viewportSize) * 2f - Vector2.One;
        normalized.X = Mathf.Clamp(normalized.X, -1f, 1f);
        normalized.Y = Mathf.Clamp(normalized.Y, -1f, 1f);

        float targetYaw = Mathf.DegToRad(MaxYawDeg) * -normalized.X;
        float targetPitch = Mathf.DegToRad(MaxPitchDeg) * -normalized.Y;

        Vector3 targetRotation = new Vector3(targetPitch, targetYaw, 0f) + _baseRotation;
        Rotation = Rotation.Lerp(targetRotation, (float)delta * RotationSpeed);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("mouse_left")) CastRayCast("UseObject");
        if (Input.IsActionJustPressed("mouse_right")) CastRayCast("ShowMenu");
    }

    private void CastRayCast(string methodName)
    {
        var spaceState = GetWorld3D().DirectSpaceState;
        Vector2 mousePos = GetViewport().GetMousePosition();

        Vector3 origin = ProjectRayOrigin(mousePos);
        Vector3 end = origin + ProjectRayNormal(mousePos) * RAY_LENGTH;

        var query = PhysicsRayQueryParameters3D.Create(origin, end);
        query.CollideWithAreas = true;
        query.CollideWithBodies = false;

        var result = spaceState.IntersectRay(query);
        if (result.Count == 0) return;

        Area3D collider = (Area3D)result["collider"];
        if (!collider.IsInGroup("CameraInteractable")) return;

        var parent = collider.GetParent().GetParent();
        
        if(parent.HasMethod("UseObject")) parent.Call(methodName);
    }
}
