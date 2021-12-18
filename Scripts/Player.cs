using System;
using Godot;

public enum WEAPON
{
    NONE,
    SUGAR_CANE
}

public class Player : Node
{
    public WEAPON CurrentWeapon = WEAPON.NONE;

}
