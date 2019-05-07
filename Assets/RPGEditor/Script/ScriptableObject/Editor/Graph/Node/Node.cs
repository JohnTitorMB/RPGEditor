using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public enum NodeType
{
    QuestStatue,
    Event,
    Action
}

[System.Serializable]
public class Node :  ScriptableObject
{
    public Rect rect;
    public Rect saveRect;

    public string title;

    public List<ConnectionPoint> inPoints;
    public List<ConnectionPoint> outPoints;

    public void Init(Vector2 position, float width, float height)
    {
        inPoints = new List<ConnectionPoint>();
        outPoints = new List<ConnectionPoint>();
        rect = new Rect(position.x, position.y, width, height);
    }

    public ConnectionPoint AddInput(string name, ConnectionPointType type)
    {
        ConnectionPoint connectionPoint;

        if (type == ConnectionPointType.In)
        {
            connectionPoint = new ConnectionPoint(name, this, type, inPoints.Count + 2);
            inPoints.Add(connectionPoint);
        }
        else
        {
            connectionPoint = new ConnectionPoint(name, this, type, outPoints.Count + 2);
            outPoints.Add(connectionPoint);
        }

        if (inPoints.Count != outPoints.Count)
            rect.height += 50;

        return connectionPoint;
    }

    public void Drag(Vector2 delta)
    {
        rect.position += delta;

        saveRect = rect;
    }

    public void Draw(int id,GUI.WindowFunction DrawNodeWindow)
    {
        rect = GUI.Window(id, rect, DrawNodeWindow,title);
    }

    public virtual void DrawNodeWindow(int id)
    {
        for (int i = 0; i < inPoints.Count; i++)
            inPoints[i].Draw();

        for (int i = 0; i < outPoints.Count; i++)
            outPoints[i].Draw();

        Event e = Event.current;
        Rect localRect = new Rect(0, 0, rect.width, rect.height);

        if (e.button == 1 && localRect.Contains(e.mousePosition))
            ProcessContextMenu();
    }

    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        DrawingGraph.OnClickRemoveNode(this);
    }

    public virtual void OnOutPointClicked(Connection connection)
    {

    }

    public virtual void OnInPointClicked(Connection connection)
    {

    }

    public virtual void Destroy()
    {

    }
}





