using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CallFunctionAction
{
    public static void Update(MyAction action)
    {
        Debug.Log("Param 0 : " + action.parametres[0].parameterString);
        Debug.Log("Param 1 : " + action.parametres[1].parameterString);

        object entity = RPGManager.Instanse.GetEntity(action.parametres[0]);
        if (entity != null)
        {
            Type type = entity.GetType();
            MethodInfo methodInfo = type.GetMethod((string)action.parametres[1].parameterString);
            methodInfo.Invoke(entity, GetParametreArray(action.parametres));
        }

        else
        {
            Debug.Log("Entity NULL");
        }

        if (action.nextAction != null)
            action.nextAction.Update();
    }

    static object[] GetParametreArray(List<Parametre> parametres)
    {
        List<object> objects = new List<object>();
        for (int i = 2; i < parametres.Count; i++)
        {
            if (parametres[i].GetParameterType() == typeof(int))
                objects.Add(parametres[i].parameterInt);
            else if (parametres[i].GetParameterType() == typeof(bool))
                objects.Add(parametres[i].parameterBool);
            else if (parametres[i].GetParameterType() == typeof(string))
                objects.Add(parametres[i].parameterString);
            else if (parametres[i].GetParameterType() == typeof(float))
                objects.Add(parametres[i].parameterfloat);
            else
            {
                object entity = RPGManager.Instanse.GetEntity(parametres[i]);
                objects.Add(entity);
            }
        }

        return objects.ToArray();
    }
}