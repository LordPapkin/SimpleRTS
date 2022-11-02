using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public static int GetNearbyResourceNodes(ResourceGeneratorData resourceGeneratorData, Vector3 position)
    {
        Collider2D[] collidersArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);
        int nearbyResourceNodesAmount = 0;

        foreach (Collider2D collider in collidersArray)
        {
            ResourceNode resourceNode = collider.GetComponent<ResourceNode>();
            if (resourceNode == null)
            {
                continue;
            }
            if (resourceNode.resourceType != resourceGeneratorData.resourceType)
            {
                continue;
            }
            nearbyResourceNodesAmount++;
        }
        nearbyResourceNodesAmount = Mathf.Clamp(nearbyResourceNodesAmount, 0, resourceGeneratorData.maxResourceNodes);

        return nearbyResourceNodesAmount;
    }

    private float timer;
    private float timerMax;
    
    private ResourceGeneratorData generatorData;
     

    public ResourceGeneratorData GetResourceGeneratorData()
    {
        return generatorData;
    }
    public float GetTimerNormalized()
    {
        return timer/timerMax;
    }
    public float GetAmountGeneratedPerSecond()
    {
        return 1f / timerMax;
    }

    private void Awake()
    {
        generatorData = GetComponent<BuildingTypeHolder>().BuildingType.ResourceGeneratorData;
        timerMax = generatorData.timerMax;
    }
    private void Start()
    {
        int nearbyResourceNodesAmount = GetNearbyResourceNodes(generatorData, this.transform.position);
        if (nearbyResourceNodesAmount == 0)
        {
            enabled = false;
        }
        else
        {
            timerMax = generatorData.timerMax + (generatorData.timerMax * (1f - ((float)nearbyResourceNodesAmount/(float)generatorData.maxResourceNodes)));
        }
    }    

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0f)
        {
            timer = timerMax;            
            ResourceManager.Instance.AddResource(generatorData.resourceType, 1);
        }
    }
}
