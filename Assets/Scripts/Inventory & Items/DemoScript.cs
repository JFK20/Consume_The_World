using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour {
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    public void PickupItem(int id) {
        bool result = inventoryManager.AddItem(ItemList.Instance.itemList[1], InventorySlot.IO.PrimaryInput);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            PickupItem(0);
        }
    }
}
