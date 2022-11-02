using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceNearbyOverlay : MonoBehaviour
{
    private ResourceGeneratorData resourceGeneratorData;
    private TextMeshPro text;
    private Sprite icon;

    private void Awake()
    {
        text = transform.Find("Text").GetComponent<TextMeshPro>();
        icon = transform.Find("Icon").GetComponent<SpriteRenderer>().sprite;
    }

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

        icon = resourceGeneratorData.resourceType.Sprite;        
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
