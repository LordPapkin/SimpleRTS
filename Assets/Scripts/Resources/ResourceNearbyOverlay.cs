using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceNearbyOverlay : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private SpriteRenderer iconSpirteRenderer;

    private ResourceGeneratorData resourceGeneratorData;   

    private void Update()
    {
        int nearbyResouceAmount = ResourceGenerator.GetNearbyResourceNodes(resourceGeneratorData, this.transform.parent.position);
        float percent = Mathf.RoundToInt((float)nearbyResouceAmount / resourceGeneratorData.maxResourceNodes * 100f);
        text.SetText(percent + "%");
    }
    public void Show(ResourceGeneratorData resourceGeneratorData)
    {
        this.resourceGeneratorData = resourceGeneratorData;
        this.gameObject.SetActive(true);

        iconSpirteRenderer.sprite = resourceGeneratorData.resourceType.Sprite;        
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
