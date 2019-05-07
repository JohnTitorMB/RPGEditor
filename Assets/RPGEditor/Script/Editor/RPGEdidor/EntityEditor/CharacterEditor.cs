using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;





public class CharacterEditor
{
    static Vector2 scrollPos;
    static Vector2 scrollPos2;
    static Vector3 dialogueListscrollPos;
    static Vector3 dialoguescrollPos;

    public static int selectedIndex = -1;
    public static int selectedIndexDialog = -1;
    static Color color_selected = Color.blue;

    public static bool ShowCharacterEditor(CharacterList characterList,Rect windowPosition)
    {
        if (characterList == null)
            return false;

        Color color_default = GUI.backgroundColor;

        GUIStyle itemStyle = new GUIStyle(GUI.skin.button);  //make a new GUIStyle

        itemStyle.alignment = TextAnchor.MiddleLeft; //align text to the left
        itemStyle.active.background = itemStyle.normal.background;  //gets rid of button click background style.
        itemStyle.margin = new RectOffset(0, 0, 0, 0); //removes the space between items (previously there was a small gap between GUI which made it harder to select a desired item)
        itemStyle.padding = new RectOffset(10, 10, 10, 10);
        itemStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
                int width = 250;
                GUILayout.Box("Character", MyGUIStyle.EditorStyle(width, 50), GUILayout.Width(width),GUILayout.Height(50));

                GUIStyle modifyListStyle = new GUIStyle(GUI.skin.button);
                modifyListStyle.alignment = TextAnchor.MiddleLeft; //align text to the left
                Sprite t = AssetDatabase.LoadAssetAtPath("Assets/Block.png", typeof(Sprite)) as Sprite;
                modifyListStyle.alignment = TextAnchor.MiddleCenter;
                modifyListStyle.margin = new RectOffset(10, 0, 0, 0);

                EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Add Character", modifyListStyle, GUILayout.Width(120), GUILayout.Height(25)))
                    {
                        Character character = ScriptableObject.CreateInstance<Character>();
                        character.name = "New Character";
                        AssetDatabase.AddObjectToAsset(character, characterList);
                        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(character));
                        characterList.itemList.Add(character);
                    }

                    if (GUILayout.Button("Remove Character", modifyListStyle, GUILayout.Width(120), GUILayout.Height(25)))
                    {
                        if (selectedIndex != -1)
                        {
                            Character character = characterList.itemList[selectedIndex];
                            characterList.itemList.RemoveAt(selectedIndex);
                            string path = AssetDatabase.GetAssetPath(character);
                            AssetDatabase.RemoveObjectFromAsset(character);
                            AssetDatabase.ImportAsset(path);

                            selectedIndex = 0;
                            if (characterList.itemList.Count == 0)
                            {
                                selectedIndex = -1;
                            }
                        }
                    }
                EditorGUILayout.EndHorizontal();
        
