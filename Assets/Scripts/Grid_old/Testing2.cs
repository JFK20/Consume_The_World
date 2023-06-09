using System;
using System.Collections;
using System.Collections.Generic;
using CodeMonkey;
using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Testing2 : MonoBehaviour {

    [SerializeField] private TilemapVisual tilemapVisual;
    private TilemapScript tilemap;
    private TilemapScript.TilemapObject.TilemapSprite tilemapSprite;
    
    // Start is called before the first frame update
    private void Start() { 
        tilemap = new TilemapScript(10, 10, 10f, new Vector3(-50,-50,0));

        tilemap.SetTilemapVisual(tilemapVisual);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            tilemap.SetTilemapSprite(position, tilemapSprite);
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            tilemapSprite = TilemapScript.TilemapObject.TilemapSprite.None;
            CMDebug.TextPopupMouse(tilemapSprite.ToString());
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            tilemapSprite = TilemapScript.TilemapObject.TilemapSprite.Ground;
            CMDebug.TextPopupMouse(tilemapSprite.ToString());
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            tilemapSprite = TilemapScript.TilemapObject.TilemapSprite.Path;
            CMDebug.TextPopupMouse(tilemapSprite.ToString());
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            tilemapSprite = TilemapScript.TilemapObject.TilemapSprite.Dirt;
            CMDebug.TextPopupMouse(tilemapSprite.ToString());
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            tilemap.Save();
            CMDebug.TextPopupMouse("Saved");
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            tilemap.Load();
            CMDebug.TextPopupMouse("Loaded");
        }
    }
}
