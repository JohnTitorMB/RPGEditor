using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RPGEditor : EditorWindow
{
    static CharacterList characterList;
    static EventActionList eventActionList;
    static ItemList itemList;
    static QuestList questList;
    static GraphList graphList;

    public static ObjectInteractableList objectInteractableList;
    //  public characterList characterList;
    
    public static RPGEditor Instance; 

    [MenuItem("Window/RPG Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(RPGEditor));
        



    }

    void OnEnable() 
    {
        Instance = this;

        Debug.Log("File");
        EditorCanvas.m_texture = AssetDatabase.LoadAssetAtPath("Assets/Level.png", typeof(Texture2D)) as Texture2D;

        if (System.IO.File.Exists("Assets/RPGEditor/Resources/characterList.asset"))
        {
            characterList = AssetDatabase.LoadAssetAtPath("Assets/RPGEditor/Resources/characterList.asset", typeof(CharacterList)) as CharacterList;

                if(characterList != null)
                    Debug.Log("GetCharacterList");
        }
        else
        {
            Debug.Log("Init characterList Item");
            
            InitCharacterList();
            CharacterEditor.selectedIndex = -1;
        }

        if (System.IO.File.Exists("Assets/RPGEditor/Resources/itemList.asset"))
        {
                itemList = AssetDatabase.LoadAssetAtPath("Assets/RPGEditor/Resources/itemList.asset", typeof(ItemList)) as ItemList;
        }
        else
        {
            Debug.Log("Init itemList Item");

            InitItemList();
            ItemEditor.selectedIndex = -1;
        }

        if (System.IO.File.Exists("Assets/RPGEditor/Resources/questList.asset"))
        {
                questList = AssetDatabase.LoadAssetAtPath("Assets/RPGEditor/Resources/questList.asset", typeof(QuestList)) as QuestList;

            
        }
        else
        {
            Debug.Log("Init questList Item");

            InitQuestList();
            QuestEditor.selectedIndex = -1;
        }

        if (System.IO.File.Exists("Assets/RPGEditor/Resources/graphList.asset"))
        {
            graphList = AssetDatabase.LoadAssetAtPath("Assets/RPGEditor/Resources/graphList.asset", typeof(GraphList)) as GraphList;

            if (graphList != null)
                Debug.Log("GetGraphList");
        }
        else
        {
            Debug.Log("Init graphList");

            InitGraphList();
        }

        if (System.IO.File.Exists("Assets/RPGEditor/Resources/eventActionList.asset"))
        {
            eventActionList = AssetDatabase.LoadAssetAtPath("Assets/RPGEditor/Resources/eventActionList.asset", typeof(EventActionList)) as EventActionList;

            if (eventActionList != null)
                Debug.Log("GetGeventActionList");
        }
        else
        {
            Debug.Log("Init eventActionList");

            InitEventActionList();
        }

        if (System.IO.File.Exists("Assets/RPGEditor/Resources/objectInteractableList.asset"))
        {
            objectInteractableList = AssetDatabase.LoadAssetAtPath("Assets/RPGEditor/Resources/objectInteractableList.asset", typeof(ObjectInteractableList)) as ObjectInteractableList;


        }
        else
        {

            InitObjectInteractableList();
        }
    }

    Vector2 scrollPos;
    Vector2 scrollPos2;

    private Color color_selected = Color.blue;
    private int editorSelectedIndex = 0;

    void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        Color color_default = GUI.backgroundColor;
        Color color_selected = Color.gray;

        GUIStyle itemStyle = new GUIStyle(GUI.skin.button);  //make a new GUIStyle

        itemStyle.alignment = TextAnchor.MiddleLeft; //align text to the left
        itemStyle.active.background = itemStyle.normal.background;  //gets rid of button click background style.
        itemStyle.margin = new RectOffset(0, 0, 0, 0); //removes the space between items (previously there was a small gap between GUI which made it harder to select a desired item)
        itemStyle.padding = new RectOffset(10, 10, 10, 10);

        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        
        GUI.backgroundColor = (editorSelectedIndex == 0) ? color_selected : Color.clear;
        if (GUILayout.Button("CharacterEditor", itemStyle,GUILayout.Height(30)))
            editorSelectedIndex = 0;
        GUI.backgroundColor = (editorSelectedIndex == 1) ? color_selected : Color.clear;
        if (GUILayout.Button("ItemEditor", itemStyle, GUILayout.Height(30)))
            editorSelectedIndex = 1;

        GUI.backgroundColor = (editorSelectedIndex == 2) ? color_selected : Color.clear;
        if (GUILayout.Button("QuestEditor", itemStyle, GUILayout.Height(30)))
            editorSelectedIndex = 2;

        GUI.backgroundColor = (editorSelectedIndex == 3) ? color_selected : Color.clear;
        if (GUILayout.Button("ObjectInteractableList", itemStyle, GUILayout.Height(30)))
            editorSelectedIndex = 3;

        GUI.backgroundColor = color_default;
        EditorGUILayout.EndHorizontal();
        
        if(editorSelectedIndex == 0)
        {
            if (CharacterEditor.ShowCharacterEditor(characterList, position))
            {
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(characterList);
                }
            }

        }
        
        if (editorSelectedIndex == 1)
        {
            if (ItemEditor.ShowItemEditor(itemList, position))
            {
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(itemList);
                }
            }


        }
        

        if (editorSelectedIndex == 2)
        {
            if (QuestEditor.ShowItemEditor(questList, graphList, position))
            {
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(questList);
                }


            }
        }

        if (editorSelectedIndex == 3)
        {
            if (ObjectInteractableEditor.ShowObjectInteractableEditor(objectInteractableList, position))
            {
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(objectInteractableList);
                }


            }
        }

      
        if(graphList)
            EditorUtility.SetDirty(graphList);

        if (eventActionList)
            EditorUtility.SetDirty(eventActionList);


        EditorGUILayout.EndVertical();


    }

    void InitCharacterList()
    {
        characterList = CharacterList.Create();
        if (characterList)
        {
            characterList.itemList = new List<Character>();
        }
    }

    void InitItemList()
    {
        itemList = ItemList.Create();
        if (characterList)
        {
            itemList.itemList = new List<Item>();
        }
    }

    void InitQuestList()
    {
        questList = QuestList.Create();
        if (questList)
        {
            questList.itemList = new List<Quest>();
        }
    }

    void InitGraphList()
    {

        graphList = CreateGraphList.Create();
        if (graphList)
        {
            graphList.characterGraphList = new List<Graph>();
            graphList.questGraphList = new List<Graph>();
        }
    }

    void InitEventActionList()
    {

        eventActionList = EventActionList.Create();
        if (eventActionList)
        {
            eventActionList.actionList = new List<MyAction>();
            eventActionList.eventList = new List<EventRPG>();
        }
    }

    void InitObjectInteractableList()
    {
        objectInteractableList = ObjectInteractableList.Create();
        if (objectInteractableList)
        {
            objectInteractableList.itemList = new List<ObjectInteractable>();
        }
    }

    public static List<Graph> GetGraph()
    {    
        return graphList.questGraphList;
    }

    public static List<Quest> GetQuests()
    {
        List<Quest> quests = new List<Quest>();

        if (questList)
        {
            for(int i = 0; i < questList.itemList.Count; i++)
            {
                List<Quest> subQuestQuestList = questList.itemList[i].GetQuests();
                for (int j = 0; j < subQuestQuestList.Count; j++)
                {
                    quests.Add(subQuestQuestList[j]);
                }
            }
        }
     
        return quests;
    }

    public static List<Character> GetCharacgter()
    {
        List<Character> characters;

        if (characterList)
            characters = characterList.itemList;
        else
            characters = new List<Character>();

        return characters;
    }

    public static List<Item> GetItem()
    {
        List<Item> items;

        if (itemList)
            items = itemList.itemList;
        else
            items = new List<Item>();

        return items;
    }

    public static EventActionList GetEventActionList()
    {
        return eventActionList;
    }

    public static List<ObjectInteractable> GetObjectInteractable()
    {
        List<ObjectInteractable> _objectInteractableList;

        if (objectInteractableList)
            _objectInteractableList = objectInteractableList.itemList;
        else
            _objectInteractableList = new List<ObjectInteractable>();

        return _objectInteractableList;
    }
}

