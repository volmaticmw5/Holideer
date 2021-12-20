using System;
using Godot;

public class NPC : KinematicBody
{
    [Export] public int Id;

    public bool Interactable;
    public bool Highlighted = false;
    private bool hasPlayer;
    private bool lockInput = false;
    public override void _Ready()
    {
        base._Ready();
        QuestManager.Instance.RegisterNpc(this);
    }

    public void _on_InteractArea_body_entered(Godot.Object body)
    {
        if (!Interactable)
            return;
        if (!body.HasMeta("type"))
            return;
        if (body.GetMeta("type").ToString() != "player")
            return;

        hasPlayer = true;
        GUIManager.PopupContainer.Open("[E] Talk");
    }

    public void _on_InteractArea_body_exited(Godot.Object body)
    {
        if (!Interactable)
            return;

        hasPlayer = false;
        GUIManager.PopupContainer.Close();
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (hasPlayer && !GameManager.Instance.Dialog.inDialog && !lockInput)
            if (@event is InputEventKey key)
                if (key.Scancode == (int)KeyList.E)
                {
                    QuestManager.TalkTo(1);
                    _lockInput();
                }
    }

    private async void _lockInput()
    {
        lockInput = true;
        await ToSignal(GetTree().CreateTimer(1f), "timeout");
        lockInput = false;
    }
}
