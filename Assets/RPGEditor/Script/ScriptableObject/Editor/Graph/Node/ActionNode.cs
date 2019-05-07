using System;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class ActionNode : Node
{
    public MyAction action = null;
    public void InitActionNode(Vector2 position, float width, float height, ActionEnum actionEnum)
    {
        Init(position, width, height);
        AddInput("", ConnectionPointType.In);
        AddInput("Next Action", ConnectionPointType.Out);

        EventActionList eventActionList = RPGEditor.GetEventActionList();

        if (eventActionList != null)
        {
            action = CreateInstance<MyAction>();
            AssetDatabase.AddObjectToAsset(action, eventActionList);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(action));
            action.Init(actionEnum);
            eventActionList.actionList.Add(action);
        }

        if (action.ActionEnum == ActionEnum.CALLFUNCTION)
            title = "Call Function";
        else if (action.ActionEnum == ActionEnum.SETTER)
            title = "Setter";

        saveRect = rect;
    }

    public static Node Create(Vector2 position, float width, float height, ActionEnum actionEnum)
    {
        ActionNode node = CreateInstance<ActionNode>();
        node.InitActionNode(position, width, height, actionEnum);
        return node;
    }

    public override void OnInPointClicked(Connection connection)
    {
        Node outNode = connection.outPoint.node;

        if (outNode != null)
        {
            if (outNode.GetType() == typeof(ActionNode))
            {
                ActionNode actionNode = (ActionNode)outNode;

                if (actionNode.action != null)
                {
                    actionNode.action.nextAction = action;
                }
            }

            else if (outNode.GetType() == typeof(EventNode))
            {
                EventNode eventNode = (EventNode)outNode;

                if (eventNode.eventRPG != null)
                {
                    eventNode.eventRPG.action = action;
                }
            }

            else if (outNode.GetType() == typeof(QuestStatusNode))
            {
                QuestStatusNode QuestStatusNode = (QuestStatusNode)outNode;
                Quest quest = QuestStatusNode.quest;
                if (quest != null)
                {
                    string name = connection.outPoint.name;

                    if (name == "OnAvailable")
                        quest.EventOnAvailable.action = action;
                    else if (name == "IsAvailable")
                        quest.EventIsAvailable.action = action;
                    else if (name == "OnStart")
                        quest.EventOnStart.action = action;
                    else if (name == "IsRun")
                        quest.EventIsRun.action = action;
                    else if (name == "OnCompleted")
                        quest.EventOnCompleted.action = action;
                }
            }
        }
    }

    public override void OnOutPointClicked(Connection connection)
    {
        Node inNode = connection.inPoint.node;

        if (inNode != null)
        {
            if (inNode.GetType() == typeof(ActionNode))
            {
                ActionNode eventNode = (ActionNode)inNode;
                action.nextAction = eventNode.action;
            }
        }
    }

    public override void DrawNodeWindow(int id)
    {
        base.DrawNodeWindow(id);

        if (action != null)
        {
            Parametre parametreObect = action.parametres[0];
            Type ObjectType = UtilityNode.DrawObjectList(parametreObect, this);

            if (action.ActionEnum == ActionEnum.CALLFUNCTION)
                UtilityNode.DrawCallFunction(ObjectType, ref action.parametres, 40, this, typeof(void));
            else if (action.ActionEnum == ActionEnum.SETTER)
                UtilityNode.DrawSetter(ObjectType, ref action.parametres, this);
        }
    }

    public override void Destroy()
    {
        if (action)
        {
            DrawingGraph.DestroyAction(action);
        }
    }

}