using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuildingSystem : MonoBehaviour {

    [SerializeField] private PlacedObjectTypeSO[] placedObjectTypeSoArray;
    private GridXZ<GridObject> grid;
    private PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Down;

    private void Awake() {
        int gridwidth = 10;
        int gridheight = 10;

        float cellSize = 10f;
        grid = new GridXZ<GridObject>(gridwidth, gridheight, cellSize, Vector3.zero,
            (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));

    }
    
    public class GridObject {

        private GridXZ<GridObject> grid;
        private int x;
        private int z;

        private Transform transform;

        public GridObject(GridXZ<GridObject> grid, int x, int z) {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }
        
        public Transform GetTransform() {
            return this.transform;
        }

        public void SetTransform(Transform Tstransform) {
            this.transform = Tstransform;
            grid.TriggerGridObjectChanged(x, z);
        }

        public void ClearTransform() {
            this.transform = null;
            grid.TriggerGridObjectChanged(x, z);
        }

        public bool CanBuild() {
            return this.transform == null;
        }

        public override string ToString() {
            return x + "," + z + "\n" + transform;
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Build(0);
        }
        if (Input.GetMouseButtonDown(1)) {
            Build(1);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
            Debug.Log(dir);
        }
    }

    private void Build(int number) {
        Vector3 pos = Mouse3D.GetMouseWorldPosition();
        grid.GetXZ(pos, out int x, out int z);

        List<Vector2Int> gridPositionList = placedObjectTypeSoArray[number].GetGridPositionList(new Vector2Int(x ,z), dir);

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
            Vector2Int rotationOffset = placedObjectTypeSoArray[number].GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition =
                grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            
            Transform builtTransform = Instantiate(placedObjectTypeSoArray[number].prefab,
                placedObjectWorldPosition,
                Quaternion.Euler(0, placedObjectTypeSoArray[number].GetRotationAngle(dir), 0));
            foreach (Vector2Int gridposition in gridPositionList) {
                grid.GetGridObject(gridposition.x, gridposition.y).SetTransform(builtTransform);
            }
        }
        else {
            Debug.Log("Already Occupied");
        }
    }
}
