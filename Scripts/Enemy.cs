using System;
using Godot;

class Enemy : KinematicBody
{
    [Export] public NodePath pAnimTree;
    [Export] public float MaxHealth = 100f;
    [Export] public Vector2 DamageRange = new Vector2(0.3f, 2.1f);
    [Export] public float MoveSpeed = 100f;
    [Export] public int AttackDelay = 100;
    [Export] public float AttackDamage = 2.5f;
    [Export] public bool Aggro = false;
    [Export] public bool CanPushback = false;
    [Export] public int PushbackChance = 20;
    [Export] public bool ShakeCameraOnAttack = true;
    [Export] public float ShakeAmount = .1f;
    [Export] public int ShakeTime = 10;
    [Export] public bool HasIdleAlt = false;
    [Export] public int IdleAltDelay = 2000;

    public float Health;

    // Prevent "double" hitting from the same hit animation
    private int lastDamagedTime = 0;
    private const int waitDmgTime = 4;
    private bool hasTarget = false;
    private bool targetable = true;
    private KinematicBody target = null;
    private Vector3 targetPos;
    private Vector3 moveTargetPos;
    private int lastAttackTick = 0;
    private int lastIdleAltTick = 0;
    private AnimationTree anim;

    public override void _Ready()
    {
        base._Ready();
        this.Health = MaxHealth;
        anim = GetNode<AnimationTree>(pAnimTree);
    }

    public void _on_HitArea_area_entered(Area area)
    {
        if (!targetable)
            return;
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

        target = GameManager.Instance.oPlayer.GetParent<KinematicBody>();
        hasTarget = true;

        // TODO:: do some kind of damage popup
        GD.Print("Get " + damage + " dmg");
    }

    public async void Die()
    {
        hasTarget = true;
        targetable = false;
        Random rnd = new Random();
        targetPos = new Vector3(Translation.x + rnd.Next(1, 1000), Translation.y, Translation.z + rnd.Next(1, 1000));
        await ToSignal(GetTree().CreateTimer(5f), "timeout");
        this.QueueFree();
        return;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (this.Health <= 0f && targetable)
            Die();

        if(targetable)
        {
            if (hasTarget)
            {
                if (Translation.DistanceTo(target.Translation) > 3.5f)
                {
                    targetPos = target.Translation;
                    moveTargetPos = targetPos - Translation;
                    LookAt(targetPos, Vector3.Up);
                    MoveAndSlide(moveTargetPos.Normalized() * delta * MoveSpeed);
                    anim.Set("parameters/Movement/blend_position", 1f);
                }
                else
                {
                    anim.Set("parameters/Movement/blend_position", 0f);
                    if (AttackDelay + lastAttackTick < GameManager.Instance.GameDeltaTime)
                        Attack();
                }
            }
            else
            {
                anim.Set("parameters/Movement/blend_position", 0f);
                if (HasIdleAlt)
                {
                    if (lastIdleAltTick + IdleAltDelay < GameManager.Instance.GameDeltaTime)
                    {
                        lastIdleAltTick = GameManager.Instance.GameDeltaTime;
                        anim.Set("parameters/IdleAlt/active", 1);
                    }
                }
            }
        }
        else
        {
            moveTargetPos = targetPos - Translation;
            LookAt(targetPos, Vector3.Up);
            MoveAndSlide(moveTargetPos.Normalized() * delta * MoveSpeed);
            anim.Set("parameters/Movement/blend_position", 1f);
        }
    }

    public void Attack()
    {
        if (!hasTarget)
            return;

        if(CanPushback)
        {
            Random rnd = new Random();
            if(rnd.Next(0,100) < PushbackChance)
                GameManager.Instance.oPlayer.GetPushedBack();
        }

        anim.Set("parameters/Attack/active", 1);
        lastAttackTick = GameManager.Instance.GameDeltaTime;
        GameManager.Instance.oPlayer.GetAttacked(calcDamage());

        if(ShakeCameraOnAttack)
            GameManager.Instance.oPlayer.oCamera.ShakeCamera(ShakeAmount, ShakeTime);
    }

    private float calcDamage()
    {
        return this.AttackDamage;
    }
}
