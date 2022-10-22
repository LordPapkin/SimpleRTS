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
            foreach (ResourceAmount resource in demolishBuildingType.constructionCostArray)
            {
                ResourceManager.Instance.AddResource(resource.resourceType, Mathf.FloorToInt(resource.amount * 0.6f));
            }

            Destroy(building.gameObject);
        });
    }

}
