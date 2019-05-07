using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGItem : RPGComponent
{
    [SerializeField]
    Item item = null;

    void Awake()
    {
        if (item)
            RPGManager.Instanse.AddEntity(this, item.name);
    }
}

