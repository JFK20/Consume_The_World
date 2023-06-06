using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapScript {
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
    
    public class TilemapObject {
        public enum TilemapSprite {
            None, 
            Ground
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

        public override string ToString() {
            return tilemapSprite.ToString();
        }
    }
}
