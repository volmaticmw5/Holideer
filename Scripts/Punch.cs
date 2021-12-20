using System;
using Godot;

public class Punch : Area
{
    [Export] public NodePath PlayerFunc;
    private Player player;

    public override void _Ready()
    {
        base._Ready();
        player = GetNode<Player>(PlayerFunc);
        SetMeta("type", "weapon");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if(player.CurrentWeapon != null && Monitoring)
        {
            this.Monitoring = false;
            this.Monitorable = false;
        }
        if(player.CurrentWeapon == null && !Monitorable)
        {
            this.Monitoring = true;
            this.Monitorable = true;
        }
    }
}
