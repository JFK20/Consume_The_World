using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreMiner : PlacedObject {
    private GroundType groundType;

    public static PlacedObject Create(Vector3 WorldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir,
        PlacedObjectTypeSO placedObjectTypeSO, GroundType groundType) {
        PlacedObject test = PlacedObject.Create(WorldPosition, origin, dir, placedObjectTypeSO);
        OreMiner oreMiner = test.GetComponent<OreMiner>();
        oreMiner.groundType = groundType;
        return test;
    }
}
