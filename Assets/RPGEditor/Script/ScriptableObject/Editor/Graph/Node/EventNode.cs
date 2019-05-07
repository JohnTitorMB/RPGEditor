using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class EventNode : Node
{
    public EventRPG eventRPG = null;

    public void InitEventNode(Vector2 position, float width, float height)
    {
        Init(position, width, height);
        AddInput("", ConnectionPointType.In);
        AddInput("Action", ConnectionPointType.Out);
        AddInput("Next Event", ConnectionPointType.Out);

        EventActionList eventActionList = RPGEditor.GetEventActionList();

        if (eventActionList != null)
        {
            eventRPG = CreateInstance<EventRPG>();
            AssetDatabase.AddObjectToAsset(eventRPG, eventActionList);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(eventRPG));
            eventRPG.Init();
            eventActionList.eventList.Add(eventRPG);
        }
        title = "Event";

        saveRect = rect;
    }

    public static Node Create(Vector2 position, float width, float height)
    {
        EventNode node = CreateInstance<EventNode>();
        node.InitEventNode(position, width, height);
        return node;
    }

    public override void OnInPointClicked(Connection connection)
    {
        Node outNode = connection.outPoint.node;

        if (outNode != null)
        {
            if (outNode.GetType() == typeof(QuestStatusNode))
            {
                QuestStatusNode QuestStatusNode = (QuestStatusNode)outNode;
                Quest quest = QuestStatusNode.quest;
                if (quest != null)
                {
                    string name = connection.outPoint.name;

                    if (name == "OnAvailable")
                        quest.EventOnAvailable.nextEvents = eventRPG;
                    else if (name == "IsAvailable")
                        quest.EventIsAvailable.nextEvents = eventRPG;
                    else if (name == "OnStart")
                        quest.EventOnStart.nextEvents = eventRPG;
                    else if (name == "IsRun")
                        quest.EventIsRun.nextEvents = eventRPG;
                    else if (name == "OnCompleted")
                        quest.EventOnCompleted.nextEvents = eventRPG;
                }
            }

            if (outNode.GetType() == typeof(EventNode))
            {
                EventNode eventNode = (EventNode)outNode;

                if (eventNode.eventRPG != null)
                {
                    if (connection.outPoint.name == "Next Event")
                        eventNode.eventRPG.nextEvents = eventRPG;
                }
            }
        }
    }

    public override void OnOutPointClicked(Connection connection)
    {
        Node inNode = connection.inPoint.node;

        if (inNode != null)
        {
            string name = connection.outPoint.name;

            if (name == "Action")
            {
                if (inNode.GetType() == typeof(ActionNode))
                {
                    ActionNode actionNode = (ActionNode)inNode;
                    eventRPG.action = actionNode.action;
                }
            }

            else if (name == "Next Event")
            {
                if (inNode.GetType() == typeof(EventNode))
                {
                    EventNode eventNode = (EventNode)inNode;
                    eventRPG.nextEvents = eventNode.eventRPG;
                }
            }
        }
    }

    public override void DrawNodeWindow(int id)
    {
        base.DrawNodeWindow(id);
        if (eventRPG != null)
        {
            Parametre parametreObect = eventRPG.parametres[0];
            Type ObjectType = UtilityNode.DrawObjectList(parametreObect, this);

            UtilityNode.DrawCallFunction(ObjectType, ref eventRPG.parametres, 80, this, typeof(bool));
        }
    }

    public override void Destroy()
    {
        if (eventRPG)
        {
            DrawingGraph.DestroyEvent(eventRPG);
        }
    }

}