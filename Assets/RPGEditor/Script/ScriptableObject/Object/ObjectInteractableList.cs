using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectInteractableList : ScriptableObject
{
    public List<ObjectInteractable> itemList;

    [MenuItem("Assets/Create/Inventory Object List")]
    public static ObjectInteractableList Create()
    {

        ObjectInteractableList asset = ScriptableObject.CreateInstance<ObjectInteractableList>();

        AssetDatabase.CreateAsset(asset, "Assets/ObjectInteractableList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
