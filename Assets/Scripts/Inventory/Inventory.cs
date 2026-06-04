using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Inventory", fileName = "NewInventory")]
public class Inventory : ScriptableObject
{
    [SerializeField] List<InventorySlot> inventorySlot;

    public void AddItem(InventoryItem item)
    {
        // Find existing stack in the inventory
        InventorySlot existingSlot = inventorySlot.Find(slot => slot.inventoryItems == item);
        if (existingSlot != null)
        {
            existingSlot.itemQuantity++;

            // Fires the event to add item
            InventoryEvents.AddItem(item);              
            return;
        }

        // Finds an empty stack in an inventory to put the item
        // Works only if there is no available item in the inventory beforehand
        InventorySlot emptySlot = inventorySlot.Find(slot => slot.inventoryItems == null);
        if (emptySlot != null)
        {
            emptySlot.inventoryItems = item;
            emptySlot.itemQuantity = 1;

            InventoryEvents.AddItem(item);
            return;
        }



        InventoryEvents.InventoryFull();
        // Withouth LINQ (LINQ stands for Language Integrated Query)
        // for(int i = 0; i < inventorySlot.Count; i++)
        // {
        //     if(inventorySlot[i].inventoryItems == item)
        //     {
        //         inventorySlot[i].itemQuantity += 1;
        //         return;
        //     }
        // }
        // for(int i = 0; i < inventorySlot.Count; i++)
        // {
        //     if (inventorySlot[i].inventoryItems == null)
        //     {
        //         inventorySlot[i].inventoryItems = item;
        //         return;
        //     }
        // }
    }

    public void RemoveItem(InventoryItem item)
    {
        InventorySlot existingItem = inventorySlot.Find(slot => slot.inventoryItems == item);
        if (existingItem == null) return;


        if (existingItem.itemQuantity > 1)
        {
            existingItem.itemQuantity--;

            // Fires an event to remove item
            InventoryEvents.RemoveItem(item);
            return;
        }
        else
        {
            existingItem.inventoryItems = null;
            existingItem.itemQuantity = 0;  // reset quantity
            InventoryEvents.RemoveItem(item);
        }
    }
}
