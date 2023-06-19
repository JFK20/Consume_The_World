using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType {
    Lincoln,
    WhiteHouse,
}

public abstract class SaveableObject : MonoBehaviour {
    protected string save;

    [SerializeField]
    private ObjectType objectType;

    public ObjectType getObjectType() {
        return objectType;
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        SaveGameManager.Instance.SaveableObjectList.Add(this);
    }

    public virtual string Save(int id) {
        return null;
    }

    public virtual void Load(string values) {
        
    }

    public void DestroySaveable() {
        SaveGameManager.Instance.SaveableObjectList.Remove(this);
    }
}
