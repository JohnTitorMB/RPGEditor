using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class EventActionList : ScriptableObject
{
    public List<EventRPG> eventList;
    public List<MyAction> actionList;


    [MenuItem("Assets/Create/Event-Action List")]
    public static EventActionList Create()
    {
        EventActionList asset = ScriptableObject.CreateInstance<EventActionList>();

        AssetDatabase.CreateAsset(asset, "Assets/RPGEditor/Resources/eventActionList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}