                int height = Mathf.RoundToInt(windowPosition.height)-145;
                GUILayout.BeginArea(new Rect(10, 135, 250, height));
                    GUIStyle itemStyle2 = new GUIStyle();  //make a new GUIStyle
                    Texture2D texture = AssetDatabase.LoadAssetAtPath("Assets/RPGEditor/Sprite/ListStyle.png", typeof(Texture2D)) as Texture2D;
                    int heightElement = 40;

                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, itemStyle2, GUILayout.Width(width), GUILayout.Height(height));

                        if (characterList.itemList.Count * (heightElement / 2) > height)
                            height = characterList.itemList.Count * (heightElement / 2);

                        for (int y = 0; y < height; y += heightElement)
                        {
                            for (int x = 0; x < width; x += texture.width)
                            {
                                GUI.DrawTexture(new Rect(x, y, width, heightElement), texture);
                            }
                        }

                        itemStyle.normal.textColor = new Color(1, 1, 1);
                        itemStyle.padding = new RectOffset(0, 0, 0, 0);
                        itemStyle.margin = new RectOffset(0, 0, 0, 0);

                        Color color_selected = Color.gray;

                        for (int i = 0; i < characterList.itemList.Count; i++)
                        {
                            GUI.backgroundColor = (selectedIndex == i) ? color_selected : Color.clear;
            
                            if (GUILayout.Button(characterList.itemList[i].name, itemStyle,GUILayout.Width(width), GUILayout.Height(heightElement/2)))
                            {
                                GUI.FocusControl("");
                                selectedIndex = i;
                                selectedIndexDialog =  -1;
                            }

                            GUI.backgroundColor = color_default; //this is to avoid affecting other GUIs outside of the list
                        }
                    EditorGUILayout.EndScrollView();
                GUILayout.EndArea();
            EditorGUILayout.EndVertical();

        GUIStyle scrollViewStyle = new GUIStyle(GUI.skin.scrollView);  //make a new GUIStyle

        scrollViewStyle.margin = new RectOffset(0, 0, 10, 10);
        scrollViewStyle.normal.background = MyGUIStyle.MakeTex(30, 30, new Color(0.23529411764f, 0.23529411764f, 0.23529411764f), Color.black, 0);

        GUILayout.BeginArea(new Rect(270,40, windowPosition.width-280, windowPosition.height - 50));
        scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, scrollViewStyle, GUILayout.Height(windowPosition.height-50));

        if (selectedIndex >= 0)
        {
            GUILayout.Space(10);
            GUIStyle itemStyleTest = new GUIStyle();
            itemStyleTest.alignment = TextAnchor.MiddleLeft; //align text to the left
            itemStyleTest.normal.background = MyGUIStyle.MakeTex(500, 500, new Color(0.23529411764f, 0.23529411764f, 0.23529411764f), Color.black, 0);//t.texture;

            GUIStyle s = new GUIStyle();
            s.normal.textColor = Color.white;
            s.margin = new RectOffset(10, 0, 0, 0);

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("CharacterName : ", s, GUILayout.Width(150));


         

            string name = EditorGUILayout.TextField(characterList.itemList[selectedIndex].name, GUILayout.Width(100));

            if (characterList.itemList[selectedIndex].name != name)
            {
                characterList.itemList[selectedIndex].name = name;

                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(characterList.itemList[selectedIndex]));

            }


            GUILayout.Label("CharacterAvatar : ", s, GUILayout.Width(150));
            characterList.itemList[selectedIndex].characterAvatar = EditorGUILayout.ObjectField(characterList.itemList[selectedIndex].characterAvatar, typeof(Texture2D), false, GUILayout.Width(100), GUILayout.Height(100)) as Texture2D;
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(1);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("CharacterType : ", s, GUILayout.Width(150));
            characterList.itemList[selectedIndex].characterType = (CharacterType)EditorGUILayout.EnumPopup(characterList.itemList[selectedIndex].characterType, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(1);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("MaxLevel : ", s, GUILayout.Width(150));
            characterList.itemList[selectedIndex].MaxLevel = EditorGUILayout.IntField(characterList.itemList[selectedIndex].MaxLevel, GUILayout.Width(100));    
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(1);

            EditorGUILayout.BeginHorizontal();

            int Level = characterList.itemList[selectedIndex].Level;
            GUILayout.Label("Level : ", s, GUILayout.Width(150));
            characterList.itemList[selectedIndex].Level = Mathf.CeilToInt(EditorGUILayout.Slider(characterList.itemList[selectedIndex].Level, 1, characterList.itemList[selectedIndex].MaxLevel, GUILayout.Width(200)));

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(1);
          
            float MaxXP = characterList.itemList[selectedIndex].XpPerLevel.Evaluate(Level);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("XP : ", s, GUILayout.Width(150));
            characterList.itemList[selectedIndex].XP = Mathf.CeilToInt(EditorGUILayout.Slider(characterList.itemList[selectedIndex].XP,0, (int)MaxXP, GUILayout.Width(200)));
            GUILayout.Box(Mathf.CeilToInt(MaxXP).ToString());
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(1);

            float MaxHP = characterList.itemList[selectedIndex].MaxHPPerLevel.Evaluate(Level);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("HP : ", s, GUILayout.Width(150));
            characterList.itemList[selectedIndex].HP = Mathf.CeilToInt(EditorGUILayout.Slider(characterList.itemList[selectedIndex].HP,0, (int)MaxHP, GUILayout.Width(200)));
            GUILayout.Box(Mathf.CeilToInt(MaxHP).ToString());
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(1);


            float MaxMP = characterList.itemList[selectedIndex].MaxMPPerLevel.Evaluate(Level);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("MP : ", s, GUILayout.Width(150));
            characterList.itemList[selectedIndex].MP = Mathf.CeilToInt(EditorGUILayout.Slider(characterList.itemList[selectedIndex].MP,0, (int)MaxMP, GUILayout.Width(200)));
            GUILayout.Box(Mathf.CeilToInt(MaxMP).ToString());
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(1);

         //   characterList.itemList[selectedIndex].XpPerLevel = EditorGUILayout.CurveField(characterList.itemList[selectedIndex].XpPerLevel, GUILayout.Width(100));
            ClampValue(characterList.itemList[selectedIndex].XpPerLevel, "Level/XP : ", characterList.itemList[selectedIndex].MaxLevel);
          //  float MaxXP = characterList.itemList[selectedIndex].XpPerLevel.keys[characterList.itemList[selectedIndex].XpPerLevel.length - 1].value;
            int MaxLevel = characterList.itemList[selectedIndex].MaxLevel;
            ClampValue(characterList.itemList[selectedIndex].XpPerLevelEnnemy, "XP/LevelEnnemy : ", 0,false);
            ClampValue(characterList.itemList[selectedIndex].MaxHPPerLevel, "MaxHP/Level : ", (int)MaxLevel);
            ClampValue(characterList.itemList[selectedIndex].MaxMPPerLevel, "MaxMP/Level : ", (int)MaxLevel);
            ClampValue(characterList.itemList[selectedIndex].AttackPerLevel, "Attack/Level : ", (int)MaxLevel);
            ClampValue(characterList.itemList[selectedIndex].MagicAttackPerLevel, "MagicAttack/Level : ", (int)MaxLevel);
            ClampValue(characterList.itemList[selectedIndex].AgilityPerLevel, "Agility/Level : ", (int)MaxLevel);
            ClampValue(characterList.itemList[selectedIndex].DefencePerLevel, "Defence/Level : ", (int)MaxLevel);
            ClampValue(characterList.itemList[selectedIndex].MagicDefenceLevel, "MagicDefence/Level : ", (int)MaxLevel);
            ClampValue(characterList.itemList[selectedIndex].luckPerLevel, "luckPerLevel/Level : ", (int)MaxLevel);

            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();

                        if (GUILayout.Button("Add Dialog", modifyListStyle, GUILayout.Width(120), GUILayout.Height(25)))
                        {
                            characterList.itemList[selectedIndex].dialogues.Add(new DialogueStatic());
                        }

                        if (GUILayout.Button("Remove Dialog", modifyListStyle, GUILayout.Width(120), GUILayout.Height(25)))
                        {
                             characterList.itemList[selectedIndex].dialogues.RemoveAt(selectedIndexDialog);
                             selectedIndexDialog = 0;
                            if (characterList.itemList[selectedIndex].dialogues.Count == 0)
                            {
                                selectedIndexDialog = -1;
                            }
                        }

                    EditorGUILayout.EndHorizontal();

                    dialogueListscrollPos = EditorGUILayout.BeginScrollView(dialogueListscrollPos, itemStyle2, GUILayout.Width(240));
                        for (int i = 0; i < characterList.itemList[selectedIndex].dialogues.Count; i++)
                            {
                                GUI.backgroundColor = (selectedIndexDialog == i) ? color_selected : Color.clear;

                                if (GUILayout.Button("Dialog " + i, itemStyle, GUILayout.Width(240)))
                                {
                                    GUI.FocusControl("");
                                    selectedIndexDialog = i;
                                }
                                GUI.backgroundColor = color_default; //this is to avoid affecting other GUIs outside of the list
                            }
                    EditorGUILayout.EndScrollView();

                EditorGUILayout.EndVertical();

                dialoguescrollPos = EditorGUILayout.BeginScrollView(dialoguescrollPos, itemStyle2);

                if (selectedIndexDialog >= 0)
                {

                    List<Reply> replies = characterList.itemList[selectedIndex].dialogues[selectedIndexDialog].Replies;

                    int size = replies.Count;

                    GUILayout.BeginHorizontal();

                    GUILayout.Label("Size : ",GUILayout.Width(100));
                    int newSize = EditorGUILayout.IntField(size);

                    if (newSize <= 0)
                        replies.Clear();
                    else if(newSize < size)
                    {
                        for (int i = size - 1; i >= newSize;i--)
                        {
                            replies.RemoveAt(i);
                        }
                    }

                    else if (newSize > size)
                    {
                        for (int i = size; i < newSize; i++)
                        {
                            Reply reply = new Reply();
                            reply.parametre = new Parametre(typeof(string));
                            reply.reply = "";
                            replies.Add(reply);
                        }
                    }

                    GUILayout.EndHorizontal();

                    for(int i = 0; i < replies.Count;i++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Reply : " + (i+1), GUILayout.Width(100));

                    Parametre parametreCharacter = replies[i].parametre;

                        List<string> names = new List<string>(); ;

                        
                        names.Add(characterList.itemList[selectedIndex].name);

                        names.Add("Player");

                        string[] namesArray = names.ToArray();

                        parametreCharacter.selected = EditorGUILayout.Popup(parametreCharacter.selected, namesArray, GUILayout.Width(100));

                        string nameObject = namesArray[parametreCharacter.selected];

                        parametreCharacter.parameterString = nameObject;

                        string replystring = EditorGUILayout.TextField(replies[i].reply);

                        Reply reply = new Reply();
                        reply.parametre = parametreCharacter;
                        reply.reply = replystring;

                         replies[i] = reply;

                    //  replies[i] = new Tuple<Parametre, string>(parametreCharacter, reply);

                    GUILayout.EndHorizontal();
                    }



            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
        GUILayout.EndArea();
     

        EditorGUILayout.EndHorizontal();

        return true;
    }

    static void ClampValue(AnimationCurve AnimationCurve,string label,int value,bool clamp = true)
    {
        GUIStyle s = new GUIStyle();
        s.normal.textColor = Color.white;
        s.margin = new RectOffset(10, 0, 0, 0);

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(label, s, GUILayout.Width(150));
        AnimationCurve = EditorGUILayout.CurveField(AnimationCurve, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();
        
        Keyframe[] keyframe = AnimationCurve.keys;
        keyframe[0].value = 0;
        keyframe[0].time = 0;
        if (clamp)
            keyframe[AnimationCurve.length - 1].time = value;

        keyframe[AnimationCurve.length - 1].time = Mathf.CeilToInt(keyframe[AnimationCurve.length - 1].time);
            keyframe[AnimationCurve.length - 1].value = Mathf.CeilToInt(keyframe[AnimationCurve.length - 1].value);
        AnimationCurve.keys = keyframe;
        GUILayout.Space(1);

    }
}


public class EditorCanvas
{
    public static Texture2D m_texture;
    public static void DrawQuad(int x, int y, int width, int height, Color color)
    {
        GUIStyle style = new GUIStyle();
        Sprite t = AssetDatabase.LoadAssetAtPath("Assets/Level.png", typeof(Sprite)) as Sprite;
        style.normal.background = t.texture;
            Debug.Log("oui");
            Rect rect = new Rect(x, y, width, height);
            GUI.Box(rect, GUIContent.none, style);
        


    }
}

public class MyGUIStyle
{
    public static GUIStyle EditorStyle(int Width,int Height)
    {
        
        GUIStyle itemStyle = new GUIStyle();
        itemStyle.alignment = TextAnchor.MiddleLeft; //align text to the left
        Sprite t = AssetDatabase.LoadAssetAtPath("Assets/Block.png", typeof(Sprite)) as Sprite;
        itemStyle.normal.background = MakeTex(Width, Height,new Color(0.23529411764f, 0.23529411764f, 0.23529411764f), Color.black,0);//t.texture;
        itemStyle.normal.textColor = new Color(1, 1, 1);
        itemStyle.alignment = TextAnchor.MiddleCenter;
        itemStyle.fontSize = 30;
        itemStyle.margin = new RectOffset(10, 10, 10, 10);
        return itemStyle;
    }

    static Texture2D textureListStyle;
    static bool l = false;
    public static GUIStyle ListStyle(int Width, int Height, int elementSize)
    {

        GUIStyle itemStyle = new GUIStyle();
        itemStyle.alignment = TextAnchor.MiddleLeft; //align text to the left
        Sprite t = AssetDatabase.LoadAssetAtPath("Assets/Block.png", typeof(Sprite)) as Sprite;

        itemStyle.normal.background = null;
  
        itemStyle.normal.textColor = new Color(1, 1, 1);
         // Texture2D texture2d = MakeTexListSytle(Width, Height, new Color(0.23529411764f, 0.23529411764f, 0.23529411764f), new Color(45, 47, 49) / 255, Color.black, false, elementSize, 1);//t.texture
          itemStyle.margin = new RectOffset(10, 0, 10, 0);


        return itemStyle;
    }


    public static Texture2D MakeTex(int width, int height, Color col, Color borderColor, int borderSize)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                if (i < borderSize || i >= height - borderSize || j < borderSize || j >= width - borderSize)
                    pix[i * width + j] = borderColor;
                else
                    pix[i * width + j] = col;


            }
        }

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.wrapMode = TextureWrapMode.Repeat;
        result.Apply();
        return result;
    }


    public static Texture2D MakeTexListSytle(int width, int height, Color col, Color col2,Color borderColor, bool horizontal, int size,int borderSize)
    {
        height = 2 * size;
        Debug.Log(height);
        width += borderSize * 2;
        height += borderSize * 2;

        Color[] pix = new Color[width * height];

        int num = Mathf.FloorToInt((height- borderSize) / size);

        for (int k = 0; k < num; k++)
        {
            int end = ((height - borderSize) / num) * k + size;

            if (k == 0 || k == num-1)
                end = ((height - borderSize) / num) * k + size + borderSize;

            for (int i = ((height - borderSize) / num) * k; i < end; i++)
            {
                string str = " ";
                for (int j = 0; j < width; ++j)
                {
                    if (i < borderSize || i >= height - borderSize || j < borderSize || j >= width - borderSize)
                        pix[i * width + j] = borderColor;
                    else
                    {
                        if(k % 2 == 0)
                            pix[i * width + j] = col;
                        else
                            pix[i * width + j] = col2;
                    }

                    if (pix[i * width + j] == col)
                        str += "0 ";
                    else if (pix[i * width + j] == col2)
                        str += "1 ";
                    else
                        str += "2 ";
                }
             //   if(!l)
               //     Debug.Log(str);
            }
        }

        l = true;
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        textureListStyle = result;
        return result;
    }

    
}

class TestAciton
{
    static public void Action()
    {
        Debug.Log("oui");

    }
}




