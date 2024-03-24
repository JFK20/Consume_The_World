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
            StartCoroutine(MineOre());
            mining = true;
        }
        
    }

    IEnumerator MineOre() {
        bool suc = ItemHelper.Instance.AddItem(OutputItems[0].Item1, OutputItems[0].Item2, ref inventorySlots);
        yield return new WaitForSeconds(timeToProcess);
        mining = false;
        yield return suc;
        yield break;
    }
}
