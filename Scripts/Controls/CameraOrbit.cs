using Godot;
using System;

public class CameraOrbit : Spatial
{
    [Export] public float MouseSensitivity = 15f;
    [Export] public Vector2 MinMaxAngle = new Vector2(-20f, 75f);

    Vector2 mouseDelta = new Vector2();
    KinematicBody player;
    bool isRotating;

    public override void _Ready()
    {
        base._Ready();
        player = GetParent<KinematicBody>();
        Input.SetMouseMode(Input.MouseMode.Visible);
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseMotion e)
            mouseDelta = e.Relative;
        if(@event is InputEventMouseButton btn)
        {
            if(btn.ButtonIndex == 2 && btn.IsPressed())
            {
                if(!isRotating)
                {
                    Input.SetMouseMode(Input.MouseMode.Captured);
                    isRotating = true;
                }
            }

            if(btn.ButtonIndex == 2 && !btn.IsPressed() && isRotating)
            {
                isRotating = false;
                Input.SetMouseMode(Input.MouseMode.Visible);
            }
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if(isRotating)
        {
            Vector3 rot = new Vector3(mouseDelta.y, mouseDelta.x, 0f) * MouseSensitivity * delta;
            rot.x = Mathf.Clamp(rot.x, MinMaxAngle.x, MinMaxAngle.y);

            // Rotate the camera
            RotationDegrees = new Vector3(RotationDegrees.x + rot.x, RotationDegrees.y, RotationDegrees.z);
            // Rotate the player
            player.RotationDegrees -= new Vector3(player.RotationDegrees.x, rot.y, player.RotationDegrees.z);
            // Reset mouse delta
            mouseDelta = Vector2.Zero;
        }
        
    }
}
