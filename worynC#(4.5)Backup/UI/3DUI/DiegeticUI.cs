using Godot;
using System;

public partial class DiegeticUI : Node3D
{
    [Export] MeshInstance3D meshInstance;
    [Export] SubViewport subViewport;
    [Export] Area3D area3D;
    [Export] CollisionShape3D collisionShape3D;

    bool mouseEntered, mouseHeld, mouseInside = false;

    Vector3 lastMousePosition3D;
    Vector2 lastMousePosition2D;
    Vector2 meshSize;

    public override void _Ready()
    {
        area3D.MouseEntered += () => mouseEntered = true;
        area3D.MouseExited += () => mouseEntered = false;
        subViewport.SetProcessInput(true);

        meshSize = (Vector2)meshInstance.Mesh.Get("size");
        collisionShape3D.Shape.Set("size", new Vector3(meshSize.X * 2f, meshSize.Y, 0.08f));
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        bool isMouseEvent = false;

        if (@event is InputEventMouse || @event is InputEventMouseButton) isMouseEvent = true;

        if (mouseEntered && (isMouseEvent || mouseHeld)) HandleMouse(@event);
        else if (!isMouseEvent) subViewport.PushInput(@event);
    }

    private void HandleMouse(InputEvent @event)
    {
        Vector2 eventPosition;

        if (@event is InputEventMouseButton mouseButton)
        {
            mouseHeld = mouseButton.Pressed;
            eventPosition = mouseButton.GlobalPosition;
        }
        else if (@event is InputEventMouseMotion mouseMotion)
        {
            eventPosition = mouseMotion.GlobalPosition;
        }
        else
        {
            return;
        }

        Vector3 mousePosition3D = FindMouse(eventPosition);
        bool mouseInside = mousePosition3D != Vector3.Zero;

        if (mouseInside)
        {
            mousePosition3D = area3D.GlobalTransform.AffineInverse() * mousePosition3D;
            lastMousePosition3D = mousePosition3D;
        }
        else
        {
            mousePosition3D = lastMousePosition3D;
        }

        Vector2 mousePosition2D = new(mousePosition3D.X, -mousePosition3D.Y);

        mousePosition2D += meshSize / 2f;
        mousePosition2D.X = mousePosition2D.X / meshSize.X * subViewport.Size.X;
        mousePosition2D.Y = mousePosition2D.Y / meshSize.Y * subViewport.Size.Y;


        if (@event is InputEventMouseButton btn)
        {
            btn.Position = mousePosition2D;
            btn.GlobalPosition = mousePosition2D;
        }
        else if (@event is InputEventMouseMotion motion)
        {
            motion.Position = mousePosition2D;
            motion.GlobalPosition = mousePosition2D;

            motion.Relative = lastMousePosition2D == Vector2.Zero ? Vector2.Zero : mousePosition2D - lastMousePosition2D;

            lastMousePosition2D = mousePosition2D;
        }

        subViewport.PushInput(@event);
    }


    private Vector3 FindMouse(Vector2 eventPosition)
    {
        const int projectionDistance = 5;

        Camera3D Camera = GetViewport().GetCamera3D();
        PhysicsDirectSpaceState3D DSS = GetWorld3D().DirectSpaceState;
        PhysicsRayQueryParameters3D rayParameters = new();

        rayParameters.From = Camera.ProjectRayOrigin(eventPosition);
        rayParameters.To = rayParameters.From + Camera.ProjectRayNormal(eventPosition) * projectionDistance;
        rayParameters.CollideWithBodies = false;
        rayParameters.CollideWithAreas = true;

        var Result = DSS.IntersectRay(rayParameters);

        if (Result.Count > 0)return (Vector3)Result["position"];
        return Vector3.Zero;
    }
}
