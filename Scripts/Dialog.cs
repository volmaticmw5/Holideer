using System;
using Godot;

public class Dialog : Container
{
    public static Dialog Instance;
    public bool inDialog = false;
    private Label childLabel;

    private int tempDialogPosition = 0;
    private string[] tempDialog;
    private bool lockInput = false;
    private Quest caller;

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        childLabel = Instance.GetNode<Label>("Label");
    }

    public async static void ClearAll()
    {
        Instance.lockInput = true;
        Instance.childLabel.Text = "";
        Instance.childLabel.PercentVisible = 0f;
        Instance.Visible = false;
        Instance.tempDialog = new string[] { };
        Instance.tempDialogPosition = 0;
        Instance.inDialog = false;
        await Instance.ToSignal(Instance.GetTree().CreateTimer(1f), "timeout");
        Instance.lockInput = false;
    }

    public static void InitializeNew(string[] texts, Quest quest = null)
    {
        Instance.caller = quest;
        Instance.childLabel.Text = texts[0];
        Instance.tempDialog = texts;
        Instance.tempDialogPosition++;
        GameManager.Instance.oPlayer.LockPlayer();
        Instance.inDialog = true;

        // TODO:: Pan camera to position arg
    }

    public async static void Show()
    {
        Instance.animateText();
        Instance.Visible = true;
        await Instance.ToSignal(Instance.GetTree().CreateTimer(1f), "timeout");
        Instance.inDialog = true;
    }

    private async void animateText()
    {
        while(Instance.childLabel.PercentVisible < 1f)
        {
            Instance.childLabel.PercentVisible += 0.05f;
            await ToSignal(GetTree().CreateTimer(.05f), "timeout");
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if(inDialog && !lockInput)
        {
            if (@event is InputEventKey key)
            {
                Instance.Advance();
            }

            if (@event is InputEventMouseButton btn)
            {
                Instance.Advance();
            }
        }
    }

    public void Finish()
    {
        ClearAll();
        GameManager.Instance.oPlayer.UnlockPlayer();
        caller.DialogEnd();
    }

    public async void Advance()
    {
        lockInput = true;
        if (tempDialogPosition == tempDialog.Length - 1)
            Finish();
        
        try
        {
            Instance.childLabel.PercentVisible = 0f;
            Instance.childLabel.Text = tempDialog[tempDialogPosition];
            tempDialogPosition++;
            animateText();
        }
        catch
        {
            Finish();
        }

        await ToSignal(GetTree().CreateTimer(1f), "timeout");
        lockInput = false;
    }
}
