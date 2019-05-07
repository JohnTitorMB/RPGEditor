using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Globalization;

public class UtilityNode
{
    public static Type DrawObjectList(Parametre parametreObect, Node node)
    {
        List<Character> characters = RPGEditor.GetCharacgter();
        List<Item> items = RPGEditor.GetItem();
        List<ObjectInteractable> objects = RPGEditor.GetObjectInteractable();
        List<Quest> quests = RPGEditor.GetQuests();

        List<string> names = new List<string>(); ;

        for (int j = 0; j < characters.Count; j++)
            names.Add("Character/" + characters[j].name);

        for (int j = 0; j < items.Count; j++)
            names.Add("Item/" + items[j].name);

        for (int j = 0; j < objects.Count; j++)
            names.Add("Object/" + objects[j].name);

        for (int j = 0; j < quests.Count; j++)
            names.Add("Quests/" + quests[j].name);

        string[] namesArray = names.ToArray();

        parametreObect.selected = EditorGUILayout.Popup(parametreObect.selected, namesArray, GUILayout.Width(100));

        string nameObject = namesArray[parametreObect.selected].Split('/')[1];

        if (nameObject != parametreObect.parameterString)
            GUI.FocusControl("");

        parametreObect.parameterString = nameObject;

        Type type = typeof(object);

        if (parametreObect.selected < characters.Count)
            type = typeof(RPGCharacter);
        else if (parametreObect.selected < characters.Count + items.Count)
            type = typeof(RPGItem);
        else if (parametreObect.selected < characters.Count + items.Count + objects.Count)
        {
            int index = parametreObect.selected - (characters.Count + items.Count);
            type = GetType(objects[0].type);
        }

        else if (parametreObect.selected < characters.Count + items.Count + objects.Count + quests.Count)
        {
            type = typeof(QuestStatues);
        }

        return type;
    }

    public static Type GetType(string TypeName)
    {

        // Try Type.GetType() first. This will work with types defined
        // by the Mono runtime, in the same assembly as the caller, etc.
        var type = Type.GetType(TypeName);

        // If it worked, then we're done here
        if (type != null)
            return type;

        // If the TypeName is a full name, then we can try loading the defining assembly directly
        if (TypeName.Contains("."))
        {

            // Get the name of the assembly (Assumption is that we are using 
            // fully-qualified type names)
            var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));

            // Attempt to load the indicated Assembly
            var assembly = Assembly.Load(assemblyName);
            if (assembly == null)
                return null;

