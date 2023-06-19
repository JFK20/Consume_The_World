using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SaveGameManager : MonoBehaviour {

    private static SaveGameManager instance;

    public static SaveGameManager Instance {
        get {
            if (instance == null) {
                instance = GameObject.FindObjectOfType<SaveGameManager>();
            }
            return instance;
        }
    }

    private List<SaveableObject> saveableObjectList;
    
    public List<SaveableObject> SaveableObjectList { get; private set; }

    private void Awake() {
        SaveableObjectList = new List<SaveableObject>();
    }

    /// <summary>
    /// goes through all Saveable Items and calling save on them <br/>
    /// the Return string is given to the Save System which writes it to the File
    /// </summary>
    public void Save() {
        int savedObjectsAmount = SaveableObjectList.Count;
        string[] objs = new string[savedObjectsAmount];
        
        for (int i = 0; i < savedObjectsAmount ; i++) {
            objs[i] = SaveableObjectList[i].Save(i);
        }

        string everything = savedObjectsAmount + "\n";
        foreach (string data in objs) {
            everything += data + "\n";
        }
        SaveSystem.Save("test",everything,true);
    }
    
    /// <summary>
    /// First up deletes all Saveable Objects on the Grid <br/>
    /// Then the File is read and the String is Split at _ <br/>
    /// The String Data is read and the Object is created and Load is called on the Object 
    /// </summary>
    public void Load() {
        foreach (SaveableObject obj in SaveableObjectList) {
            if (obj != null) {
               PlacedObject placedObject = obj.gameObject.GetComponent<PlacedObject>();
               List<Vector2Int> posList = placedObject.GetGridPositionList();
               foreach (Vector2Int pos in posList) {
                   GridBuildingSystem.Instance.grid.GetGridObject(pos.x, pos.y).ClearPlacedObject();
               } 
               Destroy(obj.gameObject);
            }
        }
        
        SaveableObjectList.Clear();
        
        
        string loadedData = SaveSystem.Load("test");
        string[] row = loadedData.Split("\n");
        for (int i = 0; i < row.Length; i++) {
            row[i] = row[i].Replace("\n", "");
            row[i] = row[i].Trim();
        }
        int objectCount = int.Parse(row[0].Trim());
        //Debug.Log(row[0] + "\n" + row[1]);

        for (int i = 1; i <= objectCount; i++) {
            //Value 0 object Type 
            //Value 1 Position
            //Value2 Dir
            //Value3 Items
            string[] value = row[i].Split("_");
            
            Vector2Int pos = StringToIntVector2(value[1]);

            PlacedObjectTypeSO.Dir rot = StringToDir(value[2]);

            PlacedObject tmp = null;
            
            switch (value[0]) {
                case "Lincoln":
                    tmp = GridBuildingSystem.Instance.Build(Resources.Load("Building/SO/LinconSo") as PlacedObjectTypeSO, pos.x, pos.y, rot);
                    break;
                case "WhiteHouse":
                    tmp = GridBuildingSystem.Instance.Build(Resources.Load("Building/SO/WhiteHouse") as PlacedObjectTypeSO, pos.x, pos.y, rot);
                    break;
            }

            if (tmp != null) {
                // Implement Later for Inventory
                tmp.Load(value[3]);
            }
        }
    }

    public Vector3Int StringToIntVector3(string value) {
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
    public Vector2Int StringToIntVector2(string value) {
        value = value.Trim(new char[] {'(', ')'});
        value = value.Replace(" ", "");
        string[] pos = value.Split(",");
        int[] intpos = new int[2];
        for (int i = 0; i < 2; i++) {
            intpos[i] = int.Parse(pos[i], CultureInfo.InvariantCulture);
        }
        return new Vector2Int(intpos[0],intpos[1]);
    }

    public Quaternion StringToQuaternion(string value) {
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
    public PlacedObjectTypeSO.Dir StringToDir(string rot) {
        switch (rot) {
            case "Down": return PlacedObjectTypeSO.Dir.Down;
            case "Up": return PlacedObjectTypeSO.Dir.Up;
            case "Left": return PlacedObjectTypeSO.Dir.Left;
            case "Right": return PlacedObjectTypeSO.Dir.Right;
            default: return PlacedObjectTypeSO.Dir.Down;
        }
    }
}
