using System;
using Godot;

class Enemy : KinematicBody
{
    [Export] public float MaxHealth = 100f;
    [Export] public Vector2 DamageRange = new Vector2(0.3f, 2.1f);
    [Export] public float MoveSpeed = 100f;
    [Export] public bool Aggro = false;

    public float Health;

    // Prevent "double" hitting from the same hit animation
    private int lastDamagedTime = 0;
    private const int waitDmgTime = 4;

    public override void _Ready()
    {
        base._Ready();
        this.Health = MaxHealth;
    }

    public void _on_HitArea_area_entered(Area area)
    {
        if (!area.HasMeta("type") && !area.GetParent().HasMeta("type"))
            return;
        if (!GameManager.Instance.oPlayer.isAttacking)
            return;
        if (GameManager.Instance.GameDeltaTime < waitDmgTime + lastDamagedTime)
            return;

        if (area.HasMeta("type"))
        {
            if(area.GetMeta("type").ToString() == "weapon")
                GetAttacked(GameManager.Instance.oPlayer.CalcDamage());
        }
        else if(area.GetParent().HasMeta("type"))
        {
            if (area.GetParent().GetMeta("type").ToString() == "weapon")
                GetAttacked(GameManager.Instance.oPlayer.CalcDamage());
        }
    }

    public void GetAttacked(float damage)
    {
        lastDamagedTime = GameManager.Instance.GameDeltaTime;
        this.Health -= damage;

        // TODO:: do some kind of damage popup
        GD.Print("Get " + damage + " dmg");
    }

    public void Die()
    {
        // TODO:: death animation
        this.QueueFree();
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (this.Health <= 0f)
            Die();
    }
}
