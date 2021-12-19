using System;
using Godot;

public class Weapon : StaticBody
{
    [Export] public WEAPON Type;
    [Export] public string Name;
    [Export] public string Description;
    [Export] public float Damage;
    [Export] public float AttackSpeed;

    [Export] public Vector3 EquippedScale;
    [Export] public Vector3 EquippedRotation;

    [Export] public NodePath pCollision;
    private CollisionShape collision;
    [Export] public NodePath pPickupArea;
    private Area pickupArea;

    private bool playerInRange = false;

    public override void _Ready()
    {
        base._Ready();
        collision = GetNode<CollisionShape>(pCollision);
        pickupArea = GetNode<Area>(pPickupArea);

        SetMeta("type", "weapon");
    }

    public void onBodyEnter(Godot.Object body)
    {
        if (!body.HasMeta("type"))
            return;
        if (body.GetMeta("type").ToString() != "player")
            return;

        playerInRange = true;
        GUIManager.PopupContainer.Open("[E] Pickup");
    }

    public void onBodyExit(Godot.Object body)
    {
        if (!body.HasMeta("type"))
            return;
        if (body.GetMeta("type").ToString() != "player")
            return;

        playerInRange = false;
        GUIManager.PopupContainer.Close();
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if(playerInRange)
            if(@event is InputEventKey key)
                if(key.Scancode == (int)KeyList.E)
                    pickup();
    }

    private void pickup()
    {
        GameManager.Instance.oPlayer.PickupWeapon(this.Type);
        this.QueueFree();
    }

    public void DisableCollisionAndAreas()
    {
        collision.Disabled = true;
        pickupArea.Monitoring = false;
        pickupArea.Monitorable = false;
    }
}

