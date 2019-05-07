using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : RPGObjectInteractableComponent
{
    bool isOpen;

    public bool IsOpen { get => isOpen; set => isOpen = value; }

    static bool DoorIsOpen(Door door)
    {
        return door.IsOpen;
    }    
}
