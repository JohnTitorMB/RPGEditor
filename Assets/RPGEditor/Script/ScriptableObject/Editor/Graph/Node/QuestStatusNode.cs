using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class QuestStatusNode : Node
{
    public Quest quest = null;

    public void InitQuestStatusNode(Vector2 position, float width, float height, Quest _quest)
    {
        Init(position, width, height);
        AddInput("OnAvailable", ConnectionPointType.Out);
        AddInput("IsAvailable", ConnectionPointType.Out);
        AddInput("OnStart", ConnectionPointType.Out);
        AddInput("IsRun", ConnectionPointType.Out);
        AddInput("OnCompleted", ConnectionPointType.Out);
        title = "Quest Status";
        quest = _quest;
    }

    public static Node Create(Vector2 position, float width, float height, Quest quest)
    {
        QuestStatusNode node = CreateInstance<QuestStatusNode>();
        node.InitQuestStatusNode(position, width, height, quest);
        return node;
    }

    public override void OnOutPointClicked(Connection connection)
    {
        Node inNode = connection.inPoint.node;

        if (inNode != null)
        {
            if (inNode.GetType() == typeof(EventNode))
            {
                EventNode eventNode = (EventNode)inNode;

                if (quest != null)
                {
                    string name = connection.outPoint.name;

                    if (name == "OnAvailable")
                        quest.EventOnAvailable.nextEvents = eventNode.eventRPG;
                    else if (name == "IsAvailable")
                        quest.EventIsAvailable.nextEvents = eventNode.eventRPG;
                    else if (name == "OnStart")
                        quest.EventOnStart.nextEvents = eventNode.eventRPG;
                    else if (name == "IsRun")
                        quest.EventIsRun.nextEvents = eventNode.eventRPG;
                    else if (name == "OnCompleted")
                        quest.EventOnCompleted.nextEvents = eventNode.eventRPG;
                }
            }

            if (inNode.GetType() == typeof(ActionNode))
            {
                ActionNode actionNode = (ActionNode)inNode;

                if (quest != null)
                {
                    string name = connection.outPoint.name;

                    if (name == "OnAvailable")
                        quest.EventOnAvailable.action = actionNode.action;
                    else if (name == "IsAvailable")
                        quest.EventIsAvailable.action = actionNode.action;
                    else if (name == "OnStart")
                        quest.EventOnStart.action = actionNode.action;
                    else if (name == "IsRun")
                        quest.EventIsRun.action = actionNode.action;
                    else if (name == "OnCompleted")
                        quest.EventOnCompleted.action = actionNode.action;
                }
            }
        }
    }

    public override void DrawNodeWindow(int id)
    {
        base.DrawNodeWindow(id);
    }
}

