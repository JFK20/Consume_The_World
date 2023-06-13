using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuildingSystem : MonoBehaviour {

    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSoList;
    private PlacedObjectTypeSO placedObjectTypeSo;
    
    private GridXZ<GridObject> grid;
    private PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Down;

    private void Awake() {
        int gridwidth = 10;
        int gridheight = 10;

        float cellSize = 10f;
        grid = new GridXZ<GridObject>(gridwidth, gridheight, cellSize, Vector3.zero,
            (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));

        placedObjectTypeSo = placedObjectTypeSoList[0];
    }
    
    public class GridObject {

        private GridXZ<GridObject> grid;
        private int x;
        private int z;

        private PlacedObject placedObject;

        public GridObject(GridXZ<GridObject> grid, int x, int z) {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }
        
        public PlacedObject GetPlacedObject() {
            return this.placedObject;
        }

        public void SetPlacedObject(PlacedObject placedObject) {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, z);
        }

        public void ClearPlacedObject() {
            this.placedObject = null;
            grid.TriggerGridObjectChanged(x, z);
        }

        public bool CanBuild() {
            return this.placedObject == null;
        }

        public override string ToString() {
            return x + "," + z + "\n" + placedObject;
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Build();
        }if (Input.GetMouseButtonDown(1)) {
            Demolish();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            placedObjectTypeSo = placedObjectTypeSoList[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            placedObjectTypeSo = placedObjectTypeSoList[1];
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }
    }

    private void Build() {
        Vector3 pos = Mouse3D.GetMouseWorldPosition();
        grid.GetXZ(pos, out int x, out int z);

        List<Vector2Int> gridPositionList = placedObjectTypeSo.GetGridPositionList(new Vector2Int(x ,z), dir);

        bool freeSlot = true;
        foreach (Vector2Int gridposition in gridPositionList) {
            if (!grid.GetGridObject(gridposition.x,gridposition.y).CanBuild()) {
                //cannot build
                freeSlot = false;
                break;
            }
        }

        GridObject gridObject = grid.GetGridObject(x, z);
        if (freeSlot) {
            Vector2Int rotationOffset = placedObjectTypeSo.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition =
                grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

            PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir,  placedObjectTypeSo);
            
            foreach (Vector2Int gridposition in gridPositionList) {
                grid.GetGridObject(gridposition.x, gridposition.y).SetPlacedObject(placedObject);
            }
        }
        else {
            Debug.Log("Already Occupied");
        }
    }

    private void Demolish() {
        GridObject gridObject = grid.GetGridObject(Mouse3D.GetMouseWorldPosition());
        PlacedObject placedObject = gridObject.GetPlacedObject();
        if (placedObject == null) {
            return;
        }
        placedObject.DestroySelf();
        List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
        
        foreach (Vector2Int gridposition in gridPositionList) {
            grid.GetGridObject(gridposition.x, gridposition.y).ClearPlacedObject();
        }
        
    }
}
