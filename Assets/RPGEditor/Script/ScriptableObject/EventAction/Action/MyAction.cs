using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[System.Serializable]
public enum ActionEnum
{
    CALLFUNCTION = 0,
    SETTER
}

[System.Serializable]
public class MyAction : ScriptableObject
{
    public ActionEnum actionEnum;
    public void Init(ActionEnum _actionEnum)
    {
        ActionEnum = _actionEnum;
        parametres = new List<Parametre>();

        parametres.Add(new Parametre(typeof(string)));

        if (ActionEnum == ActionEnum.CALLFUNCTION)
            parametres.Add(new Parametre(typeof(string)));
        else
            parametres.Add(new Parametre(typeof(string)));
    }

    public MyAction nextAction = null;

    public List<Parametre> parametres;

    public string str = "bonjour";

    public ActionEnum ActionEnum { get => actionEnum; set => actionEnum = value; }

    public virtual void Update()
    {
       

        if (ActionEnum == ActionEnum.CALLFUNCTION)
            CallFunctionAction.Update(this);
        else
            SetterAction.Update(this);

    }
}





