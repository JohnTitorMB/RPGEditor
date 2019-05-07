using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class QuestEdition
{

    static Vector2 scrollPos;
    static Vector2 scrollPos2;
    static Color color_selected = Color.blue;

    public static bool ShowItemEditor(GraphList graphList, Quest quest,ref int selectedIndex, Rect windowPosition)
    {
        if (quest == null || graphList == null)
            return false;


        Color color_default = GUI.backgroundColor;
        Color color_selected = Color.gray;

        GUIStyle itemStyle = new GUIStyle(GUI.skin.button);  //make a new GUIStyle

        itemStyle.alignment = TextAnchor.MiddleLeft; //align text to the left
        itemStyle.active.background = itemStyle.normal.background;  //gets rid of button click background style.
        itemStyle.margin = new RectOffset(0, 0, 0, 0); //removes the space between items (previously there was a small gap between GUI which made it harder to select a desired item)
        itemStyle.padding = new RectOffset(10, 10, 10, 10);

        EditorGUILayout.BeginHorizontal();
        string name = EditorGUILayout.TextField("Quest Name :", quest.name, GUILayout.Width(300));

        if (name != quest.name)
        {
            quest.name = name;
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(quest));
        }

        GUILayout.Space(40);
        quest.questDescription = EditorGUILayout.TextField("Quest Description :", quest.questDescription, GUILayout.Width(900), GUILayout.Height(100));
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add SubQuest"))
        {
            EventActionList eventActionList = RPGEditor.GetEventActionList();

            if (eventActionList)
            {
                Quest subQuest = ScriptableObject.CreateInstance<Quest>();
                subQuest.name = "New Quest";
                AssetDatabase.AddObjectToAsset(subQuest, quest);

                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(subQuest));
                subQuest.Init(eventActionList);

                quest.subQuest.Add(subQuest);

                graphList.questGraphList.Add(new Graph(graphList.questGraphList.Count));
            }
        }

        if (GUILayout.Button("Remove SubQuest"))
        {



            QuestEditor.DestroyQuest(ref quest.subQuest);
        }
        EditorGUILayout.EndHorizontal();

        return true;
    }
}

