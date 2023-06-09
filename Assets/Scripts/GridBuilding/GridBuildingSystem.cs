using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuildingSystem : MonoBehaviour {

    [SerializeField] private PlacedObjectTypeSO placedObjectTypeSo1;
    [SerializeField] private PlacedObjectTypeSO placedObjectTypeSo2;
    private GridXZ<GridObject> grid;

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
            Vector3 pos = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(pos, out int x, out int z);
            Vector3 posOfObj = grid.GetWorldPosition(x, z);

            GridObject gridObject = grid.GetGridObject(x, z);
            if (gridObject.CanBuild()) {
                Transform builtTransform = Instantiate(placedObjectTypeSo1.prefab, posOfObj, Quaternion.identity);
                gridObject.SetTransform(builtTransform);
            }
            else {
                Debug.Log("Already Occupied");
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            Vector3 pos = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(pos, out int x, out int z);

            List<Vector2Int> gridPositionList = placedObjectTypeSo2.GetGridPositionList(new Vector2Int(x ,z), PlacedObjectTypeSO.Dir.Down);

            bool freeSlot = true;
            foreach (Vector2Int gridposition in gridPositionList) {
                if (!grid.GetGridObject(gridposition.x,gridposition.y).CanBuild()) {
                    //cannot build
                    freeSlot = false;
                    break;
                }
            }
            
            Vector3 posOfObj = grid.GetWorldPosition(x, z);

            GridObject gridObject = grid.GetGridObject(x, z);
            if (freeSlot) {
                Transform builtTransform = Instantiate(placedObjectTypeSo2.prefab, posOfObj, Quaternion.identity);
                foreach (Vector2Int gridposition in gridPositionList) {
                    grid.GetGridObject(gridposition.x, gridposition.y).SetTransform(builtTransform);
                }
            }
            else {
                Debug.Log("Already Occupied");
            }
        }
    }
}
