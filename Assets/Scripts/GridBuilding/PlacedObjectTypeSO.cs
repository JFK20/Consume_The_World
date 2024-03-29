﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/PlacedObjectTypeSO")]
public class PlacedObjectTypeSO : ScriptableObject {

    public static Dir GetNextDir(Dir dir) {
        switch (dir) {
            default:
            case Dir.Down:      return Dir.Left;
            case Dir.Left:      return Dir.Up;
            case Dir.Up:        return Dir.Right;
            case Dir.Right:     return Dir.Down;
        }
    }

    public enum Dir {
        Down,
        Left,
        Up,
        Right,
    }

    public string nameString;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int height;
    public Building building;
    public GroundType[] suitableGrounds;


    public int GetRotationAngle(Dir dir) {
        switch (dir) {
            default:
            case Dir.Down:  return 0;
            case Dir.Left:  return 90;
            case Dir.Up:    return 180;
            case Dir.Right: return 270;
        }
    }

    public Dir RotationToDir(int rotation) {
        int rotnormalized = rotation % 360;
        switch (rotnormalized) {
            case 0: return Dir.Down;
            case 90: return Dir.Left;
            case 180: return Dir.Up;
            case 270: return Dir.Right;
            default: return Dir.Down;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir) {
        switch (dir) {
            default:
            case Dir.Down:  return new Vector2Int(0, 0);
            case Dir.Left:  return new Vector2Int(0, width);
            case Dir.Up:    return new Vector2Int(width, height);
            case Dir.Right: return new Vector2Int(height, 0);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir) {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir) {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++) {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < height; x++) {
                    for (int y = 0; y < width; y++) {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
        }
        return gridPositionList;
    }
    
    public enum Building {
        Lincoln,
        WhiteHouse,
    }

    /*public static GroundType[] AcceptedGrounds(Building building) {
        GroundType[] lincolnArray = new[] { GroundType.Ore };
        GroundType[] WhiteHouseArray = new[] { GroundType.Grass };
        switch (building) {
            case Building.Lincoln: return lincolnArray;
            case Building.WhiteHouse: return WhiteHouseArray;
            default: return Array.Empty<GroundType>();
        }
    }*/

}
