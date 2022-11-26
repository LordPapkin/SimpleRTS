using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler
{
    private static PlayerInput input = new PlayerInput();

    public void Enable()
    {
        input.Player.Move.Enable();
        input.Player.MousePositon.Enable();
        input.Player.PlaceBuildingClick.Enable();
        input.Player.CancelBuildingClick.Enable();
        input.Player.ScrollWheel.Enable();
        input.Player.OptionMenu.Enable();
    }

    public void Disable()
    {
        input.Player.Move.Disable();
        input.Player.MousePositon.Disable();
        input.Player.PlaceBuildingClick.Disable();
        input.Player.CancelBuildingClick.Disable();
        input.Player.ScrollWheel.Disable(); 
        input.Player.OptionMenu.Disable();
    }

    public event Action<InputAction.CallbackContext> PlaceBuildingClick
    {
        add { input.Player.PlaceBuildingClick.performed += value; }
        remove { input.Player.PlaceBuildingClick.performed -= value; }
    }

    public event Action<InputAction.CallbackContext> CancelBuildingClick
    {
        add { input.Player.CancelBuildingClick.performed += value; }
        remove { input.Player.CancelBuildingClick.performed -= value; }
    }

    public event Action<InputAction.CallbackContext> OptionsMenu
    {
        add { input.Player.OptionMenu.performed += value; }
        remove { input.Player.OptionMenu.performed-= value; }
    }

    public Vector2 KeyboardMovementInput()
    {
        return input.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 MousePosition()
    {
        return input.Player.MousePositon.ReadValue<Vector2>();
    }

    public Vector2 ScrollWheel()
    {
        return input.Player.ScrollWheel.ReadValue<Vector2>();   
    }
}
