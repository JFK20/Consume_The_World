using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class Testing : MonoBehaviour {
    [SerializeField] private HeatMapVisual heatMapVisual;
    [SerializeField] private HeatMapBoolVisual heatMapBoolVisual;
    //[SerializeField] private HeatMapGenericVisual heatMapGenericVisual;
    private GridScript<HeatMapGridObject> grid;
    private GridScript<bool> boolGrid;
    private GridScript<StringGridObject> stringGrid;

    private void Start() { 
        //grid = new GridScript<HeatMapGridObject>(20, 10, 8f, Vector3.zero, (GridScript<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));
        stringGrid =  new GridScript<StringGridObject>(20, 10, 8f, Vector3.zero, (GridScript<StringGridObject> g, int x, int y) => new StringGridObject(g, x, y));
        
    }

    private void Update() {
        Vector3 position = UtilsClass.GetMouseWorldPosition();

        /*if (Input.GetMouseButtonDown(0)) {
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(position);
            if (heatMapGridObject != null) {
                heatMapGridObject.AddValue(5);
            }
        }*/
        if (Input.anyKeyDown) {
            String e = Input.inputString;
            if (e.Length == 1 ) {
                if(char.IsLetter(e[0])) { stringGrid.GetGridObject(position).AddLetter(e); }
                if(char.IsDigit(e[0])) {stringGrid.GetGridObject(position).AddNumber(e);}
            }
        }
       
    }
}

public class HeatMapGridObject {

    private GridScript<HeatMapGridObject> grid;
    private int value;
    private int x;
    private int y;

    public HeatMapGridObject(GridScript<HeatMapGridObject> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    private const int MIN = 0;
    private const int MAX = 100;
    
    public void AddValue(int addValue) {
        value += addValue;
        Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x,y);
    }

    public float GetValueNormalized() {
        return (float)value / MAX;
    }

    public override string ToString() {
        return value.ToString();
    }
}

public class StringGridObject {

    private GridScript<StringGridObject> grid;
    private int value;
    private int x;
    private int y;

    public StringGridObject(GridScript<StringGridObject> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
        letters = "";
        numbers = "";
    }

    private string letters;
    private string numbers;
    
    public void AddLetter(string letter) {
        letters += letter;
        grid.TriggerGridObjectChanged(x,y);
    }
    
    public void AddNumber(string number) {
        numbers += number;
        grid.TriggerGridObjectChanged(x,y);
    }

    public override string ToString() {
        return letters + "\n" + numbers;
    }
}
