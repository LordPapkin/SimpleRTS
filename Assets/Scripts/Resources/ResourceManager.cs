using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;

    [SerializeField] private List<ResourceAmount> startingResourceAmountList;
    [SerializeField] private ResourceTypeSO repairResource;
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

    public bool CanAffordRepair(int amount)
    {
        if (amount > GetResourceAmount(repairResource))
        { 
            return false;
        }
        return true;
    }

    public void SpendRepairCost(int amount)
    {
        resourceAmountDictionary[repairResource] -= amount;
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
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
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

        foreach(ResourceAmount resourceAmount in startingResourceAmountList)
        {
            AddResource(resourceAmount.resourceType, resourceAmount.amount);
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
