using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PlacedObject : SaveableObject  {

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
                int count = int.Parse(data[2]);
                int slotNumber = int.Parse(data[0]);
                GameObject newItemGo = Instantiate(InventoryManager.Instance.inventoryItemPrefab, inventorySlots[slotNumber].transform);
                InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
                inventoryItem.InitialiseItem(StringToItem(data[1]), count);
            }
        }
        
    }

    public override string Save(int id) {
        string items = SaveItems();
        string pos = origin.ToString();
        string data = getObjectType().ToString() + "_" + placedObjectTypeSO.building.ToString() + "_" + pos + "_" + dir.ToString() + "_" + items;
        return data;
    }

    /// <summary>
    /// cycles through all the slots and converts the Items to Strings
    /// </summary>
    /// <returns></returns>
    protected string SaveItems() {
        string data = "";
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventoryItem toSave = inventorySlots[i].GetComponentInChildren<InventoryItem>(includeInactive: true);
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
                default: return Resources.Load("Inventory/So/Test") as Item;
        }
    }
}