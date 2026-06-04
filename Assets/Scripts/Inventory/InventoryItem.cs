using UnityEngine;

public enum ItemType
{
    FishingRod, 
    Bait, 
    Fish, 
    Quest, 
    Consumable
}

[CreateAssetMenu(menuName = "Inventory/Item", fileName = "NewItem")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    public float itemPrice;
    public bool isUsable;
}


