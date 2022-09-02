using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;

    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;
    private ResourceTypeListSO resourceTypeList;    

    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceAmountDictionary[resourceType];
    }
    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] += amount;
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }
    public bool CanAfford(ResourceAmount[] resourceCostAmountArray)
    {
        foreach(ResourceAmount resourceCostAmount in resourceCostAmountArray)
        {
            if(resourceCostAmount.amount > GetResourceAmount(resourceCostAmount.resourceType))
            {
                return false;
            }
        }
        return true;
    }
    public void SpendResources(ResourceAmount[] resourceCostAmountArray)
    {
        foreach (ResourceAmount resourceCostAmount in resourceCostAmountArray)
        {
            resourceAmountDictionary[resourceCostAmount.resourceType] -= resourceCostAmount.amount;
        }
    }

    private void Awake()
    {
        Instance = this;
        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        foreach(ResourceTypeSO resourceType in resourceTypeList.list)
        {
            resourceAmountDictionary[resourceType] = 0;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.J))
        {            
            TestLogResourceAmountDictionary();
        }
    }

    private void TestLogResourceAmountDictionary()
    {
        foreach(ResourceTypeSO resourceType in resourceAmountDictionary.Keys)
        {
            Debug.Log(resourceType.nameString + " : " + resourceAmountDictionary[resourceType]);
        }
    }

    
}
