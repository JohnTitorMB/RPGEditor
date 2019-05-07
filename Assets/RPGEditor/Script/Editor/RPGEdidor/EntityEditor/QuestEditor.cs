using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;




public class QuestEditor
{
    static Vector2 scrollPos;
    static Vector2 scrollPos2;
    public static int selectedIndex = -1;
    static Color color_selected = Color.blue;

    private static int editorSelectedIndex = 0;

    static QuestList questList;
    static GraphList graphList;

    public static bool ShowItemEditor(QuestList _questList, GraphList _graphList, Rect windowPosition)
    {
        if (_questList == null)
            return false;

        graphList = _graphList;

        questList = _questList;

        Color color_default = GUI.backgroundColor;
        Color color_selected = Color.gray;

        GUIStyle itemStyle = new GUIStyle(GUI.skin.button);  //make a new GUIStyle

        itemStyle.alignment = TextAnchor.MiddleLeft; //align text to the left
        itemStyle.active.background = itemStyle.normal.background;  //gets rid of button click background style.
        itemStyle.margin = new RectOffset(0, 0, 0, 0); //removes the space between items (previously there was a small gap between GUI which made it harder to select a desired item)
        itemStyle.padding = new RectOffset(10, 10, 10, 10);

        EditorGUILayout.BeginHorizontal();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(windowPosition.width / 6), GUILayout.Height(windowPosition.height));


        List<Quest> quests = questList.itemList;


        int index = -1;

        DrawQuestList(quests, color_default, color_selected, itemStyle, ref index,0);


        EditorGUILayout.EndScrollView();

        scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.Height(windowPosition.height));
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Quest"))
        {
            EventActionList eventActionList = RPGEditor.GetEventActionList();

            if (eventActionList)
            {
                Quest quest = ScriptableObject.CreateInstance<Quest>();
                quest.name = "New Quest";
                AssetDatabase.AddObjectToAsset(quest, questList);

                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(quest));
                quest.Init(eventActionList);

                questList.itemList.Add(quest);

                graphList.questGraphList.Add(new Graph(graphList.questGraphList.Count));
            }

        }

        if (GUILayout.Button("Remove Quest"))
        {

            if (selectedIndex != -1)
            {
                DestroyQuest(ref questList.itemList);
            }

            selectedIndex = -1;
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        GUI.backgroundColor = (editorSelectedIndex == 0) ? color_selected : Color.clear;
        if (GUILayout.Button("QuestEdition", itemStyle))
            editorSelectedIndex = 0;
        GUI.backgroundColor = (editorSelectedIndex == 1) ? color_selected : Color.clear;
        if (GUILayout.Button("QuestGraph", itemStyle))
            editorSelectedIndex = 1;

        GUI.backgroundColor = color_default;


        EditorGUILayout.EndHorizontal();
        if (selectedIndex >= 0)
        {
            int indexinList = -1;
            index = -1;
            Quest quest = null;
            quests = questList.itemList;

            GetIndexInList(quests, ref index, ref indexinList, ref quest, false);

            if (editorSelectedIndex == 0)
            {
                if (QuestEdition.ShowItemEditor(graphList, quest, ref indexinList, windowPosition))
                {

                }
            }

            if (editorSelectedIndex == 1)
            {
                if (graphList == null)
                    Debug.Log("bonjour");
                if (DrawingGraph.DrawQuestGraphNodes(graphList, selectedIndex, windowPosition, quest))
                {

                }
            }
        }


        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();

        return true;
    }

    public static void DestroyQuest(ref List<Quest> list)
    {
        Quest quest = null;
        int indexinList = -1;
        int index = -1;

        GetIndexInList(questList.itemList, ref index, ref indexinList, ref quest, false);

        DrawingGraph.DestroyEvent(quest.EventIsAvailable);
        DrawingGraph.DestroyEvent(quest.EventIsRun);
        DrawingGraph.DestroyEvent(quest.EventOnAvailable);
        DrawingGraph.DestroyEvent(quest.EventOnCompleted);
        DrawingGraph.DestroyEvent(quest.EventOnStart);


        list.Remove(quest);
        string path = AssetDatabase.GetAssetPath(quest);
        AssetDatabase.RemoveObjectFromAsset(quest);
        AssetDatabase.ImportAsset(path);

        Graph graph = graphList.questGraphList[selectedIndex];

        for (int i = 0; i < graph.nodes.Count; i++)
        {
            DrawingGraph.DestroyNode(graph.nodes[i]);
        }

        graphList.questGraphList.RemoveAt(selectedIndex);
    }

    static void DrawQuestList(List<Quest> quests, Color color_default, Color color_selected, GUIStyle itemStyle, ref int index,int indexSpace)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            index++;

            GUI.backgroundColor = (selectedIndex == index) ? color_selected : Color.clear;

            itemStyle.padding = new RectOffset(10+ indexSpace * 10, 10, 10, 10);

            //show a button using the new GUIStyle
            if (GUILayout.Button(quests[i].name, itemStyle))
            {
                selectedIndex = index;
                GUI.FocusControl("");
                Debug.Log(selectedIndex);
                //do something else (e.g ping an object)
            }

            itemStyle.padding = new RectOffset(10, 10, 10, 10);

            DrawQuestList(quests[i].subQuest, color_default, color_selected, itemStyle, ref index, indexSpace + 1);

            GUI.backgroundColor = color_default; //this is to avoid affecting other GUIs outside of the list
        }
    }

    static void GetIndexInList(List<Quest> quests, ref int index,ref int indexInList,ref Quest quest,bool find)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            index++;
            if (!find)
            {
                if ((selectedIndex == index))
                {
                    indexInList = i;
                    quest = quests[i];
                    find = true;
                    break;
                }
            }
            
            if(!find)
                GetIndexInList(quests[i].subQuest, ref index,ref indexInList,ref quest,find);
        }
    }
}

