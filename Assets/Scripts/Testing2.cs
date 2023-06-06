using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Testing2 : MonoBehaviour {
    private TilemapScript tilemap;
    
    // Start is called before the first frame update
    private void Start() { 
        tilemap = new TilemapScript(10, 10, 10f, Vector3.zero);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            tilemap.SetTilemapSprite(position, TilemapScript.TilemapObject.TilemapSprite.Ground);
        }
    }
}
