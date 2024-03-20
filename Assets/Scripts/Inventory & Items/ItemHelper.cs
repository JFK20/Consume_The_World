using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHelper : MonoBehaviour
{
    private static ItemHelper instance;

    public static ItemHelper Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<ItemHelper>();
            }
            return instance;
        }
    }
    
    public void SpawnNewItem(int id, ref InventorySlot slot) {
        GameObject newItemGo = Instantiate(InventoryManager.Instance.inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(ItemList.Instance.itemList[id]);
    }
    
    public bool AddItem(int id ,InventorySlot.IO io, ref InventorySlot[] inventorySlots) {
        if (inventorySlots.Length <= 0) {
            Debug.Log("no Slots");
            return false;
        }

        //finds an slot with same item
        if (ItemList.Instance.itemList[id].stackable) {
            for (int i = 0; i < inventorySlots.Length; i++) {
                InventorySlot slot = inventorySlots[i];
                if (slot.io == io) {
                    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(includeInactive: true);
                    if (itemInSlot != null && itemInSlot.item.getId == id &&
                        itemInSlot.count < InventoryManager.Instance.maxStackedItem) {
                        itemInSlot.count++;
                        itemInSlot.RefreshCount();
                        return true;
                    }
                }
            }
        }
        //finds an empty slot
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            if (slot.io == io) { 
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(includeInactive: true);
                if (itemInSlot == null) {
                    SpawnNewItem(id, ref slot);
                    return true;
                }
            }
        }
        return false;
    }
}
