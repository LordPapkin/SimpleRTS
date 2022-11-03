using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairButton : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private BuildingTypeHolder buildingTypeHolder;
    [SerializeField] private Button repairButton;
    [SerializeField] private float repairEfficiency;

    private ResourceAmount[] repairCostArray;

    private void Awake()
    {       
        AddRepairButtonListener();
        repairCostArray = new ResourceAmount[buildingTypeHolder.BuildingType.ConstructionCostArray.Length];
        
    }

    private void AddRepairButtonListener()
    {        
        repairButton.onClick.AddListener(() =>
        {            
            float damagedPercent = 1f - (float)healthSystem.HealthAmount/(float)healthSystem.HealthAmountMax;
            Array.Copy(buildingTypeHolder.BuildingType.ConstructionCostArray, repairCostArray, repairCostArray.Length);

            for (int i = 0; i < repairCostArray.Length; i++)
            {
                repairCostArray[i].Amount = Mathf.FloorToInt((float)buildingTypeHolder.BuildingType.ConstructionCostArray[i].Amount * repairEfficiency * damagedPercent);
            }

            if (ResourceManager.Instance.CanAfford(repairCostArray))
            {
                healthSystem.Heal();  
                ResourceManager.Instance.SpendResources(repairCostArray);
            }           
        });
        
    }
}
