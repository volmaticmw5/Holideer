using Godot;
using System;

public class PlayerControls : KinematicBody
{
    private Spatial meshTransform;
    [Export] public float MovementSpeed = 5f;
    [Export] public float MovementSpeedInAir = 2f;
    [Export] public Vector3 Gravity = new Vector3(0, -9.3f, 0f);
    [Export] public float JumpForce = 10f;
    [Export] public int JumpTime = 1000;

    // Animation
    [Export] public NodePath AnimationTree;

    private Vector3 velocity = Vector3.Zero;
    private bool wasFallingPreviousTick = false;
    private int jumpTimeLeft;
    private bool isJumping = false;
    private AnimationTree anim;

    public override void _Ready()
    {
        base._Ready();
        anim = GetNode<AnimationTree>(AnimationTree);
    }

    public override void _Input(InputEvent @event)
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        // Update wasfalling if we're falling
        if (!IsOnFloor() && !wasFallingPreviousTick)
            wasFallingPreviousTick = true;
        
        Vector2 motionTarget = new Vector2(
            Input.GetActionStrength("move_left") - Input.GetActionStrength("move_right"),
            Input.GetActionStrength("move_forward") - Input.GetActionStrength("move_back")
        ).Normalized();

        anim.Set("parameters/Move/blend_position", new Vector2(motionTarget.x, motionTarget.y));
        anim.Set("parameters/GroundState/current", IsOnFloor() ? 1 : 0);

        // Apply direction
        Vector3 direction;
        if (isJumping)
            direction = (Transform.basis.z * motionTarget.y + Transform.basis.x * motionTarget.x) * MovementSpeedInAir;
        else
            direction = (Transform.basis.z * motionTarget.y + Transform.basis.x * motionTarget.x) * MovementSpeed;
        velocity += direction;

        // Jump
        if (Input.IsKeyPressed((int)KeyList.Space) && IsOnFloor())
        {
            if (!isJumping)
            {
                jumpTimeLeft = JumpTime;
                isJumping = true;
                anim.Set("parameters/Jump/active", true);
            }
        }

        if (isJumping)
        {
            // This check has to come before the velocity applying one, else we'll always be reset isJumping to false
            if (IsOnFloor() && jumpTimeLeft != JumpTime)
            {
                isJumping = false;
                jumpTimeLeft = 0;
            }

            if (jumpTimeLeft > 0)
            {
                velocity.y += JumpForce;
                jumpTimeLeft--;
            }
        }

        // Apply gravity
        velocity += Gravity;

        // Update wasfalling if not falling anymore
        if (wasFallingPreviousTick && IsOnFloor())
            wasFallingPreviousTick = false;

        // Apply delta. HAS TO BE LAST
        velocity *= delta;

        velocity = MoveAndSlide(velocity, new Vector3(0, 1, 0), false, 3);
        velocity = Vector3.Zero;
    }
}
