using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public abstract class PlacedObject : SaveableObject {

    public static PlacedObject Create(Vector3 WorldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir,
        PlacedObjectTypeSO placedObjectTypeSO) {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab,
            WorldPosition,
            Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
        placedObject.inventorySlots = placedObjectTransform.gameObject.GetComponentsInChildren<InventorySlot>(includeInactive: true);
        placedObject.placedObjectTypeSO = placedObjectTypeSO;
        placedObject.origin = origin;
        placedObject.dir = dir;
        return placedObject;
    }
    
    
    protected PlacedObjectTypeSO placedObjectTypeSO;
    protected Vector2Int origin;
    protected PlacedObjectTypeSO.Dir dir;
    [SerializeField] protected Canvas inventory;
    protected InventorySlot[] inventorySlots = null;

    public InventorySlot[] GetInventorySlots => inventorySlots;
    public Canvas GetInventory => inventory;

    public List<Vector2Int> GetGridPositionList() {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }

    public void Demolish() {
        GridBuildingSystem.Instance.GetInventory();
        GridBuildingSystem.Instance.Demolish(this);
        Controls.Instance.inInventory = false;
    }

    public void DestroySelf() {
        Destroy(gameObject);
        //SaveGameManager.Instance.SaveableObjectList.Remove(this);
        DestroySaveable();
    }
    
    /// <summary>
    /// Gets the String and cuts it at * and parses the Data
    /// and Creates the Item 
    /// </summary>
    /// <param name="values"> string </param>
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
                int  id = int.Parse(data[1]);
                int count = int.Parse(data[2]);
                int slotNumber = int.Parse(data[0]);
                GameObject Debugobj = InventoryManager.Instance.inventoryItemPrefab;
                GameObject newItemGo = Instantiate(Debugobj, inventorySlots[slotNumber].transform);
                InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
                inventoryItem.InitialiseItem(SaveHelper.IntToItem(id), count);
            }
        }
        
    }

    public override string Save(int id) {
        string items = SaveHelper.SaveItems(ref inventorySlots);
        string pos = origin.ToString();
        string data = getObjectType().ToString() + "_" + placedObjectTypeSO.building.ToString() + "_" + pos + "_" + dir.ToString() + "_" + items;
        return data;
    }

    #region Items & Inventorys
    
    protected bool AddItem(int id ,InventorySlot.IO io) {
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
                    SpawnNewItem(id, slot);
                    return true;
                }
            }
        }
        return false;
    }
    
    private void SpawnNewItem(int id, InventorySlot slot) {
        GameObject newItemGo = Instantiate(InventoryManager.Instance.inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(ItemList.Instance.itemList[id]);
    }

    protected virtual IEnumerator ProcessItem(Item inputItem, InventorySlot.IO inputIO, int outputItem, InventorySlot.IO outputIO,int time) {
        return null;
    }

    protected bool RemoveItem(Item itemToRemove, InventorySlot.IO io) {
        if (inventorySlots.Length <= 0) {
            Debug.Log("no Slots");
            return false;
        }
        
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            if (slot.io == io) {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(includeInactive: true);
                if (itemInSlot != null && itemInSlot.item.getId == itemToRemove.getId &&
                    itemInSlot.count > 0) {
                    itemInSlot.count--;
                    if (itemInSlot.count > 0) {
                        itemInSlot.RefreshCount();
                    }
                    else {
                        Destroy(itemInSlot.gameObject);
                    }
                    return true;
                }
            }
        }
        return false;
    }

    protected bool FreeSlot(Item item, InventorySlot.IO io) {
        if (inventorySlots.Length <= 0) {
            Debug.Log("no Slots");
            return false;
        }

        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            if (slot.io == io) {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(includeInactive: true);
                if (itemInSlot == null || (itemInSlot.item.getId == item.getId &&
                                           itemInSlot.count < InventoryManager.Instance.maxStackedItem)) {
                    return true;
                }
            }
        }
        return false;
    }
    protected bool FreeSlot(int itemId, InventorySlot.IO io) {
        Item item = ItemList.Instance.itemList[itemId];
        return FreeSlot(item, io);
    }

    protected Item FindItem(Item item, InventorySlot.IO io) {
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i]; 
            if (slot.io == io) {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(includeInactive: true);
                if (itemInSlot != null && itemInSlot.item.getId == item.getId &&
                    itemInSlot.count > 0) {
                    return itemInSlot.item;
                }
            }
        }
        return null;
    }
    protected Item FindItem(int id, InventorySlot.IO io) {
        Item itemToFind = ItemList.Instance.itemList[id];
        return FindItem(itemToFind, io);
    }
    
    #endregion
    
}