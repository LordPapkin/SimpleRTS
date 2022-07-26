using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [Header("Camera Settings")]
    [SerializeField] float zoomAmount = 2f;
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float minOrthographicSize =10f;
    [SerializeField] float maxOrthographicSize =30f;    
    [SerializeField] float moveSpeed = 25f;

    private float orthographicSize;
    private float targetOrthographicSize;

    private void Start()
    {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
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
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(x, y).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
