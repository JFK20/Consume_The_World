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
}
