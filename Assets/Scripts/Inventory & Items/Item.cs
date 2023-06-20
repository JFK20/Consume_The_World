using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "SO/Items")]
public class Item : ScriptableObject {

    [Header("Only gameplay")]
    //public TileBase tile; //OnMap change to 3D
    //public GameObject prefab;
    public ItemType type; // ItemType
    //public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;
    
    [Header("Both")]
    public Sprite image; //Sprite in inventory

    [SerializeField] 
    private int id = -1;

    public int getId => id;
}

public enum ItemType {
    IronOre,
    Tool,
    Weapon,
    Test,
}