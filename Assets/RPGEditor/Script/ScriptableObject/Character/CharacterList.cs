using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterList : ScriptableObject
{
    public List<Character> itemList;

    [MenuItem("Assets/Create/Inventory Character List")]
    public static CharacterList Create()
    {

        CharacterList asset = ScriptableObject.CreateInstance<CharacterList>();

        AssetDatabase.CreateAsset(asset, "Assets/CharacterList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}
