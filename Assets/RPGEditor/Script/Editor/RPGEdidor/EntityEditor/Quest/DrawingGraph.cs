using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DrawingGraph
{
    public static GUIStyle inPointStyle;
    public static GUIStyle outPointStyle;

    static ConnectionPoint selectedInPoint;
    static ConnectionPoint selectedOutPoint;

    static Vector2 offset;
    static Vector2 drag;

    static Rect position;

    public static List<Node> nodeList;

    static Graph graph;

    static GraphList graphList;

    static Quest quest;

    public static bool DrawQuestGraphNodes(GraphList _graphList, int graphIndex,Rect WindowPosition,Quest _quest)
    {
        graphList = _graphList;

        if (graphList == null)
            return false;

        position = WindowPosition;


        graph = graphList.questGraphList[graphIndex];

        quest = _quest;



        DrawGrid();

       graph.Draw();
        
        DrawConnectionLine(Event.current);

        ProcessEvents(Event.current);
        
        return true;
    }

    static void DrawGrid()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);
    }

    static void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    static void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    ClearConnectionSelection();
                }

                if (e.button == 1)
                {
                    ProcessContextQuestMenu(e.mousePosition);
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnDrag(e.delta);
                }
                break;
        }
    }

    static void DrawConnectionLine(Event e)
    {
        if (selectedInPoint != null && selectedOutPoint == null)
        {

            Handles.DrawBezier(
                selectedInPoint.GetGlobalRect().center,
                e.mousePosition,
                selectedInPoint.GetGlobalRect().center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null)
        {



            Handles.DrawBezier(
                selectedOutPoint.GetGlobalRect().center,
                e.mousePosition,
                selectedOutPoint.GetGlobalRect().center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }

    static void ProcessContextMenu(Vector2 mousePosition)
    {
       /* GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(graph,mousePosition));
        genericMenu.ShowAsContext();*/
    }

    static void ProcessContextQuestMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();

        genericMenu.AddItem(new GUIContent("QuestStatus"), false, () => OnClickAddNode(graph,QuestStatusNode.Create(mousePosition, 200, 100, quest)));
        genericMenu.ShowAsContext();

        genericMenu.AddItem(new GUIContent("CallFunction"), false, () => OnClickAddNode(graph, ActionNode.Create(mousePosition, 200, 100, ActionEnum.CALLFUNCTION)));
        genericMenu.ShowAsContext();

        genericMenu.AddItem(new GUIContent("Setter"), false, () => OnClickAddNode(graph, ActionNode.Create(mousePosition, 200, 100,ActionEnum.SETTER)));
        genericMenu.ShowAsContext();

        genericMenu.AddItem(new GUIContent("Event"), false, () => OnClickAddNode(graph, EventNode.Create(mousePosition, 200, 100)));
        genericMenu.ShowAsContext();
    }

    static void OnClickAddNode(Graph graph, Node node)
    {
        AssetDatabase.AddObjectToAsset(node, graphList);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(node));

        graph.AddNode(node);
        GUI.changed = true;
    }

    static void OnDrag(Vector2 delta)
    {
        drag = delta;

     
            for (int i = 0; i < graph.nodes.Count; i++)
            {
                graph.nodes[i].Drag(delta);
            }
        
        
        GUI.changed = true;
    }

    public static void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                Connection conection = CreateConnection();
                inPoint.node.OnInPointClicked(conection);
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    public static void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null)
        {

            if (selectedOutPoint.node != selectedInPoint.node)
            {
                Connection conection = CreateConnection();
                outPoint.node.OnOutPointClicked(conection);
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    public static void OnClickRemoveNode(Node node)
    {
        if (graph.connections != null)
        {
            Debug.Log("zere");
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < graph.connections.Count; i++)
            {
                Connection connection = graph.connections[i];

                if(graph.connections[i].inPoint.node == node)
                {
                    connectionsToRemove.Add(graph.connections[i]);
                }

                if (graph.connections[i].outPoint.node == node)
                {
                    connectionsToRemove.Add(graph.connections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
                Debug.Log("ezerezrez");
                graph.connections.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }

        DestroyNode(node);
    }

    static void OnClickRemoveConnection(Connection connection)
    {
        graph.connections.Remove(connection);
    }

    static Connection CreateConnection()
    {
        if (graph.connections == null)
        {
            graph.connections = new List<Connection>();
        }


        Connection connection = new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection);

        graph.connections.Add(connection);

        return connection;
    }

    static void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }

    public static void DestroyNode(ScriptableObject scriptableObject)
    {
        Node node = (Node)scriptableObject;
        node.Destroy();

        graph.nodes.Remove(node);
        string path = AssetDatabase.GetAssetPath(node);
        AssetDatabase.RemoveObjectFromAsset(scriptableObject);
        AssetDatabase.ImportAsset(path);
    }

    public static void DestroyAction(ScriptableObject scriptableObject)
    {
        EventActionList eventActionList = RPGEditor.GetEventActionList();
        eventActionList.actionList.Remove((MyAction)scriptableObject);
        string path = AssetDatabase.GetAssetPath(scriptableObject);
        AssetDatabase.RemoveObjectFromAsset(scriptableObject);
        AssetDatabase.ImportAsset(path);
    }

    public static void DestroyEvent(ScriptableObject scriptableObject)
    {
        EventActionList eventActionList = RPGEditor.GetEventActionList();
        eventActionList.eventList.Remove((EventRPG)scriptableObject);
        string path = AssetDatabase.GetAssetPath(scriptableObject);
        AssetDatabase.RemoveObjectFromAsset(scriptableObject);
        AssetDatabase.ImportAsset(path);
    }
}

public class CreateGraphList
{
    [MenuItem("Assets/Create/Inventory Graph List")]
    public static GraphList Create()
    {
        GraphList asset = ScriptableObject.CreateInstance<GraphList>();

        AssetDatabase.CreateAsset(asset, "Assets/RPGEditor/Resources/graphList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
