using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour {
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    public void PickupItem(int id) {
        bool result = inventoryManager.AddItem(itemsToPickup[id], InventorySlot.IO.PrimaryInput);
        Debug.Log(result);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            PickupItem(0);
        }
    }
}
