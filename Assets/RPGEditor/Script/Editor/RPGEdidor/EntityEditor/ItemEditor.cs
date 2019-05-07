using UnityEngine;
using UnityEditor;

public class ItemEditor
{
    static Vector2 scrollPos;
    static Vector2 scrollPos2;
    public static int selectedIndex = -1;
    static Color color_selected = Color.blue;

    public static bool ShowItemEditor(ItemList itemList, Rect windowPosition)
    {
        if (itemList == null)
            return false;
      
        Color color_default = GUI.backgroundColor;
        Color color_selected = Color.gray;

        GUIStyle itemStyle = new GUIStyle(GUI.skin.button);  //make a new GUIStyle

        itemStyle.alignment = TextAnchor.MiddleLeft; //align text to the left
        itemStyle.active.background = itemStyle.normal.background;  //gets rid of button click background style.
        itemStyle.margin = new RectOffset(0, 0, 0, 0); //removes the space between items (previously there was a small gap between GUI which made it harder to select a desired item)
        itemStyle.padding = new RectOffset(10, 10, 10, 10);

        EditorGUILayout.BeginHorizontal();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(windowPosition.width / 6), GUILayout.Height(windowPosition.height));

                for (int i = 0; i < itemList.itemList.Count; i++)
                {
                    GUI.backgroundColor = (selectedIndex == i) ? color_selected : Color.clear;

                    //show a button using the new GUIStyle
                    if (GUILayout.Button(itemList.itemList[i].name, itemStyle))
                    {
                        selectedIndex = i;
                        //do something else (e.g ping an object)
                    }

                    GUI.backgroundColor = color_default; //this is to avoid affecting other GUIs outside of the list
                }

