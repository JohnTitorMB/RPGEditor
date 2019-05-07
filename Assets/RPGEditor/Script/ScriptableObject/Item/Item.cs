using UnityEngine;

[System.Serializable]
public class Item : EntityData
{
    public enum ItemUsing
    {
        Player,
        Ennemy,
        Both,
    }

    public enum ItemType
    {
        Generic,
        Weapon,
        Shield,
        Potion,
    }

    public Item()
    {
    }

    public virtual void Copy(Item item)
    {
    }

    public string ItemDescription;
    public Sprite ItemIcon;
    public ItemType itemType;
    public ItemUsing itemUsing;
    public int NmbCharacter;
    public int HpPerSeconde;
    public int MpPerSecond;
}
