using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : SaveableObject  {

    public static PlacedObject Create(Vector3 WorldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir,
        PlacedObjectTypeSO placedObjectTypeSO) {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab,
            WorldPosition,
            Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
        placedObject.placedObjectTypeSO = placedObjectTypeSO;
        placedObject.origin = origin;
        placedObject.dir = dir;
        
        return placedObject;
    }
    
    
    private PlacedObjectTypeSO placedObjectTypeSO;
    private Vector2Int origin;
    private PlacedObjectTypeSO.Dir dir;
    [SerializeField] private Canvas inventory;
    
    public Canvas GetInventory => inventory;

    public List<Vector2Int> GetGridPositionList() {
        return placedObjectTypeSO.GetGridPositionList(origin, dir);
    }

    public void DestroySelf() {
        Destroy(gameObject);
        //SaveGameManager.Instance.SaveableObjectList.Remove(this);
        DestroySaveable();
    }
    
    public override void Load(string[] values) {
        // Load Values For Inventory
    }

    public override string Save(int id) {
        string pos = origin.ToString();
        string data = getObjectType().ToString() + "_" + pos + "_" + dir.ToString();
        return data;
    }
}