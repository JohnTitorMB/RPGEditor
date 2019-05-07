using System.Collections.Generic;
using UnityEditor;

[System.Serializable]
public class Quest : EntityData
{
    public string questDescription = "";

    public Quest()
    {
        subQuest = new List<Quest>();
    }

    public void Init(EventActionList eventActionList)
    {
        InitEvent(ref EventOnAvailable, eventActionList);
        InitEvent(ref EventIsAvailable, eventActionList);
        InitEvent(ref EventOnStart, eventActionList);
        InitEvent(ref EventIsRun, eventActionList);
        InitEvent(ref EventOnCompleted, eventActionList);
    }

    public void InitEvent(ref EventRPG eventRPG,EventActionList eventActionList)
    {
        eventRPG = CreateInstance<EmptyEventRPG>();
        AssetDatabase.AddObjectToAsset(eventRPG, eventActionList);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(eventRPG));
        eventActionList.eventList.Add(eventRPG);
    }

    public List<Quest> GetQuests()
    {
        List<Quest> quests = new List<Quest>();
        quests.Add(this);
        for (int i = 0; i < subQuest.Count; i++)
        {
            List<Quest> subQuestQuestList = subQuest[i].GetQuests();


            for (int j = 0; j < subQuestQuestList.Count; j++)
            {
                quests.Add(subQuestQuestList[j]);
            }
        }

        return quests;
    }

    public List<Quest> subQuest;

    public EventRPG EventOnAvailable;
    public EventRPG EventIsAvailable;
    public EventRPG EventOnStart;
    public EventRPG EventIsRun;
    public EventRPG EventOnCompleted;

}