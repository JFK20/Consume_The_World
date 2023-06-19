using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler {
    public Image image;
    public Color selectedColor, notSelectedColor;
    public IO io;

    public enum IO {
        PrimaryInput,
        SecondaryInput,
        PrimaryOutput,
        SecondaryOutput,
    }
    
    private void Awake() {
        Deselect();
    }

    public void Deselect() {
        image.color = notSelectedColor;
    }
    public void Select() {
        image.color = selectedColor;
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0) {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
}
