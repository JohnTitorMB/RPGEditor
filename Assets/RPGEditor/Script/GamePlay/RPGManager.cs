using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;



public class RPGManager : MonoBehaviour
{
    [SerializeField]
    EventActionList eventActionList;

    [SerializeField]
    RPGPlayerCharacter rPGPlayerCharacter;

    

    static RPGManager instanse = null;
    public static RPGManager Instanse
    {
        get
        {
            if (instanse == null)
                instanse = GameObject.Find("RPGManager").GetComponent<RPGManager>();

            return instanse;
        }
    }

    public RPGPlayerCharacter RPGPlayerCharacter { get => rPGPlayerCharacter; set => rPGPlayerCharacter = value; }
    public EventActionList EventActionList { get => eventActionList; set => eventActionList = value; }

    List<Tuple<string, object>> entities = new List<Tuple<string, object>>();

    void Awake()
    {
    }

    public void AddEntity(object entity,string str)
    {
        entities.Add(new Tuple<string, object>(str,entity));
    }

    public object GetEntity(Parametre name)
    {
        foreach(Tuple<string, object> tuple in entities)
        {
            if (tuple.Item1 == name.parameterString)
                return tuple.Item2;
        }

        return null;
    }
}
