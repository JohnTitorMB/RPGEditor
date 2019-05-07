using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    static QuestManager instanse = null;
    public static QuestManager Instanse
    {
        get
        {
            if (instanse == null)
                instanse = GameObject.Find("QuestManager").GetComponent<QuestManager>();

            return Instanse;
        }
    }

    [SerializeField]
    QuestList questList; 

    private void Awake()
    {
        object obj = Resources.Load("RPGEditor/Resources/queststatueList.asset");
        if (obj == null)
        {
            obj = QuestStatueList.Create();
            questStatues = ((QuestStatueList)obj).itemList;


            for (int i = 0; i < questList.itemList.Count; i++)
            {
                List<Quest> subQuestQuestList = questList.itemList[i].GetQuests();
                for (int j = 0; j < subQuestQuestList.Count; j++)
                {
                    questStatues.Add(new QuestStatues(subQuestQuestList[j]));
                }
            }

        }
        else
            questStatues = ((QuestStatueList)obj).itemList;

        foreach (QuestStatues questStatue in questStatues)
        {
            RPGManager.Instanse.AddEntity(questStatue, questStatue.Name);
        }
        
    }

    private void Start()
    {
        if (questStatues.Count > 0)
        {
            if (questStatues[0].IsAvailable == false)
            {
                questStatues[0].IsAvailable = true;
                questStatues[0].IsRun = true;
            }
        }


    }

    List<QuestStatues> questStatues;
    void Update()
    {
        foreach (QuestStatues questStatue in questStatues)
        {
            questStatue.Update();
        }

        
    }
    
    public bool TriggerEnter(RPGComponent obj1, RPGComponent obj2)
    {

        if (obj1 == null || obj2 == null)
            return false;

        if (obj1.OnTriggerEnterList.Contains(obj2.gameObject))
            return true;

        return false;
    }

    public object GetObject(string str)
    {
        return null;
    }

}