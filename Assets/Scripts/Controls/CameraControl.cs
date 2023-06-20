using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private bool useEdgeScrolling = true;
    [SerializeField] private bool useDragPen = true;
    [SerializeField] private float moveSpeed = 100f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float dragPanSpeed = 1f;
    [SerializeField] private float zoomSped = 5f;
    [SerializeField] private float minZoom = 1f;
    [SerializeField] private float maxZoom = 50f;

    private bool dragPanMoveActive;
    private Vector2 lastMousePosition;
    private Vector3 followOffset;

    public static CameraControl Instance { get; private set; }

    private void Awake() {
        Instance = this;
        
        followOffset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }

    public void UpdateCamera() {
        HandleCameraRotation();
        if(useEdgeScrolling) HandleCameraMovementEdgeScrolling();
        if(useDragPen) HandleCameraMovementDragPen();
        HandleCameraMovement();
        HandleCameraZoom();
    }
    
    /*void Update() {
        HandleCameraRotation();
        if(useEdgeScrolling) HandleCameraMovementEdgeScrolling();
        if(useDragPen) HandleCameraMovementDragPen();
        HandleCameraMovement();
        HandleCameraZoom();
    }*/
    
    private void HandleCameraMovement() {
        Vector3 inputDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;
        
        
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        transform.position += moveSpeed * Time.deltaTime * moveDir;
    }

    private void HandleCameraRotation() {
        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q)) rotateDir =- 1f;
        if (Input.GetKey(KeyCode.E)) rotateDir =+ 1f;

        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
    }

    private void HandleCameraMovementEdgeScrolling() {
        Vector3 inputDir = new Vector3(0, 0, 0);
        
        int edgeScrollSize = 20;
        
        if (Input.mousePosition.x < edgeScrollSize) {
            inputDir.x = -1f;
        }
        if (Input.mousePosition.y < edgeScrollSize) {
            inputDir.z = -1f;
        }
        if (Input.mousePosition.x > Screen.width - edgeScrollSize) {
            inputDir.x = 1f;
        }
        if (Input.mousePosition.y > Screen.height - edgeScrollSize) { 
                inputDir.z = +1f;
        }

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        transform.position += moveSpeed * Time.deltaTime * moveDir;
    }
    
    private void HandleCameraMovementDragPen() {
        if (Input.GetMouseButtonDown(1)) {
            dragPanMoveActive = true;
            lastMousePosition = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(1)) {
            dragPanMoveActive = false;
        }

        if (dragPanMoveActive) {
            Vector3 inputDir = new Vector3(0, 0, 0);
            
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;
            
            inputDir.x = -mouseMovementDelta.x * dragPanSpeed;
            inputDir.z = -mouseMovementDelta.y * dragPanSpeed;

            lastMousePosition = Input.mousePosition;
            
            Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

            transform.position += moveSpeed * Time.deltaTime * moveDir;
        }
    }

    private void HandleCameraZoom() {
        Vector3 zoomDir = followOffset.normalized;
        if (Input.mouseScrollDelta.y > 0) {
            followOffset -= zoomDir;
        }
        if (Input.mouseScrollDelta.y < 0) {
            followOffset += zoomDir;
        }

        if (followOffset.magnitude < minZoom) {
            followOffset = zoomDir * minZoom;
        }
        if (followOffset.magnitude > maxZoom) {
            followOffset = zoomDir * maxZoom;
        }
        
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset,
            Time.deltaTime * zoomSped);

    }
    
}
