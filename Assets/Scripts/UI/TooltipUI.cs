using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public class TooltipTimer
    {
        public float timer;
    }

    public static TooltipUI Instance => instance;
    [SerializeField] private Vector2 padding;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private RectTransform backgroundReactTransform;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform canvasRectTransform;

    private static TooltipUI instance;    
    private TooltipTimer timer;


    public void Show(string tooltipText, TooltipTimer tooltipTimer = null)
    {
        this.timer = tooltipTimer;
        gameObject.SetActive(true);
        SetText(tooltipText);    
        UpdateToolTipPosition();
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Update()
    {
        UpdateToolTipPosition();
        HandleTimerCountDown();
    }

    private void HandleTimerCountDown()
    {
        if (timer != null)
        {
            timer.timer -= Time.deltaTime;
            if (timer.timer < 0)
            {
                Hide();
            }
        }
    }

    private void UpdateToolTipPosition()
    {
        Vector2 anchoredPosition = Utilities.GetMousePosition() / canvasRectTransform.localScale.x;

        if (anchoredPosition.x + backgroundReactTransform.rect.width > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundReactTransform.rect.width;
        }
        if (anchoredPosition.x < 0)
        {
            anchoredPosition.x = 0;
        }
        if (anchoredPosition.y + backgroundReactTransform.rect.height > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundReactTransform.rect.height;
        }
        if (anchoredPosition.y < 0)
        {
            anchoredPosition.y = 0;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);       
        backgroundReactTransform.sizeDelta = textSize + padding;
    }
}