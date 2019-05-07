using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[System.Serializable]
public enum ConnectionPointType { In, Out }

[System.Serializable]
public class ConnectionPoint
{
    public Rect textRect;

    public Rect rect;

    public ConnectionPointType type;

    public Node node = null;

    public GUIStyle style;

    public string name;

    public int ofset;

    public ConnectionPoint(string name, Node node, ConnectionPointType type, int ofset)
    {
        this.type = type;
        this.node = node;
        rect = new Rect(0, 0, 10f, 20f);
        textRect = new Rect(0, 0, 85f, 20f);
        this.ofset = ofset;

        this.name = name;

        if (this.type == ConnectionPointType.In)
        {
            style = new GUIStyle();
            style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            style.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            style.border = new RectOffset(4, 4, 12, 12);
        }

        else
        {
            style = new GUIStyle();
            style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
            style.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
            style.border = new RectOffset(4, 4, 12, 12);
        }

    }

    public void Draw()
    {
        rect.y = 50 * ofset - 25 - rect.height * 0.5f;
        textRect.y = rect.y;

        switch (type)
        {
            case ConnectionPointType.In:
            {
                rect.x = 0;
                textRect.x = rect.width + 2;
            }
            break;

            case ConnectionPointType.Out:
            {
                rect.x = node.rect.width - rect.width;
                textRect.x = node.rect.width - textRect.width - 20;
            }
            break;
        }

        GUIStyle inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        if (GUI.Button(rect, "", inPointStyle))
        {
            if(type == ConnectionPointType.In)
                DrawingGraph.OnClickInPoint(this);
            else
                DrawingGraph.OnClickOutPoint(this);
        }
        
        if(name != "")
            GUI.Box(textRect, name);
    }

    public Rect GetGlobalRect()
    {
        Rect gRect = rect;
        gRect.position += node.rect.position;
        return gRect;
    }
}
