using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
    public static Controls Instance { get; private set; }
    
    [SerializeField] private GridBuildingSystem gridBuildingSystem;

    [SerializeField] private KeyCode rotation;
    [SerializeField] private KeyCode openInventory;
    [SerializeField] private KeyCode quickSave;
    [SerializeField] private KeyCode quickLoad;
    [SerializeField] private KeyCode buildMode;
    [SerializeField] private KeyCode openPlayerInventory;
    [SerializeField] private GameObject buildPreview;
    public bool inInventory;
    private bool inBuildMode;
    private bool inPlayerInventory;


    public void Awake() {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inInventory && !inPlayerInventory) {
            if (Input.GetKeyDown(buildMode)) {
                inBuildMode = !inBuildMode;
                buildPreview.gameObject .SetActive(inBuildMode);
            }
            if (Input.GetMouseButtonDown(0) && inBuildMode) {
                gridBuildingSystem.Build();
            }
            /*if (Input.GetMouseButtonDown(1)) {
                gridBuildingSystem.Demolish();
            }*/
            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                gridBuildingSystem.DeselectObjectType();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                gridBuildingSystem.placedObjectTypeSo = gridBuildingSystem.GetPlacedObjectSoList[0]; // getter
                gridBuildingSystem.RefreshSelectedObjectType();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                gridBuildingSystem.placedObjectTypeSo = gridBuildingSystem.GetPlacedObjectSoList[1];
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
        else if (inPlayerInventory || inInventory) {
            inBuildMode = false;
            buildPreview.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(openInventory)) {
            gridBuildingSystem.GetInventory();
            inInventory = !inInventory;
        }
        if (Input.GetKeyDown(openPlayerInventory)) {
            InventoryManager.Instance.OpenPlayerInventory();
            inPlayerInventory = !inPlayerInventory;
        }
    }
}
