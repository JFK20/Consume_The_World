using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ObjectType {
    Lincoln,
    WhiteHouse,
}

public abstract class SaveableObject : MonoBehaviour {
    protected string save;
    
    private ObjectType objectType;
    
    // Start is called before the first frame update
    private void Start()
    {
        SaveGameManager.Instance.SaveableObjectList.Add(this);
    }

    public virtual void Save(int id) {
        PlayerPrefs.SetString(id.ToString(), transform.position.ToString());
    }

    public virtual void Load(string values) {
        
    }

    public  void DestroySaveable() {
        
    }

    public void OnDestroy() {
        SaveGameManager.Instance.SaveableObjectList.Remove(this);
    }
}
