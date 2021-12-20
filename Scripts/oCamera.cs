using System;
using Godot;

public class oCamera : ClippedCamera
{
    private float amount;
    private bool isShaking = false;
    private int timeLeft = 0;
    private Random rand = new Random();

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (timeLeft > 0)
            timeLeft--;

        if(isShaking)
        {
            if (timeLeft <= 0)
            {
                isShaking = false;
                HOffset = 0;
                VOffset = 0;
                return;
            }

            HOffset = rand.Next(-1, 1) * amount;
            VOffset = rand.Next(-1, 1) * amount;
        }
    }

    public void ShakeCamera(float amount, int time)
    {
        this.amount = amount;
        this.timeLeft = time;
        this.isShaking = true;
    }
}

