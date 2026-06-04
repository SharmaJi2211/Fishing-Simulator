using System;

public static class InventoryEvents
{
    // Updates UI when something is added
    public static event Action<InventoryItem> OnItemAdded;

    public static void AddItem(InventoryItem item) => OnItemAdded?.Invoke(item);


    // Updates UI when something is removed
    public static event Action<InventoryItem> OnItemRemoved;
    
    public static void RemoveItem(InventoryItem item) => OnItemRemoved?.Invoke(item);

    // Shows Inventory full message 
    public static event Action OnInventoryFull;

    public static void InventoryFull() => OnInventoryFull?.Invoke();


    // Whne items quantity changes
    public static event Action<int, InventorySlot> OnItemQuantityChanged;

    public static void OnItemQtyChanged(int index, InventorySlot inventory) => OnItemQuantityChanged?.Invoke(index, inventory);

}
