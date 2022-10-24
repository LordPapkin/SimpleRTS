using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler Instance => instance;
    private static CameraHandler instance;

    public bool IsEdgeScrollingEnabled => isEdgeScrollingEnabled;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [Header("Camera Settings")]
    [SerializeField] private float zoomAmount = 2f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minOrthographicSize =10f;
    [SerializeField] private float maxOrthographicSize =30f;    
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float moveSpeedOnZoomMultiplayer = 0.65f;
    [Header("Map Settings Settings")]
    [SerializeField] private float egdeScrollingSize = 50f;
    [SerializeField] private float mapBoundX = 150f;
    [SerializeField] private float mapBoundY = 150f;

    private Vector3 targetTransformPosition;
    private Vector3 moveDir = Vector3.zero;

    private float orthographicSize;
    private float targetOrthographicSize;
    private float startOrthographicSize;
    private float zoomScale;

    private bool isEdgeScrollingEnabled = false;

    public void ToggleEdgeScrolling(bool isEnabled)
    {
        isEdgeScrollingEnabled = isEnabled;
        PlayerPrefs.SetInt("edgeScrolling", isEdgeScrollingEnabled ? 1 : 0);
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        isEdgeScrollingEnabled = PlayerPrefs.GetInt("edgeScrolling", 0) == 1;
    }
    private void Start()
    {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
        startOrthographicSize = orthographicSize;
    }
    void Update()
    {
        HandleCameraMovement();
        HandleCameraZoom();
    }

    private void HandleCameraZoom()
    {
        
        targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;
        
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);
        
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }

    private void HandleCameraMovement()
    {
        //take keyboard input
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //take mouse input
        if (isEdgeScrollingEnabled)
        {
            if (Input.mousePosition.x > Screen.width - egdeScrollingSize)
            {
                x = +1f;
            }
            if (Input.mousePosition.x < egdeScrollingSize)
            {
                x = -1f;
            }
            if (Input.mousePosition.y > Screen.height - egdeScrollingSize)
            {
                y = +1f;
            }
            if (Input.mousePosition.y < egdeScrollingSize)
            {
                y = -1f;
            }
        }        

        moveDir.x = x;
        moveDir.y = y;
        moveDir.Normalize();
        zoomScale = orthographicSize / startOrthographicSize;

        targetTransformPosition = transform.position + (moveDir * moveSpeed * zoomScale * moveSpeedOnZoomMultiplayer * Time.deltaTime);
        if (Mathf.Abs(targetTransformPosition.x) > mapBoundX || Mathf.Abs(targetTransformPosition.y) > mapBoundY)
        {
            return;
        }
        transform.position = targetTransformPosition;
    }
}
