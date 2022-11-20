using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInit : MonoBehaviour
{
    private PlayerInputHandler playerInputHandler;

    private void OnEnable()
    {
        playerInputHandler = new PlayerInputHandler();
        playerInputHandler.Enable();
    }

    private void OnDisable()
    {
        playerInputHandler.Disable();
    }
}
