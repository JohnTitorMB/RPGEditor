using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ItemList : ScriptableObject
{
    public List<Item> itemList;

    [MenuItem("Assets/Create/Inventory RPGItem List")]
    public static ItemList Create()
    {
        ItemList asset = ScriptableObject.CreateInstance<ItemList>();

        AssetDatabase.CreateAsset(asset, "Assets/RPGEditor/Resources/itemList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
