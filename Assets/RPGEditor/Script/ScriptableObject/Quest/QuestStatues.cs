using UnityEngine;

[System.Serializable]
public class QuestStatues
{
    public QuestStatues(Quest _quest)
    {
        quest = _quest;
    }
    Quest quest = null;

    bool isCompleted;
    bool isAvailable;
    bool isRun;

    public string Name
    {
        get
        {
            if (quest != null)
                return quest.name;
            return null;
        }
    }


    public bool IsAvailable
    {
        get => isAvailable;
        set
        {
            isAvailable = value;
            if (isAvailable && quest != null)
            {
                Debug.Log(quest.name + " : " + "OnAvailable");
                RPGDialogueManager.Instanse.DebugLog(quest.name + " : " + " Disponnible");
                quest.EventOnAvailable.Update();
            }
        }
    }

    public bool IsCompleted
    {
        get => isCompleted;
        set
        {
            isCompleted = value;
            if (isCompleted && quest != null)
            {
                RPGDialogueManager.Instanse.DebugLog(quest.name + " : " + " Terminer");
                quest.EventOnCompleted.Update();
            }
        }
    }

    public bool IsRun
    {
        get => isRun;
        set
        {
            isRun = value;
            if (isRun && quest != null)
            {
                RPGDialogueManager.Instanse.DebugLog(quest.name + " : " + " Commencer");
                quest.EventOnStart.Update();
            }
        }
    }

    public void Update()
    {
        if (isAvailable && !isRun && quest != null)
        {
            //   Debug.Log(quest.questName + " : " + "IsAvailable");
            quest.EventIsAvailable.Update();
        }

        if (isRun && quest != null)
        {
            //  Debug.Log(quest.questName + " : " + "Run");
            quest.EventIsRun.Update();
        }

    }
}
