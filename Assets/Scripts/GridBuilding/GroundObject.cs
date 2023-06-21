using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundType {
    Standard,
    Ore,
    Grass
}

public class GroundObject : MonoBehaviour
{
    [SerializeField] private GroundType groundType;

    [SerializeField] private Plane plane;
    
    public GroundType GroundType => groundType;

    public Plane Plane => plane;

}
