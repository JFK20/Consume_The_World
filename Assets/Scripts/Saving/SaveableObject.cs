using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ObjectType {
    Lincoln,
    WhiteHouse,
}

public class SaveableObject : MonoBehaviour {
    protected string save;
    private ObjectType objectType;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    public virtual void Save(int id) {
        
    }

    public virtual void Load(string[] values) {
        
    }

    public  void DestroySaveable() {
        
    }
}
