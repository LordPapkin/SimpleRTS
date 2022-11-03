using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolishButton : MonoBehaviour
{
    [SerializeField] private Building building;
    [SerializeField] private Button demolishButton;    

    private void Awake()
    {
        AddDemolishButtonListener();
    }

    private void AddDemolishButtonListener()
    {
        demolishButton.onClick.AddListener(() =>
        {
            BuildingTypeSO demolishBuildingType = building.GetComponent<BuildingTypeHolder>().BuildingType;
            foreach (ResourceAmount resource in demolishBuildingType.ConstructionCostArray)
            {
                ResourceManager.Instance.AddResource(resource.ResourceType, Mathf.FloorToInt(resource.Amount * 0.6f));
            }

            Destroy(building.gameObject);
        });
    }

}
