using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
    [SerializeField] private GridBuildingSystem gridBuildingSystem;

    [SerializeField] private KeyCode rotation;
    [SerializeField] private KeyCode openInventory;
    [SerializeField] private KeyCode quickSave;
    [SerializeField] private KeyCode quickLoad;
    private bool inInventory;


    // Update is called once per frame
    void Update()
    {
        if (!inInventory) {
            if (Input.GetMouseButtonDown(0)) {
                gridBuildingSystem.Build();
            }
            /*if (Input.GetMouseButtonDown(1)) {
                gridBuildingSystem.Demolish();
            }*/
            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                gridBuildingSystem.DeselectObjectType();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                gridBuildingSystem.PlacedObjectTypeSo = gridBuildingSystem.GetPlacedObjectSoList[0]; // getter
                gridBuildingSystem.RefreshSelectedObjectType();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                gridBuildingSystem.PlacedObjectTypeSo = gridBuildingSystem.GetPlacedObjectSoList[1];
                gridBuildingSystem.RefreshSelectedObjectType();
            }
            if (Input.GetKeyDown(rotation)) {
                gridBuildingSystem.Dir = PlacedObjectTypeSO.GetNextDir(gridBuildingSystem.Dir); //getter
            }
            if (Input.GetKeyDown(quickSave)) {
                SaveGameManager.Instance.Save();
                Debug.Log("Saved");
            }
            if (Input.GetKeyDown(quickLoad)) {
                SaveGameManager.Instance.Load();
                Debug.Log("Loaded");
            }
            CameraControl.Instance.UpdateCamera();
        }
        if (Input.GetKeyDown(openInventory)) {
            gridBuildingSystem.GetInventory();
            inInventory = !inInventory;
        }
    }
}
