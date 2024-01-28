using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    public int maxStackedItem = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    private Canvas currentInventory;
    private bool isInventoryOpen;

    [SerializeField] private GameObject playerInventory;
    [SerializeField] private Canvas playerCanvas;
    [SerializeField] private InventorySlot[] playerInventorySlots;
    private bool inPlayerInventory;

    private static InventoryManager instance;

    public static InventoryManager Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<InventoryManager>();
            }
            return instance;
        }
    }

    public void OpenInventory(GridBuildingSystem.GridObject gridObject) {
        if (!isInventoryOpen) {
                PlacedObject placedObject = gridObject.GetPlacedObject();
                if (placedObject == null) {
                    return;
                }
                currentInventory = placedObject.GetInventory;
                currentInventory.gameObject.SetActive(true);
                isInventoryOpen = true;
                inventorySlots = placedObject.GetInventorySlots;
        }
        else if(isInventoryOpen) {
            if (currentInventory) { currentInventory.gameObject.SetActive(false); }
            isInventoryOpen = false;
            currentInventory = null;
            inventorySlots = Array.Empty<InventorySlot>();
        }
    }


    public bool AddItem(Item item, InventorySlot.IO io) {
        if (currentInventory == null) {
            return false;
        }
        //finds an slot with same item
        if (item.stackable) {
            for (int i = 0; i < inventorySlots.Length; i++) {
                InventorySlot slot = inventorySlots[i];
                if (slot.io == io) {
                    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                    if (itemInSlot != null && itemInSlot.item.getId == item.getId && itemInSlot.count < maxStackedItem) {
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
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null) {
                    SpawnNewItem(item, slot);
                    return true;
                }
            }
        }
        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot slot) {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    public void OpenPlayerInventory() {
        inPlayerInventory = !inPlayerInventory;
        playerInventory.SetActive(inPlayerInventory);
    }

}
