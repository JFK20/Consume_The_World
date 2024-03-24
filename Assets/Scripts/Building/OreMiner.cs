using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreMiner : ProcessingBuilding {
    private bool mining = false;
    public static PlacedObject Create(Vector3 WorldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir,
        PlacedObjectTypeSO placedObjectTypeSO, GroundType groundType) {
        PlacedObject tmp = PlacedObject.Create(WorldPosition, origin, dir, placedObjectTypeSO);
        OreMiner oreMiner = tmp.GetComponent<OreMiner>();
        return tmp;
    }

    public void Update() {
        if (!mining) {
            StartCoroutine(MineOre(OutputItems[0].Item1, OutputItems[0].Item2, timeToProcess));
            mining = true;
        }
        else
        {
            StartCoroutine(Wait(timeToProcess));
        }
        
    }

    IEnumerator MineOre(int itemID, InventorySlot.IO io, float time) {
        bool suc = ItemHelper.Instance.AddItem(itemID, io, ref inventorySlots);
        yield return new WaitForSeconds(time);
        mining = false;
        yield return suc;
        yield break;
    }
    
    IEnumerator Wait(float time) {
        yield return new WaitForSeconds(time);
        yield break;
    }
}
