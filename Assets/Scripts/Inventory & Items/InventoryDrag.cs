using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Random Stolen Script from the Internet that moves a Canvas; <br/>
/// https://gyanendushekhar.com/2019/11/11/move-canvas-ui-mouse-drag-unity-3d-drag-drop-ui/
/// </summary>
public class InventoryDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler 
{
    
    private Vector2 lastMousePosition;
 
    /// <summary>
    /// This method will be called on the start of the mouse drag
    /// </summary>
    /// <param name="eventData">mouse pointer event data</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Begin Drag");
        lastMousePosition = eventData.position;
    }
 
    /// <summary>
    /// This method will be called during the mouse drag
    /// </summary>
    /// <param name="eventData">mouse pointer event data</param>
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = eventData.position;
        Vector2 diff = currentMousePosition - lastMousePosition;
        RectTransform rect = GetComponent<RectTransform>();
        
 
        Vector3 newPosition = rect.position +  new Vector3(diff.x, diff.y, 0);
        Vector3 oldPos = rect.position;
        rect.position = newPosition;
        if(!IsRectTransformInsideSreen(rect))
        {
            rect.position = oldPos;
        }
        lastMousePosition = currentMousePosition;
    }
 
    /// <summary>
    /// This method will be called at the end of mouse drag
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        //sdDebug.Log("End Drag");
        //Implement further funtionlity here
    }
 
    /// <summary>
    /// This methods will check is the rect transform is inside the screen or not
    /// </summary>
    /// <param name="rectTransform">Rect Trasform</param>
    /// <returns></returns>
    private bool IsRectTransformInsideSreen(RectTransform rectTransform)
    {
        bool isInside = false;
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        int visibleCorners = 0;
        Rect rect = new Rect(0,0,Screen.width, Screen.height);
        foreach(Vector3 corner in corners)
        {
            if(rect.Contains(corner))
            {
                visibleCorners++;
            }
        }
        if(visibleCorners == 4)
        {
            isInside = true;
        }
        return isInside;
    }
}
