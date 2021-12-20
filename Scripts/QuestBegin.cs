using System;
using System.Collections.Generic;

public class QuestBegin : Quest
{
    public QuestBegin(int id, string name, string description, QUEST_TYPE type, int npcId, string[] dialogs) : base(id, name, description, type, npcId, dialogs)
    {

    }

    public override void TalkTo()
    {
        GUIManager.PopupContainer.Close();
        Dialog.ClearAll();
        Dialog.InitializeNew(Dialogs, this);
        Dialog.Show();
    }

    public override void DialogEnd()
    {
        QuestManager.Instance.CurrentQuestId = 2;
    }
}
