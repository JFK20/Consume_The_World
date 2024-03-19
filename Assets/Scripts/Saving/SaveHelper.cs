using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class SaveHelper
{
    public static Item StringToItem(string item) {
        switch (item) {
            case "Test": return Resources.Load("Inventory/So/Test") as Item;
            case "IronOre": return Resources.Load("Inventory/So/Iron Ore") as Item;
            case "IronIngot": return Resources.Load("Inventory/So/Iron Ingot") as Item;
            default: return Resources.Load("Inventory/So/Test") as Item;
        }
    }
    
    /// <summary>
    /// cycles through all the slots and converts the Items to Strings
    /// </summary>
    /// <returns></returns>
    public static string SaveItems(ref InventorySlot[] slots) {
        string data = "";
        for (int i = 0; i < slots.Length; i++) {
            InventoryItem toSave = slots[i].GetComponentInChildren<InventoryItem>(includeInactive: true);
            if (toSave != null) {
                data += i.ToString() + ":";
                data += toSave.item.type.ToString()+ ":";
                data += toSave.count + "*";
            }
        }
        return data;
    }
    
    public static Vector3Int StringToIntVector3(string value) {
        value = value.Trim(new char[] {'(', ')'});
        value = value.Replace(" ", "");
        string[] pos = value.Split(",");
        int[] intpos = new int[3];
        for (int i = 0; i < 3; i++) {
            float tmp =  float.Parse(pos[i], CultureInfo.InvariantCulture);
            intpos[i] = Mathf.RoundToInt(tmp);
        }
        return new Vector3Int(intpos[0],intpos[1],intpos[2]);
    }
    
    /// <summary>
    /// Converts an String to an Vector2Int <br/>
    /// Can Convert if String Contains Data as Ints
    /// </summary>
    /// <param name="value"> String </param>
    /// <returns>
    ///  Vector2Int
    /// </returns>
    public static Vector2Int StringToIntVector2(string value) {
        value = value.Trim(new char[] {'(', ')'});
        value = value.Replace(" ", "");
        string[] pos = value.Split(",");
        int[] intpos = new int[2];
        for (int i = 0; i < 2; i++) {
            intpos[i] = int.Parse(pos[i], CultureInfo.InvariantCulture);
        }
        return new Vector2Int(intpos[0],intpos[1]);
    }
    
    public static Quaternion StringToQuaternion(string value) {
        value = value.Trim(new char[] {'(', ')'});
        value = value.Replace(" ", "");
        string[] pos = value.Split(",");
        
        return new Quaternion(float.Parse(pos[0], CultureInfo.InvariantCulture),float.Parse(pos[1], CultureInfo.InvariantCulture),float.Parse(pos[2], CultureInfo.InvariantCulture),float.Parse(pos[3], CultureInfo.InvariantCulture));
    }
    
    /// <summary>
    ///  Converts a String to a Dir
    /// </summary>
    /// <param name="rot"> string </param>
    /// <returns> Dir </returns>
    public static PlacedObjectTypeSO.Dir StringToDir(string rot) {
        switch (rot) {
            case "Down": return PlacedObjectTypeSO.Dir.Down;
            case "Up": return PlacedObjectTypeSO.Dir.Up;
            case "Left": return PlacedObjectTypeSO.Dir.Left;
            case "Right": return PlacedObjectTypeSO.Dir.Right;
            default: return PlacedObjectTypeSO.Dir.Down;
        }
    }
    
    public static GroundType StringToGroundType(string type) {
        switch (type) {
            case "Ore": return GroundType.TestOre;
            case "Grass": return GroundType.Grass;
            default: return GroundType.Standard;
        }
    }
}
