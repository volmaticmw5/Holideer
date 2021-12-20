using System;
using Godot;

public enum QUEST_TYPE
{
    KILL,
    TALK_TO,
    COLLECT
}

public class Quest
{
    public int Id;
    public QUEST_TYPE Type;
    public string Name;
    public string Description;
    public int NpcId;
    public string[] Dialogs;

    public Quest(int id, string name, string description, QUEST_TYPE type, int npcId, string[] dialogs)
    {
        this.Id = id;
        this.Type = type;
        this.Name = name;
        this.Description = description;
        this.NpcId = npcId;
        this.Dialogs = dialogs;
    }

    public virtual void TalkTo()
    {
        GUIManager.PopupContainer.Close();
        Dialog.ClearAll();
        Dialog.InitializeNew(Dialogs);
        Dialog.Show();
    }

    public virtual void DialogEnd()
    {

    }
}