            // Ask that assembly to return the proper Type
            type = assembly.GetType(TypeName);
            if (type != null)
                return type;

        }

        // If we still haven't found the proper type, we can enumerate all of the 
        // loaded assemblies and see if any of them define the type
        var currentAssembly = Assembly.GetExecutingAssembly();
        var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
        foreach (var assemblyName in referencedAssemblies)
        {

            // Load the referenced assembly
            var assembly = Assembly.Load(assemblyName);
            if (assembly != null)
            {
                // See if that assembly defines the named type
                type = assembly.GetType(TypeName);
                if (type != null)
                    return type;
            }
        }

        // The type just couldn't be found...
        return null;

    }


    public static bool IsSupportedType(Type type)
    {
        Type[] RPGType = { typeof(int), typeof(float), typeof(bool), typeof(string), typeof(RPGCharacter), typeof(QuestStatues), typeof(RPGItem) };
        return Array.IndexOf(RPGType, type) != -1;
    }

    public static Parametre AddParameter(Type type)
    {
        if (IsSupportedType(type))
            return new Parametre(type);

        return null;
    }

    public static void DrawParameter(Parametre parametre, string parametreName)
    {
        GUILayout.Label(parametreName + " : ");
        Type parametreType = parametre.GetParameterType();

        if (parametreType == typeof(int))
            parametre.parameterInt = EditorGUILayout.IntField(parametre.parameterInt);
        else if (parametreType == typeof(bool))
            parametre.parameterBool = EditorGUILayout.Toggle(parametre.parameterBool);
        else if (parametreType == typeof(float))
            parametre.parameterfloat = EditorGUILayout.FloatField(parametre.parameterfloat);
        else if (parametreType == typeof(string))
            parametre.parameterString = EditorGUILayout.TextField(parametre.parameterString);
        else
        {
            List<string> names = new List<string>();

            if (parametreType == typeof(RPGCharacter))
            {
                List<Character> characters = RPGEditor.GetCharacgter();
                for (int j = 0; j < characters.Count; j++)
                    names.Add(characters[j].name);
            }

            else if (parametreType == typeof(QuestStatues))
            {
                List<Quest> quests = RPGEditor.GetQuests();
                for (int j = 0; j < quests.Count; j++)
                    names.Add(quests[j].name);
            }

            else if (parametreType == typeof(RPGItem))
            {
                List<Item> items = RPGEditor.GetItem();
                for (int j = 0; j < items.Count; j++)
                    names.Add(items[j].name);
            }

            if (names.Count > 0)
            {
                parametre.selected = EditorGUILayout.Popup(parametre.selected, names.ToArray(), GUILayout.Width(100));
                if (parametre.selected < names.Count)
                    parametre.parameterString = names[parametre.selected];
            }
        }
    }

    public static void DrawCallFunction(Type ObjectType, ref List<Parametre> parametres, int space, Node node, Type returnType)
    {
        ParameterInfo[] parametersInfo = null;

        Parametre pf = parametres[1];

        MethodInfo[] methodsInfo = ObjectType.GetMethods();
        List<MethodInfo> methodsInfoList = new List<MethodInfo>();
        List<string> names = new List<string>();

        for (int i = 0; i < methodsInfo.Length; i++)
        {
            parametersInfo = methodsInfo[i].GetParameters();

            bool SupoportedMethod = true;

            for (int j = 0; j < parametersInfo.Length; j++)
            {
                Type propertyType = parametersInfo[j].ParameterType;
                if (!IsSupportedType(propertyType))
                    SupoportedMethod = false;
            }

            if (SupoportedMethod && methodsInfo[i].ReturnType == returnType)
            {
                methodsInfoList.Add(methodsInfo[i]);
                names.Add(methodsInfo[i].Name);
            }
        }

        GUILayout.BeginHorizontal();

        GUILayout.Label("Method:");
        pf.selected = EditorGUILayout.Popup(pf.selected, names.ToArray(), EditorStyles.popup);

        GUILayout.EndHorizontal();

        if (pf.selected < methodsInfoList.Count && pf.selected >= 0)
        {
            MethodInfo methodInfo = methodsInfoList[pf.selected];
            parametersInfo = methodInfo.GetParameters();

            if (pf.parameterString != names[pf.selected])
            {
                GUI.FocusControl("");

                if (parametres.Count > 3)
                    parametres.RemoveRange(2, parametres.Count - 2);

                while (parametres.Count > 2)
                    parametres.Remove(parametres[parametres.Count - 1]);

                node.rect = node.saveRect;

                if (parametersInfo.Length > 2)
                    node.rect.height += (parametersInfo.Length - 2) * 20;

                for (int i = 0; i < parametersInfo.Length; i++)
                    parametres.Add(AddParameter(parametersInfo[i].ParameterType));
            }

            pf.parameterString = names[pf.selected];
        }

        GUILayout.Space(space);

        for (int i = 2; i < parametres.Count; i++)
        {
            GUILayout.BeginHorizontal();

            DrawParameter(parametres[i], parametersInfo[i - 2].Name);

            GUILayout.EndHorizontal();
        }
    }

    public static void DrawSetter(Type ObjectType, ref List<Parametre> parametres, Node node)
    {
        Parametre pf = parametres[1];

        PropertyInfo[] PropertiesInfo = ObjectType.GetProperties();
        List<PropertyInfo> PropertiesInfoList = new List<PropertyInfo>();
        List<string> names = new List<string>();
        for (int i = 0; i < PropertiesInfo.Length; i++)
        {
            TextInfo UsaTextInfo = new CultureInfo("en-US", false).TextInfo;
            //string capitalized = UsaTextInfo.ToTitleCase(PropertiesInfo[k].Name);
            if (IsSupportedType(PropertiesInfo[i].PropertyType))
            {
                PropertiesInfoList.Add(PropertiesInfo[i]);
                names.Add(PropertiesInfo[i].Name);
            }
        }

        if (PropertiesInfoList.Count == 0)
            return;

        GUILayout.BeginHorizontal();

        GUILayout.Label("Property:");
        pf.selected = EditorGUILayout.Popup(pf.selected, names.ToArray(), EditorStyles.popup);

        GUILayout.EndHorizontal();

        if (pf.selected < PropertiesInfoList.Count && pf.selected >= 0)
        {
            PropertyInfo info = PropertiesInfoList[pf.selected];

            if (pf.parameterString != names[pf.selected])
            {
                GUI.FocusControl("");
                if (parametres.Count > 2)
                    parametres.RemoveAt(2);

                Type type = info.PropertyType;

                Parametre parametre = AddParameter(type);
                if (parametre != null)
                    parametres.Add(parametre);
            }

            pf.parameterString = names[pf.selected];

            GUILayout.Space(40);

            GUILayout.BeginHorizontal();

            if (parametres.Count > 2)
                DrawParameter(parametres[2], info.Name);

            GUILayout.EndHorizontal();
        }
    }
}