using UnityEngine;
using UnityEditor;

public class ObjectInteractableEditor
{
    static Vector2 scrollPos;
    static Vector2 scrollPos2;
    public static int selectedIndex = -1;
    static Color color_selected = Color.blue;

    public static bool ShowObjectInteractableEditor(ObjectInteractableList objectInteractableList, Rect windowPosition)
    {      
        if (objectInteractableList == null)
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

        for (int i = 0; i < objectInteractableList.itemList.Count; i++)
        {
            GUI.backgroundColor = (selectedIndex == i) ? color_selected : Color.clear;

            //show a button using the new GUIStyle
            if (GUILayout.Button(objectInteractableList.itemList[i].name, itemStyle))
            {
                selectedIndex = i;
                GUI.FocusControl("");
                //do something else (e.g ping an object)
            }

            GUI.backgroundColor = color_default; //this is to avoid affecting other GUIs outside of the list
        }

        EditorGUILayout.EndScrollView();
        scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.Height(windowPosition.height));
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Object"))
        {
            ObjectInteractable objectInteractable = ScriptableObject.CreateInstance<ObjectInteractable>();
            objectInteractable.name = "ObjectInteractable";
            AssetDatabase.AddObjectToAsset(objectInteractable, objectInteractableList);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(objectInteractable));
            objectInteractableList.itemList.Add(objectInteractable);
        }

        if (GUILayout.Button("Remove Object"))
        {
            if (selectedIndex != -1)
            {
                ObjectInteractable objectInteractable = objectInteractableList.itemList[selectedIndex];
                objectInteractableList.itemList.RemoveAt(selectedIndex);
                string path = AssetDatabase.GetAssetPath(objectInteractable);
                AssetDatabase.RemoveObjectFromAsset(objectInteractable);
                AssetDatabase.ImportAsset(path);

                selectedIndex = 0;
                if (objectInteractableList.itemList.Count == 0)
                {
                    selectedIndex = -1;
                }
            }
        }

        EditorGUILayout.EndHorizontal();

        if (selectedIndex >= 0)
        {
            GUIStyle style = new GUIStyle(GUI.skin.textField);
            style.normal.textColor = Color.black;
            string name = EditorGUILayout.TextField("Name:", objectInteractableList.itemList[selectedIndex].name, style, GUILayout.Width(300));
            
            if(name != objectInteractableList.itemList[selectedIndex].name)
            {
                objectInteractableList.itemList[selectedIndex].name = name;
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(objectInteractableList));
            }


            objectInteractableList.itemList[selectedIndex].type = EditorGUILayout.TextField("Type:", objectInteractableList.itemList[selectedIndex].type, style, GUILayout.Width(300));
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();

        return true;
    }

}
