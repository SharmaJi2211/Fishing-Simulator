public class InventorySlot
{
    public InventoryItem inventoryItems;
    public int itemQuantity;

    public InventorySlot(InventoryItem item, int quantity)
    {
        this.inventoryItems = item;
        this.itemQuantity = quantity;
    }
}
