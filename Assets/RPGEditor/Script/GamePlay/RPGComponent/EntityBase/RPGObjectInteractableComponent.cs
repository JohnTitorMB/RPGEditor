using UnityEngine;

public class RPGObjectInteractableComponent : RPGComponent
{
    [SerializeField]
    ObjectInteractable objectInteractable = null;

    void Awake()
    {
        if (objectInteractable)
            RPGManager.Instanse.AddEntity(this, objectInteractable.name);
    }
}
