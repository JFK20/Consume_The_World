using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundType {
    Standard,
    Ore,
    Grass
}

public class GroundObject : SaveableObject
{
    [SerializeField] private GroundType groundType;

    public GroundType GroundType {
        get => groundType;
        set => groundType = value;
    }

    public int x { get; set; }
    
    public int y { get; set; }

    public static GameObject Create(GameObject obj, Vector3Int pos, Quaternion quaternion, Transform transform, GroundType groundType) {
        GameObject tmp = Instantiate(obj, new Vector3((pos.x + 0.5f) * GridBuildingSystem.Instance.grid.GetCellSize(),0,(pos.z + 0.5f) * GridBuildingSystem.Instance.grid.GetCellSize()), quaternion, transform );
        GroundObject groundObject = tmp.GetComponent<GroundObject>();
        groundObject.x = pos.x;
        groundObject.y = pos.z;
        groundObject.groundType = groundType;
        return tmp;
    }

    public override string Save(int id) {
        string pos = new Vector2Int(x,y).ToString();
        string data = getObjectType().ToString() + "_" + groundType.ToString() + "_" + pos ;
        return data;
    }
}

public struct GroundStruct {
    public int x { get; }
    
    public int y { get; }

    private GroundType groundType;

    public GroundType GroundType {
        get => groundType;
        set => groundType = value;
    }

    public GroundStruct(GroundType groundType, int x, int y) {
        this.groundType = groundType;
        this.x = x;
        this.y = y;
    }
}
