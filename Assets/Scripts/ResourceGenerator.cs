using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private float timer;
    private float timerMax;

    private int nearbyResourceNodesAmount;

    
    private ResourceGeneratorData generatorData;
    private Collider2D[] collidersArray; 

    private void Awake()
    {
        generatorData = GetComponent<BuildingTypeHolder>().BuildingType.resourceGeneratorData;
        timerMax = generatorData.timerMax;
    }
    private void Start()
    {
        collidersArray = Physics2D.OverlapCircleAll(this.transform.position, generatorData.resourceDetectionRadius);
        foreach(Collider2D collider in collidersArray)
        {
            ResourceNode resourceNode = collider.GetComponent<ResourceNode>();
            if(resourceNode == null)
            {
                continue;
            }
            if(resourceNode.resourceType != generatorData.resourceType)
            {
                continue;
            }
            nearbyResourceNodesAmount++;
        }
        nearbyResourceNodesAmount = Mathf.Clamp(nearbyResourceNodesAmount, 0, generatorData.maxResourceNodes);
        Debug.Log(nearbyResourceNodesAmount);

        if(nearbyResourceNodesAmount == 0)
        {
            enabled = false;
        }
        else
        {
            timerMax = (generatorData.timerMax / 2f) + generatorData.timerMax * (1 - (float)nearbyResourceNodesAmount / generatorData.maxResourceNodes);
        }
        Debug.Log(timerMax);
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
