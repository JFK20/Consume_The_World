using System;
using System.Collections;
using System.Collections.Generic;
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
        throw new NotImplementedException();
    }

    public void Save() {
        
    }

    public void Load() {
        
    }

    public Vector3 StringToVector(string value) {
        return Vector3.zero;
    }

    public Quaternion StringToQuaternion(string value) {
        return Quaternion.identity;
    }
}
