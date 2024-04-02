using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : ProcessingBuilding
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
            Item item = FindItem(InputItems[0].Item1, InputItems[0].Item2);
            if (item != null) {
                StartCoroutine(ProcessItem(InputItems[0].Item1,InputItems[0].Item2,OutputItems[0].Item1,OutputItems[0].Item2, timeToProcess));
                smelting = true;
            }
            else {
                StartCoroutine(Wait(timeToProcess));
            }
            
        }
        
    }

    protected override IEnumerator ProcessItem(int inputItemId, InventorySlot.IO inputIO, int outputItemId, InventorySlot.IO outputIO,float time) {
         if (FreeSlot(outputItemId, outputIO)) {
             yield return new WaitForSeconds(time);
             RemoveItem(ItemList.Instance.itemList[inputItemId], inputIO);
             ItemHelper.Instance.AddItem(outputItemId, outputIO, ref inventorySlots);
             smelting = false;
             yield return true;
         }
         else {
             yield return false;
         }
         yield break;
    }

    IEnumerator Wait(float time) {
        yield return new WaitForSeconds(time);
        yield break;
    }

}
