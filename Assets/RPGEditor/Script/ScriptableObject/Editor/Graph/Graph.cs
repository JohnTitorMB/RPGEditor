using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Graph
{
    public Graph(int _ID)
    {
        nodes = new List<Node>();
        connections = new List<Connection>();
        ID = _ID;
    }

    public int ID = -1;
    [SerializeField]
    public List<Node> nodes = new List<Node>();

    public List<Connection> connections;

    public Graph()
    {
        nodes = new List<Node>();
        connections = new List<Connection>();
    }

    public void Draw()
    {
        RPGEditor.Instance.BeginWindows();

        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Draw(i,DrawNodeWindow);
        }

        RPGEditor.Instance.EndWindows();

        for (int i = 0; i < connections.Count; i++)
        {
            connections[i].Draw();
        }
    }


    void DrawNodeWindow(int id)
    {
        nodes[id].DrawNodeWindow(id);

        GUI.DragWindow();
    }

    public void AddNode(Node node)
    {
        nodes.Add(node);
        
       
    }
}

