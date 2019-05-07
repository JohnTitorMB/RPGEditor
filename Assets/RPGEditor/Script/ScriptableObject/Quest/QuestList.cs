using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestList : ScriptableObject
{
    public List<Quest> itemList;

    [MenuItem("Assets/Create/Inventory Quest List")]
    public static QuestList Create()
    {
        QuestList asset = ScriptableObject.CreateInstance<QuestList>();

        AssetDatabase.CreateAsset(asset, "Assets/RPGEditor/Resources/questList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
