using System;
using Godot;

public class Weapon : StaticBody
{
    [Export] public string Name;
    [Export] public string Description;
    [Export] public float Damage;
    [Export] public float AttackSpeed;

    public void _on_PickupArea_area_entered()
    {
        GD.Print("Entered");
    }

    public void _on_PickupArea_body_entered()
    {
        GD.Print("Body enter");
    }
}