            EditorGUILayout.EndScrollView();    
            scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.Height(windowPosition.height));
                EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("Add Item"))
                    {
                        Item item = ScriptableObject.CreateInstance<Item>();
                        item.name = "New Item";
                        AssetDatabase.AddObjectToAsset(item, itemList);
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(item));
                        itemList.itemList.Add(item);
                    }

                    if (GUILayout.Button("Remove Item"))
                    {
                        if (selectedIndex != -1)
                        {
                            Item item = itemList.itemList[selectedIndex];
                            itemList.itemList.RemoveAt(selectedIndex);
                            string path = AssetDatabase.GetAssetPath(item);
                            AssetDatabase.RemoveObjectFromAsset(item);
                            AssetDatabase.ImportAsset(path);

                            selectedIndex = 0;
                            if (itemList.itemList.Count == 0)
                            {
                                selectedIndex = -1;
                            }
                        }
                    }

                EditorGUILayout.EndHorizontal();

                if (selectedIndex >= 0)
                {

                    string name = EditorGUILayout.TextField("ItemName:", itemList.itemList[selectedIndex].name, GUILayout.Width(300));
                    
                    if(name != itemList.itemList[selectedIndex].name)
                    {
                        itemList.itemList[selectedIndex].name = name;
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(itemList.itemList[selectedIndex]));
           
                    }
                
                    itemList.itemList[selectedIndex].ItemDescription = EditorGUILayout.TextField("ItemDescription:", itemList.itemList[selectedIndex].ItemDescription, GUILayout.Width(300));
                    itemList.itemList[selectedIndex].ItemIcon = EditorGUILayout.ObjectField("ItemIcon:", itemList.itemList[selectedIndex].ItemIcon, typeof(Sprite), false, GUILayout.Width(200)) as Sprite;
                    Item.ItemType CurrentItemType = itemList.itemList[selectedIndex].itemType;
                    itemList.itemList[selectedIndex].itemType = (Item.ItemType)EditorGUILayout.EnumPopup(itemList.itemList[selectedIndex].itemType);
                    if(itemList.itemList[selectedIndex].itemType != CurrentItemType)
                    {
                        string path = AssetDatabase.GetAssetPath(itemList.itemList[selectedIndex]);
                        Item item = null;
                        switch (itemList.itemList[selectedIndex].itemType)
                        {
                            case Item.ItemType.Weapon:
                            {
                                item = ScriptableObject.CreateInstance<WeaponItem>();
                            }
                            break;
                            case Item.ItemType.Shield:
                            {
                                item = ScriptableObject.CreateInstance<ShieldItem>();
                            }
                            break;
                            case Item.ItemType.Potion:
                            {
                               item = ScriptableObject.CreateInstance<PotionItem>();
                            }
                            break;
                        }

                        if(item != null)
                        {
                            item.Copy(itemList.itemList[selectedIndex]);
                            AssetDatabase.AddObjectToAsset(item, itemList);
                            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(item));

                            AssetDatabase.RemoveObjectFromAsset(itemList.itemList[selectedIndex]);
                            AssetDatabase.ImportAsset(path);

                            itemList.itemList[selectedIndex] = item;
                        }
                    }

                    displayItemMembers(itemList);
                }

        EditorGUILayout.EndScrollView(); 
        EditorGUILayout.EndHorizontal();
 
        return true;
    }

    static void displayItemMembers(ItemList itemList)
    {
        switch (itemList.itemList[selectedIndex].itemType)
        {
            case Item.ItemType.Weapon:
                WeaponItem weaponItem = (WeaponItem)itemList.itemList[selectedIndex];
                weaponItem.Dammage = EditorGUILayout.IntField("Dammage: ", weaponItem.Dammage, GUILayout.Width(200));
                weaponItem.MPCost = EditorGUILayout.IntField("MPCost: ", weaponItem.MPCost, GUILayout.Width(200));
                weaponItem.canBreak = EditorGUILayout.Toggle("canBreak: ", weaponItem.canBreak, GUILayout.Width(200));
                if(weaponItem.canBreak)
                {
                    weaponItem.HP = EditorGUILayout.IntField("HP: ", weaponItem.HP, GUILayout.Width(200));                  
                    EditorGUILayout.BeginHorizontal();
                        weaponItem.HpPerUsing = EditorGUILayout.IntField("HpPerUsing: ", weaponItem.HP, GUILayout.Width(200));
                        weaponItem.HpPerUsingRandom = EditorGUILayout.IntField("+-", weaponItem.HpPerUsingRandom, GUILayout.Width(200));
                    EditorGUILayout.EndHorizontal();
                }
            break;
            case Item.ItemType.Shield:
                ShieldItem shieldItem = (ShieldItem)itemList.itemList[selectedIndex];
                shieldItem.resistence = EditorGUILayout.IntField("Resistence: ", shieldItem.resistence, GUILayout.Width(200));
                shieldItem.resistenceMagic = EditorGUILayout.IntField("ResistenceMagic: ", shieldItem.resistenceMagic, GUILayout.Width(200));
                shieldItem.canBreak = EditorGUILayout.Toggle("CanBreak: ", shieldItem.canBreak, GUILayout.Width(200));
                if (shieldItem.canBreak)
                {
                    shieldItem.HP = EditorGUILayout.IntField("HP: ", shieldItem.HP, GUILayout.Width(200));
                    /*
                    EditorGUILayout.BeginHorizontal();
                        shieldItem.HpPerUsing = EditorGUILayout.IntField("HpPerUsing: ", shieldItem.HP, GUILayout.Width(200));
                        shieldItem.HpPerUsingRandom = EditorGUILayout.IntField("+-", shieldItem.HpPerUsingRandom, GUILayout.Width(200));
                    EditorGUILayout.EndHorizontal();*/
                }
            break;
            case Item.ItemType.Potion:
                PotionItem potionItem = (PotionItem)itemList.itemList[selectedIndex];

                potionItem.potionItemType = (PotionItem.PotionItemType)EditorGUILayout.EnumPopup(potionItem.potionItemType);
                switch (potionItem.potionItemType)
                {
                    case PotionItem.PotionItemType.HP:
                        potionItem.HP = EditorGUILayout.IntField("HP: ", potionItem.HP, GUILayout.Width(200));
                        potionItem.poisonous = EditorGUILayout.Toggle("IsPoisonous: ", potionItem.poisonous, GUILayout.Width(200));
                        break;
                    case PotionItem.PotionItemType.MP:
                        potionItem.MP = EditorGUILayout.IntField("MP: ", potionItem.MP, GUILayout.Width(200));
                        break;
                }
                
                break;
        }
    }

}
