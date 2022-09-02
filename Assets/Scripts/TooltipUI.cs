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

    private static TooltipUI instance;
    private TextMeshProUGUI textMeshPro;
    private RectTransform backgroundReactTransform;
    private RectTransform rectTransform;
    private RectTransform canvasRectTransform;
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

        textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        backgroundReactTransform = transform.Find("Background").GetComponent<RectTransform>();
        canvasRectTransform = transform.parent.GetComponent<RectTransform>();
        rectTransform = GetComponent<RectTransform>();       

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
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

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