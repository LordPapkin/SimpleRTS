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

    private PlayerInputHandler playerInputHandler;

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

    private void OnEnable()
    {
        playerInputHandler = new PlayerInputHandler();       
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
        
        targetOrthographicSize += -playerInputHandler.ScrollWheel().y * zoomAmount;
        
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);
        
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }

    private void HandleCameraMovement()
    {
        //take keyboard input
        moveDir = (Vector3)playerInputHandler.KeyboardMovementInput();

        //take mouse input
        if (isEdgeScrollingEnabled)
        {
            if (playerInputHandler.MousePosition().x > Screen.width - egdeScrollingSize)
            {
                moveDir.x = +1f;
            }
            if (playerInputHandler.MousePosition().x < egdeScrollingSize)
            {
                moveDir.x = -1f;
            }
            if (playerInputHandler.MousePosition().y > Screen.height - egdeScrollingSize)
            {
                moveDir.y = +1f;
            }
            if (playerInputHandler.MousePosition().y < egdeScrollingSize)
            {
                moveDir.y = -1f;
            }
        }
           
        moveDir.Normalize();
        zoomScale = orthographicSize / startOrthographicSize;
        
        targetTransformPosition = transform.position + (moveDir * moveSpeed * (zoomScale * moveSpeedOnZoomMultiplayer) * Time.deltaTime);
        if (Mathf.Abs(targetTransformPosition.x) > mapBoundX || Mathf.Abs(targetTransformPosition.y) > mapBoundY)
        {
            return;
        }
        transform.position = targetTransformPosition;
    }
}
