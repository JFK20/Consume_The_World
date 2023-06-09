using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapScript {
    
    public event EventHandler OnLoaded;
    private GridScript<TilemapObject> grid;

    public TilemapScript(int width, int height, float cellSize, Vector3 originPosition) {
        grid = new GridScript<TilemapObject>(width, height, cellSize, originPosition, (GridScript<TilemapObject> g, int x, int y) => new TilemapObject(g,x,y));
        
    }

    public void SetTilemapSprite(Vector3 worldPosition, TilemapObject.TilemapSprite tilemapSprite) {
        TilemapObject tilemapObject = grid.GetGridObject(worldPosition);
        if (tilemapObject != null) {
            tilemapObject.SetTilemapSprite(tilemapSprite);
        }
    }
    
    public void SetTilemapVisual(TilemapVisual tilemapVisual) {
        tilemapVisual.SetGrid(this ,grid);
    }

    // Saving and loading
    
    public void Save() {
        List<TilemapObject.SaveObjectTmO> saveObjectList = new List<TilemapObject.SaveObjectTmO>();
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                TilemapObject tilemapObject = grid.GetGridObject(x, y);
                saveObjectList.Add(tilemapObject.Save());
            }
        }

        SaveObject saveObject = new SaveObject { SaveObjectTmOArray = saveObjectList.ToArray() };
        
        SaveSystem.SaveObject(saveObject);
    }

    public void Load() {
        SaveObject saveObject = SaveSystem.LoadMostRecentObject<SaveObject>();
        foreach (TilemapObject.SaveObjectTmO SOTmO in saveObject.SaveObjectTmOArray) {
            TilemapObject tilemapObject = grid.GetGridObject(SOTmO.x, SOTmO.y);
            tilemapObject.Load(SOTmO);
        }

        OnLoaded?.Invoke(this, EventArgs.Empty);
    }

    public class SaveObject {
        public TilemapObject.SaveObjectTmO[] SaveObjectTmOArray;
        
    }
    
    public class TilemapObject {
        public enum TilemapSprite {
            None, 
            Ground,
            Path,
            Dirt,
        }

        private GridScript<TilemapObject> grid;
        private TilemapSprite tilemapSprite;
        private int x;
        private int y;

        public TilemapObject(GridScript<TilemapObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetTilemapSprite(TilemapSprite tilemapSprite) {
            this.tilemapSprite = tilemapSprite;
            this.grid.TriggerGridObjectChanged(x,y);
        }

        public TilemapSprite GetTilemapSprite() {
            return tilemapSprite;
        }

        public override string ToString() {
            return tilemapSprite.ToString();
        }
        
        [Serializable]
        public class SaveObjectTmO {
            public TilemapSprite tilemapSprite;
            public int x;
            public int y;
        }

        public SaveObjectTmO Save() {
            return new SaveObjectTmO {
                tilemapSprite = tilemapSprite,
                x = x,
                y = y,
            };
        }

        public void Load(SaveObjectTmO saveObjectTmO) {
            tilemapSprite = saveObjectTmO.tilemapSprite;
        }
    }
}
