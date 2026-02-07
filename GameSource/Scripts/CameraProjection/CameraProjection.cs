using Godot;
using System;

public partial class CameraProjection : Camera3D
{
    private const float RAY_LENGTH = 1000f;
    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("mouse_left"))
        {
            CastRayCast();
        }
    }

    private void CastRayCast()
    {
        PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState;
        Vector2 mousePos = GetViewport().GetMousePosition();

        Vector3 origin = ProjectRayOrigin(mousePos);
        Vector3 end = origin + ProjectRayNormal(mousePos) * RAY_LENGTH;

        PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(origin, end);
        query.CollideWithAreas = true;
        query.CollideWithBodies = false;

        Godot.Collections.Dictionary result = spaceState.IntersectRay(query);

        if (result.Count == 0) return;
        
        Area3D collider = (Area3D)result["collider"];

        if (!collider.IsInGroup("CameraInteractable")) return;

        Node3D parent = collider.GetParent<Node3D>();
        
        GD.Print($"{parent is ModifierCard3d} , {parent is PointCard3d}");
    }

}
