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
