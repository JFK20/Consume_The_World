using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : SaveableObject {
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

    #region save & loading
    
    public override void Load(string values) {
    // Load Values For Inventory
    string[] items = values.Split("*");
    //data 0 slotnumber
    //data 1 item
    //data 2 count
    foreach (string item in items) {
        if (!item.Equals("")) {
            //Debug.Log(item);
            string[] data = item.Split(":");
            //Debug.Log(data[0] + "," + data[1] + "," + data[2]);
            int count = int.Parse(data[2]);
            int slotNumber = int.Parse(data[0]);
            GameObject newItemGo = Instantiate(inventoryItemPrefab, playerInventorySlots[slotNumber].transform);
            InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
            inventoryItem.InitialiseItem(StringToItem(data[1]), count);
        }
    }
    
    }

    public override string Save(int id) {
        string items = SaveItems();
        string data = "PlayerInventory" + "_" + items;
        return data;
    }

    /// <summary>
    /// cycles through all the slots and converts the Items to Strings
    /// </summary>
    /// <returns></returns>
    protected string SaveItems() {
        string data = "";
        for (int i = 0; i < playerInventorySlots.Length; i++) {
            InventoryItem toSave = playerInventorySlots[i].GetComponentInChildren<InventoryItem>(includeInactive: true);
            if (toSave != null) {
                data += i.ToString() + ":";
                data += toSave.item.type.ToString()+ ":";
                data += toSave.count + "*";
            }
        }
        return data;
    }

    protected Item StringToItem(string item) {
        switch (item) {
                case "Test": return Resources.Load("Inventory/So/Test") as Item;
                case "IronOre": return Resources.Load("Inventory/So/Iron Ore") as Item;
                case "IronIngot": return Resources.Load("Inventory/So/Iron Ingot") as Item;
                default: return Resources.Load("Inventory/So/Test") as Item;
        }
    }
    
    #endregion
    
}
