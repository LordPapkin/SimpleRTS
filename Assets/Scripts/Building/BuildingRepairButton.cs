using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairButton : MonoBehaviour
{
    [SerializeField] private Building building;
    [SerializeField] private Button repairButton;

    private HealthSystem healthSystem;

    private void Awake()
    {
        AddRepairButtonListener();
    }

    private void AddRepairButtonListener()
    {        
        repairButton.onClick.AddListener(() =>
        {
            healthSystem = building.GetComponent<HealthSystem>();
            int repairCost = Mathf.FloorToInt((healthSystem.HealthAmountMax - healthSystem.HealthAmount) / 3);
            
            Debug.Log(repairCost);
            if (ResourceManager.Instance.CanAffordRepair(repairCost))
            {
                healthSystem.Heal();  
                ResourceManager.Instance.SpendRepairCost(repairCost);
            }
            else
            {
                TooltipUI.Instance.Show("Cannot afford reapir cost!", new TooltipUI.TooltipTimer { timer = 2f }); 
            }
        });
        
    }
}
