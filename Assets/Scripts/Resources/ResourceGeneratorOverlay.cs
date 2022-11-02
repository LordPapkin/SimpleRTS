using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceGeneratorOverlay : MonoBehaviour
{
    [SerializeField] private ResourceGenerator resourceGenerator;
    [SerializeField] private Transform barGameObject;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshPro textMeshPro;

    private void Start()
    {
        ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();
        spriteRenderer.sprite = resourceGeneratorData.ResourceType.Sprite;
        textMeshPro.SetText(resourceGenerator.GetAmountPerSecond().ToString("F1"));        
    }

    private void Update()
    {
        barGameObject.transform.localScale = new Vector3(1f - resourceGenerator.GetTimerNormalized(), 1f, 1f);
    }
}
