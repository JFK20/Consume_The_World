using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : PlacedObject
{
    private bool smelting = false;
    
    public new static PlacedObject Create(Vector3 WorldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir,
        PlacedObjectTypeSO placedObjectTypeSO) {
        PlacedObject tmp = PlacedObject.Create(WorldPosition, origin, dir, placedObjectTypeSO);
        Smelter smelter = tmp.GetComponent<Smelter>();
        return tmp;
    }
    
    public void Update() {
        if (!smelting) {
            Item item = FindItem(1, InventorySlot.IO.PrimaryInput);
            if (item != null) {
                StartCoroutine(ProcessItem(item,InventorySlot.IO.PrimaryInput,2,InventorySlot.IO.PrimaryOutput,3));
                smelting = true;
            }
            else {
                Wait(3);
            }
            
        }
        
    }

    protected override IEnumerator ProcessItem(Item inputItem, InventorySlot.IO inputIO, int outputItem, InventorySlot.IO outputIO,int time) {
         if (FreeSlot(ItemList.Instance.itemList[outputItem], outputIO)) {
             yield return new WaitForSeconds(time);
             RemoveItem(inputItem, inputIO);
             AddItem(outputItem, outputIO);
             smelting = false;
             yield return true;
         }
         else {
             yield return false;
         }
         yield break;
    }

    IEnumerator Wait(int time) {
        yield return new WaitForSeconds(time);
        yield break;
    }

}
