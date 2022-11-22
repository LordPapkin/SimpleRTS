using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class MouseEnterExitEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event EventHandler MouseEnter;
    public event EventHandler MouseExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseEnter?.Invoke(this, EventArgs.Empty);
        Debug.Log("On");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseExit?.Invoke(this, EventArgs.Empty);
        Debug.Log("Off");
    }
}
