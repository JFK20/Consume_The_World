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

    public void Save() {
        int savedObjectsAmount = SaveableObjectList.Count;
        PlayerPrefs.SetInt("ObjectCount" , savedObjectsAmount);
        
        for (int i = 0; i < savedObjectsAmount ; i++) {
            SaveableObjectList[i].Save(i);
        }
    }

    public void Load() {
        int objectCount = PlayerPrefs.GetInt("ObjectCount");

        for (int i = 0; i < objectCount; i++) {
            //work around
            string value = PlayerPrefs.GetString(i.ToString());
            Vector3Int pos = StringToIntVector(value);
            int cellSize = GridBuildingSystem.Instance.grid.GetCellSize();
            pos = new Vector3Int(pos.x / cellSize,pos.y / cellSize,pos.z / cellSize);
            Debug.Log(pos);
            PlacedObject placedObject = GridBuildingSystem.Instance.Build(Resources.Load("Building/SO/LinconSo") as PlacedObjectTypeSO, pos.x, pos.z);
        }
    }

    public Vector3Int StringToIntVector(string value) {
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

    public Quaternion StringToQuaternion(string value) {
        return Quaternion.identity;
    }
}
