using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceNearbyOverlay : MonoBehaviour
{
    private ResourceGeneratorData resourceGeneratorData;
    
    private void Update()
    {
        int nearbyResouceAmount = ResourceGenerator.GetNearbyResourceNodes(resourceGeneratorData, this.transform.parent.position);
        float percent = Mathf.RoundToInt((float)nearbyResouceAmount / resourceGeneratorData.maxResourceNodes * 100f);
        transform.Find("Text").GetComponent<TextMeshPro>().SetText(percent + "%");
    }
    public void Show(ResourceGeneratorData resourceGeneratorData)
    {
        this.resourceGeneratorData = resourceGeneratorData;
        this.gameObject.SetActive(true);

        transform.Find("Icon").GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;        
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
