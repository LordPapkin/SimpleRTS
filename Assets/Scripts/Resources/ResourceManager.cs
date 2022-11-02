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
        if(resourceAmountDictionary[resourceType] + amount < 9999)
        {
            resourceAmountDictionary[resourceType] += amount;
        }
        else
        {
            resourceAmountDictionary[resourceType] += amount - (resourceAmountDictionary[resourceType] + amount - 9999);
        }
        
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
        
    }

    public bool CanAffordRepair(int amount)
    {
        int repairResourceAmount = GetResourceAmount(repairResource);
        if (amount > repairResourceAmount)
        {
            ShowNeededResources(amount, repairResourceAmount);
            return false;
        }
        return true;
    }
    
    public bool CanAfford(ResourceAmount[] resourceCostAmountArray)
    {
        foreach(ResourceAmount resourceCostAmount in resourceCostAmountArray)
        {
            if(resourceCostAmount.amount > GetResourceAmount(resourceCostAmount.resourceType))
            {
                ShowNeededResources(resourceCostAmountArray);
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

    public void SpendRepairCost(int amount)
    {
        resourceAmountDictionary[repairResource] -= amount;
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

    private void ShowNeededResources(int amountNeeded, int amountAvailable)
    {
        string result = "";       
        result += "<color=" + repairResource.ColorHex + ">" + repairResource.NameShort + (amountNeeded - amountAvailable) + "</color> ";
        TooltipUI.Instance.Show(result + " Needed!", new TooltipUI.TooltipTimer { timer = 2f });
    }

    private void ShowNeededResources(ResourceAmount[] resourceCostAmountArray)
    {
        string result = "";
        foreach (ResourceAmount constructionCost in resourceCostAmountArray)
        {
            int resourcesNeeded = constructionCost.amount;
            int resourcesAvailable = GetResourceAmount(constructionCost.resourceType);
            if (resourcesNeeded > resourcesAvailable)
            {
                result += "<color=" + constructionCost.resourceType.ColorHex + ">" + constructionCost.resourceType.NameShort + (resourcesNeeded - resourcesAvailable) + "</color> ";
            }            
        }        
        TooltipUI.Instance.Show(result + " Needed!", new TooltipUI.TooltipTimer { timer = 2f });
    }
}
