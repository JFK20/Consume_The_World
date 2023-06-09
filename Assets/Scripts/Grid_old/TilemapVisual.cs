using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;

public class TilemapVisual : MonoBehaviour {
    
    [Serializable]
    public struct TilemapSpriteUV {
        public TilemapScript.TilemapObject.TilemapSprite tilemapSprite;
        public Vector2Int uv00Pixels;
        public Vector2Int uv11Pixels;
        
    }
    
    private struct UVCoords {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    [SerializeField] private TilemapSpriteUV[] tilemapSpriteUVArray;
    private GridScript<TilemapScript.TilemapObject> grid;
    private Mesh mesh;
    private bool updateMesh;
    private Dictionary<TilemapScript.TilemapObject.TilemapSprite, UVCoords> uvCoordsDictionary;

    private void Awake() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Texture texture = GetComponent<MeshRenderer>().material.mainTexture;
        float textureWidth = texture.width;
        float textureHeight = texture.height;

        uvCoordsDictionary = new Dictionary<TilemapScript.TilemapObject.TilemapSprite, UVCoords>();
        
        foreach (TilemapSpriteUV TilemapSpriteUV in tilemapSpriteUVArray) {
            uvCoordsDictionary[TilemapSpriteUV.tilemapSprite] = new UVCoords {
                uv00 = new Vector2(TilemapSpriteUV.uv00Pixels.x / textureWidth, TilemapSpriteUV.uv00Pixels.y / textureHeight),
                uv11 = new Vector2(TilemapSpriteUV.uv11Pixels.x / textureWidth, TilemapSpriteUV.uv11Pixels.y / textureHeight),
            };
        }
    }

    public void SetGrid(TilemapScript tilemapScript ,GridScript<TilemapScript.TilemapObject> grid) {
        this.grid = grid;
        UpdateHeatMapVisual();

        grid.OnGridObjectChanged += Grid_OnGridValueChanged;
        tilemapScript.OnLoaded += TilemapScript_OnLoaded;
    }

    private void TilemapScript_OnLoaded(object sender, EventArgs e) {
        updateMesh = true;
    }

    private void Grid_OnGridValueChanged(object sender, GridScript<TilemapScript.TilemapObject>.OnGridObjectChangedEventArgs e) {
        updateMesh = true;
    }

    private void LateUpdate() {
        if (updateMesh) {
            updateMesh = false;
            UpdateHeatMapVisual();
        }
    }

    private void UpdateHeatMapVisual() {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth() * grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                int index = x * grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * grid.GetCellSize();

                TilemapScript.TilemapObject gridObject = grid.GetGridObject(x, y);
                TilemapScript.TilemapObject.TilemapSprite tilemapSprite = gridObject.GetTilemapSprite();
                Vector2 gridValueUV00, gridValueUV11;

                if (tilemapSprite == TilemapScript.TilemapObject.TilemapSprite.None) {
                    gridValueUV00 = Vector2.zero;
                    gridValueUV11 = Vector2.zero;
                    quadSize = Vector3.zero;
                }
                else {
                    UVCoords uvCoords = uvCoordsDictionary[tilemapSprite];
                    gridValueUV00 = uvCoords.uv00;
                    gridValueUV11 = uvCoords.uv11;
                }
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x, y) + quadSize * .5f, 0f, quadSize, gridValueUV00, gridValueUV11);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

}
