using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreMiner : PlacedObject {
    private bool mining = false;
    public static PlacedObject Create(Vector3 WorldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir,
        PlacedObjectTypeSO placedObjectTypeSO, GroundType groundType) {
        PlacedObject tmp = PlacedObject.Create(WorldPosition, origin, dir, placedObjectTypeSO);
        OreMiner oreMiner = tmp.GetComponent<OreMiner>();
        return tmp;
    }

    public void Update() {
        if (!mining) {
            StartCoroutine(MineOre(3));
            mining = true;
        }
        
    }

    IEnumerator MineOre(int time) {
        bool suc = ItemHelper.Instance.AddItem(1, InventorySlot.IO.PrimaryOutput, ref inventorySlots);
        yield return new WaitForSeconds(time);
        mining = false;
        yield return suc;
        yield break;
    }
}
