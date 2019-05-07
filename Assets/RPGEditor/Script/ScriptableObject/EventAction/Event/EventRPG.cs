using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[System.Serializable]
public class EventRPG : ScriptableObject
{   
    public EventRPG nextEvents = null;
    public MyAction action = null;
    
    public List<Parametre> parametres;

    public void Init()
    {
        parametres = new List<Parametre>();
        parametres.Add(new Parametre(typeof(string)));
        parametres.Add(new Parametre(typeof(string)));
    }

    public virtual void Update()
    {
        object entity = RPGManager.Instanse.GetEntity(parametres[0]);
        if(entity != null)
        {
            Type type = entity.GetType();
            MethodInfo methodInfo =  type.GetMethod(parametres[1].parameterString);
            if((bool)methodInfo.Invoke(entity, GetParametreArray(parametres)))
            {
                if (action != null)
                    action.Update();
            }
        }

        if (nextEvents != null)
            nextEvents.Update();
    }

    object[] GetParametreArray(List<Parametre> parametres)
    {
        List<object> objects = new List<object>();
        for(int i= 2; i< parametres.Count;i++)
        {
            if(parametres[i].GetParameterType() == typeof(int))
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







