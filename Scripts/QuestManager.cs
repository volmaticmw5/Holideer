using System;
using System.Collections.Generic;
using Godot;

public class QuestManager
{
    public static QuestManager Instance;
    public int CurrentQuestId;
    private int LastQuestId;
    public Dictionary<int, Quest> Quests;
    public Dictionary<int, NPC> Npcs;

    public QuestManager(int current = -1)
    {
        Instance = this;
        Npcs = new Dictionary<int, NPC>();
        Quests = new Dictionary<int, Quest>()
        {
            {
                1, 
                new QuestBegin(
                    1, 
                    "Trapped", 
                    "Your little helpers are trapped inside the house! Go see what's going on.",
                    QUEST_TYPE.TALK_TO,
                    1,
                    new string[]{
                        "Ah there you are! That must've been some good wine eheheh..",
                        "Anyways, I don't know if you noticed but we're kind of trapped in here!",
                        "At this rate, there's no way we'll be able to pack all the presents in time for christmas.",
                        "Something happened to the reindeer, they kept following us and giving us a mean look!",
                        "We're too scared to venture outside... Will you please scare them off?",
                        "I hope you come back!"
                    }
                )
            },
            {
                2,
                new QuestSantaHouse(
                    2,
                    "Fend off the reindeer",
                    "Scare the reindeer away so your little helpers can get to the factory!",
                    QUEST_TYPE.KILL,
                    -1,
                    new string[]{
                        
                    }
                )
            },
        };

        if(current != -1)
            CurrentQuestId = current;

        HighlightCurrentQuestNpc();
    }

    public static void HighlightCurrentQuestNpc()
    {
        if (Instance == null)
            return;

        if (Instance.LastQuestId != Instance.CurrentQuestId)
        {
            foreach (KeyValuePair<int, NPC> npc in Instance.Npcs)
            {
                npc.Value.Interactable = false;
                npc.Value.Highlighted = false;
            }

            if (!Instance.Quests.ContainsKey(Instance.CurrentQuestId))
                return;
            if (!Instance.Npcs.ContainsKey(Instance.Quests[Instance.CurrentQuestId].NpcId))
                return;

            if (Instance.CurrentQuestId > 0)
            {
                Instance.Npcs[Instance.Quests[Instance.CurrentQuestId].NpcId].Highlighted = true;
                Instance.Npcs[Instance.Quests[Instance.CurrentQuestId].NpcId].Interactable = true;
            }

            Instance.LastQuestId = Instance.CurrentQuestId;
        }
    }

    public void ResetNpcs()
    {
        Npcs.Clear();
        LastQuestId = -1;
    }

    public void RegisterNpc(NPC npc)
    {
        Npcs.Add(npc.Id, npc);
    }

    public static void TalkTo(int fromNpc)
    {
        foreach(KeyValuePair<int, Quest> quest in Instance.Quests)
        {
            if(quest.Value.NpcId == fromNpc)
            {
                if(Instance.CurrentQuestId == quest.Value.NpcId)
                {
                    Instance.Quests[Instance.CurrentQuestId].TalkTo();

                    break;
                }
            }
        }
    }
}
