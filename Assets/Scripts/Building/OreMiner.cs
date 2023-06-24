using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreMiner : PlacedObject {
    private GroundType groundType;
    [SerializeField] private Item[] itemsToMine;
    private bool mining = false;
    public static PlacedObject Create(Vector3 WorldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir,
        PlacedObjectTypeSO placedObjectTypeSO, GroundType groundType) {
        PlacedObject test = PlacedObject.Create(WorldPosition, origin, dir, placedObjectTypeSO);
        OreMiner oreMiner = test.GetComponent<OreMiner>();
        oreMiner.groundType = groundType;
        return test;
    }

    public void Update() {
        if (!mining) {
            StartCoroutine(MineOre(3));
            mining = true;
        }
        
    }

    IEnumerator MineOre(int time) {
        Item item = itemsToMine[0];
        bool suc = AddItem(item, InventorySlot.IO.PrimaryOutput);
        yield return new WaitForSeconds(time);
        mining = false;
        yield return suc;
        yield break;
    }
}
