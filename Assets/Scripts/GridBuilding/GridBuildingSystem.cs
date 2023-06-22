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
    public List<PlacedObjectTypeSO> GetPlacedObjectSoList => placedObjectTypeSoList;

    private PlacedObjectTypeSO placedObjectTypeSo;
    public PlacedObjectTypeSO PlacedObjectTypeSo {
        get => placedObjectTypeSo;
        set => placedObjectTypeSo = value;
    }


    public GridXZ<GridObject> grid { get; private set; }
    private PlacedObjectTypeSO.Dir dir = PlacedObjectTypeSO.Dir.Down;
    public PlacedObjectTypeSO.Dir Dir {
        get => dir;
        set => dir = value;
    }
    

    private bool InventoryOpen;
    
    public void GetInventory() {
        InventoryManager.Instance.OpenInventory(grid.GetGridObject(Mouse3D.GetMouseWorldPosition()));
    }

    private void Awake() {
        Instance = this;
        
        int gridwidth = 11;
        int gridheight = 11;

        int cellSize = 10;
        grid = new GridXZ<GridObject>(gridwidth, gridheight, cellSize, Vector3.zero,
            (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));

        placedObjectTypeSo = placedObjectTypeSoList[0];
        GenerateFloor();
    }

    private void GenerateFloor() {
        GroundType[,] groundLocations = new GroundType[grid.GetWidth(),grid.GetHeight()];
        groundLocations[5, 5] = GroundType.Ore;
        for (int i = 0; i < grid.GetWidth(); i++) {
            for (int j = 0; j < grid.GetHeight(); j++) {
                GridObject gridObject = grid.GetGridObject(new Vector3(i * grid.GetCellSize(), 0, j * grid.GetCellSize()));
                GameObject floorObj = null;
                
                switch (groundLocations[i,j]) {
                    case GroundType.Ore:
                        gridObject.GroundType = GroundType.Ore;
                        floorObj = Resources.Load("Building/Floor/Ore") as GameObject;
                        break;
                    default:
                        gridObject.GroundType = GroundType.Grass;
                        floorObj = Resources.Load("Building/Floor/Grass") as GameObject;
                        break;
                }
                
                gridObject.Ground = Instantiate(floorObj, new Vector3((i + 0.5f) * grid.GetCellSize(),0,(j + 0.5f) * grid.GetCellSize()), Quaternion.identity, this.gameObject.transform );
            }
        }
    }
    
    /// <summary>
    /// Builds the Object with the Currently Selected Prefab at the MousePosition 
    /// </summary>
    public void Build() {
        if (placedObjectTypeSo == null) {
            return;
        }
        Vector3 pos = Mouse3D.GetMouseWorldPosition();
        grid.GetXZ(pos, out int x, out int z);

        List<Vector2Int> gridPositionList = placedObjectTypeSo.GetGridPositionList(new Vector2Int(x ,z), dir);

        bool freeSlot = true;
        GridObject onPositionObject = null;
        foreach (Vector2Int gridposition in gridPositionList) {
            onPositionObject = grid.GetGridObject(gridposition.x,gridposition.y);
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

            //Debug.Log("pos x:" + x + " pos z:" + z);
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

    /// <summary>
    /// Builds a Give PlacedObjectSoPrefab at given Location with given Rotation
    /// </summary>
    /// <param name="givenPlacedObjectTypeSo"> PlacedObjectTypeSo </param>
    /// <param name="x"> int </param>
    /// <param name="z"> int </param>
    /// <param name="rot"> Dir </param>
    /// <returns></returns>
    public PlacedObject Build(PlacedObjectTypeSO givenPlacedObjectTypeSo, int x, int z, PlacedObjectTypeSO.Dir rot) {
        if (givenPlacedObjectTypeSo == null) {
            return null;
        }

        dir = rot;

        List<Vector2Int> gridPositionList = givenPlacedObjectTypeSo.GetGridPositionList(new Vector2Int(x ,z), dir);

        bool freeSlot = true;
        foreach (Vector2Int gridposition in gridPositionList) {
            GridObject onPositionObject = grid.GetGridObject(gridposition.x,gridposition.y);
            if (onPositionObject == null) {
                return null;
            }
            if (!onPositionObject.CanBuild()) {
                //cannot build
                freeSlot = false;
                break;
            }
        }

        //GridObject gridObject = grid.GetGridObject(x, z);
        if (freeSlot) {
            Vector2Int rotationOffset = givenPlacedObjectTypeSo.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition =
                grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

            PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir,  givenPlacedObjectTypeSo);
            
            foreach (Vector2Int gridposition in gridPositionList) {
                grid.GetGridObject(gridposition.x, gridposition.y).SetPlacedObject(placedObject);
            }
            OnObjectPlaced?.Invoke(this, EventArgs.Empty);
            return placedObject;
        }
        else {
            Debug.Log("Already Occupied");
            return null;
        }
    }

    /*
    public void Demolish() {
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
    }*/
    
    public void Demolish(PlacedObject placedObject) {
        //gridObject.GetPlacedObject();
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
    
    public void DeselectObjectType() {
        placedObjectTypeSo = null; RefreshSelectedObjectType();
    }

    public void RefreshSelectedObjectType() {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public class GridObject {

        private GridXZ<GridObject> grid;
        private int x;
        private int z;

        private PlacedObject placedObject;

        private GameObject ground;

        public GameObject Ground {
            get => ground;
            set => ground = value;
        }
        
        private GroundType groundType;
        
        public GroundType GroundType {
            get => groundType;
            set => groundType = value;
        }
        
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
    
}
