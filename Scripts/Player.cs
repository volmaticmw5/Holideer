using System;
using Godot;

public enum WEAPON
{
    NONE,
    SUGAR_CANE
}

public class Player : Node
{
    [Export] public NodePath KinematicBd;
    private PlayerControls playerControls;
    [Export] public NodePath pCamera;
    public oCamera oCamera;
    [Export] public NodePath pWeaponAttatch;
    private BoneAttachment weaponAttatch;

    public float Health;
    public Weapon CurrentWeapon;
    public bool isAttacking;

    public override void _Ready()
    {
        base._Ready();

        Health = 100f;

        if (GameManager.Instance.oPlayer == null)
            GameManager.Instance.oPlayer = this;

        weaponAttatch = GetNode<BoneAttachment>(pWeaponAttatch);
        playerControls = GetNode<PlayerControls>(KinematicBd);
        oCamera = GetNode<oCamera>(pCamera);
    }

    public void PickupWeapon(WEAPON weapon)
    {
        // TODO:: if we already have a weapon, drop it near the player

        // Equip this new weapon
        PackedScene weaponRes = (PackedScene)ResourceLoader.Load("res://Prefabs/Weapons/" + weapon.ToString().ToLower() + ".tscn");
        if(weaponRes == null)
        {
            GD.PrintErr("Failed to load weapon resource (" + weapon.ToString() + ")");
            return;
        }

        Weapon weaponObj = (Weapon)weaponRes.Instance();
        if(weaponObj == null)
        {
            GD.PrintErr("Failed to instantiate weapon object (" + weapon.ToString() + ")");
            return;
        }

        weaponAttatch.AddChild(weaponObj);
        weaponObj.DisableCollisionAndAreas();
        weaponObj.Scale = weaponObj.EquippedScale;
        weaponObj.RotationDegrees = weaponObj.EquippedRotation;

        CurrentWeapon = weaponObj;
    }

    public float CalcDamage()
    {
        float dmg = 1f;

        if (CurrentWeapon != null)
            dmg += CurrentWeapon.Damage;

        return dmg;
    }

    public float GetAttackSpeed()
    {
        float speed = 1f;
        speed += CurrentWeapon == null ? 0f : CurrentWeapon.AttackSpeed;

        return speed;
    }

    public void GetAttacked(float damage)
    {
        this.Health -= damage;
        // TODO:: effects
    }

    public void GetPushedBack()
    {
        playerControls.GetPushedBack();
    }

    public void LockPlayer()
    {
        playerControls.canMove = false;
    }

    public void UnlockPlayer()
    {
        playerControls.canMove = true;
    }
}
