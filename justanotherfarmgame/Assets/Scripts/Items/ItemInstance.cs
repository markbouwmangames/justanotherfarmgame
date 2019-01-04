[System.Serializable]
public class ItemInstance {
    public Item Item;
    public int Quantity;

    public string Id {  get { return Item.Id; } }

    public ItemInstance(Item item, int quantity = 1) {
        Item = item;
        Quantity = quantity;
    }
}
