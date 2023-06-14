using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridBuildingSystem : MonoBehaviour {
    
    public static GridBuildingSystem Instance { get; private set; }
    
    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSoList;
    private PlacedObjectTypeSO placedObjectTypeSo;
    
    private GridXZ<GridObject> grid;
    private PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Down;

    private void Awake() {
        Instance = this;
        
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
        }
        if (Input.GetMouseButtonDown(1)) {
            Demolish();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            DeselectObjectType();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            placedObjectTypeSo = placedObjectTypeSoList[0];
            RefreshSelectedObjectType();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            placedObjectTypeSo = placedObjectTypeSoList[1];
            RefreshSelectedObjectType();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            dir = PlacedObjectTypeSO.GetNextDir(dir);
        }
    }

    private void Build() {
        if (placedObjectTypeSo == null) {
            return;
        }
        Vector3 pos = Mouse3D.GetMouseWorldPosition();
        grid.GetXZ(pos, out int x, out int z);

        List<Vector2Int> gridPositionList = placedObjectTypeSo.GetGridPositionList(new Vector2Int(x ,z), dir);

        bool freeSlot = true;
        foreach (Vector2Int gridposition in gridPositionList) {
            GridObject onPositionObject = grid.GetGridObject(gridposition.x,gridposition.y);
            if (onPositionObject == null) {
                return;
            }
            if (!onPositionObject.CanBuild()) {
                //cannot build
                freeSlot = false;
                break;
            }
        }

        //GridObject gridObject = grid.GetGridObject(x, z);
        if (freeSlot) {
            Vector2Int rotationOffset = placedObjectTypeSo.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition =
                grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

            PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir,  placedObjectTypeSo);
            
            foreach (Vector2Int gridposition in gridPositionList) {
                grid.GetGridObject(gridposition.x, gridposition.y).SetPlacedObject(placedObject);
            }
            OnObjectPlaced?.Invoke(this, EventArgs.Empty);
        }
        else {
            Debug.Log("Already Occupied");
        }
    }

    private void Demolish() {
        GridObject gridObject = grid.GetGridObject(Mouse3D.GetMouseWorldPosition());
        if (gridObject == null) {
            return;
        }
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
    
    public Vector3 GetMouseWorldSnappedPosition() {
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        grid.GetXZ(mousePosition, out int x, out int z);

        if (placedObjectTypeSo != null) {
            Vector2Int rotationOffset = placedObjectTypeSo.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        } else {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation() {
        if (placedObjectTypeSo != null) {
            return Quaternion.Euler(0, placedObjectTypeSo.GetRotationAngle(dir), 0);
        } else {
            return Quaternion.identity;
        }
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return placedObjectTypeSo;
    }
    
    private void DeselectObjectType() {
        placedObjectTypeSo = null; RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType() {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }
}
