public class WeaponItem : Item
{
    public override void Copy(Item item)
    {
        name = item.name;
        ItemIcon = item.ItemIcon;
        ItemDescription = item.ItemDescription;
        itemType = ItemType.Weapon;
    }

    public int Dammage;
    public int MPCost;
    public bool canBreak;
    public int HP;
    public int HpPerUsing;
    public int HpPerUsingRandom;

}

