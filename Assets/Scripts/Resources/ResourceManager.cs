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

    public void SpendResources(ResourceAmount[] resourceCostArray)
    {
        foreach (ResourceAmount resourceCost in resourceCostArray)
        {
            resourceAmountDictionary[resourceCost.ResourceType] -= resourceCost.Amount;
        }
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);

    }

    public bool CanAfford(ResourceAmount[] resourceCostArray)
    {
        foreach(ResourceAmount resourceCost in resourceCostArray)
        {
            if(resourceCost.Amount > GetResourceAmount(resourceCost.ResourceType))
            {
                ShowNeededResources(resourceCostArray);
                return false;
            }
        }
        return true;
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
            AddResource(resourceAmount.ResourceType, resourceAmount.Amount);
        }
    }

    private void ShowNeededResources(int amountNeeded, int amountAvailable)
    {
        string result = "";       
        result += "<color=" + repairResource.ColorHex + ">" + repairResource.NameShort + (amountNeeded - amountAvailable) + "</color> ";
        TooltipUI.Instance.Show(result + " Needed!", new TooltipUI.TooltipTimer { timer = 2f });
    }

    private void ShowNeededResources(ResourceAmount[] resourceCostArray)
    {
        string result = "";
        foreach (ResourceAmount resourceCost in resourceCostArray)
        {
            int resourcesNeeded = resourceCost.Amount;
            int resourcesAvailable = GetResourceAmount(resourceCost.ResourceType);
            if (resourcesNeeded > resourcesAvailable)
            {
                result += "<color=" + resourceCost.ResourceType.ColorHex + ">" + resourceCost.ResourceType.NameShort + (resourcesNeeded - resourcesAvailable) + "</color> ";
            }            
        }        
        TooltipUI.Instance.Show(result + " Needed!", new TooltipUI.TooltipTimer { timer = 2f });
    }
}
