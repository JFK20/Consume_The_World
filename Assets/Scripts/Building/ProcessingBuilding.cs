using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[Serializable]
public struct ItemSlotStruct {
    public Item item;
    public InventorySlot.IO io;
}

public class ProcessingBuilding : PlacedObject
{
    [SerializeField] private ItemSlotStruct[] inputItems;
    protected List<Tuple<int, InventorySlot.IO>> InputItems;
    
    [SerializeField] private ItemSlotStruct[] outputItems;
    protected List<Tuple<int, InventorySlot.IO>> OutputItems;
    
    [SerializeField] protected float timeToProcess;

    public void Awake()
    {
        InputItems = new List<Tuple<int, InventorySlot.IO>>();
        OutputItems = new List<Tuple<int, InventorySlot.IO>>();
        foreach (ItemSlotStruct itemSlotStruct in inputItems)
        {
            InputItems.Add(new Tuple<int, InventorySlot.IO>(itemSlotStruct.item.getId, itemSlotStruct.io));
        }
        foreach (ItemSlotStruct itemSlotStruct in outputItems)
        {
            OutputItems.Add(new Tuple<int, InventorySlot.IO>(itemSlotStruct.item.getId, itemSlotStruct.io));
        }
    }
}
