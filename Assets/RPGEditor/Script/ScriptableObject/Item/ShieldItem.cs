public class ShieldItem : Item
{
    public override void Copy(Item item)
    {
        name = item.name;
        ItemIcon = item.ItemIcon;
        ItemDescription = item.ItemDescription;
        itemType = ItemType.Shield;

    }

    public int resistence;
    public int resistenceMagic;
    public bool canBreak;
    public int HP;
}