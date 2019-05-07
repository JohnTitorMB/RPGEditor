using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestStatueList : ScriptableObject
{
    public List<QuestStatues> itemList = new List<QuestStatues>();

    [MenuItem("Assets/Create/Quest Statue")]
    public static QuestStatueList Create()
    {
        QuestStatueList asset = ScriptableObject.CreateInstance<QuestStatueList>();

        AssetDatabase.CreateAsset(asset, "Assets/RPGEditor/Resources/queststatueList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
