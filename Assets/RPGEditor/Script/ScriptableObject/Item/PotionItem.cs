public class PotionItem : Item
{
    public enum PotionItemType
    {
        HP,
        MP,

    }

    public override void Copy(Item item)
    {
        name = item.name;
        ItemIcon = item.ItemIcon;
        ItemDescription = item.ItemDescription;
        itemType = ItemType.Potion;

    }

    public PotionItemType potionItemType;
    public int HP = 5;
    public bool poisonous;
    public int MP = 5;
}