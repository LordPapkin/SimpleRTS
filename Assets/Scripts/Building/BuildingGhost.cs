using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spirteRenderer;
    [SerializeField] private ResourceNearbyOverlay resourceNearbyOverlay;
    [Header("Highlight Settings")]
    [SerializeField] private Color canBuildColor;
    [SerializeField] private Color cantBuildColor;

    private bool isShown;
    private BuildingTypeSO activeBuildingType;

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;        
    }    

    private void Update()
    {
        transform.position = Utilities.GetMouseWorldPosition();
        HandleHighlight();
    }

    private void HandleHighlight()
    {
        if (isShown)
        {
            if (BuildingManager.Instance.CanPlaceBuilding(activeBuildingType, Utilities.GetMouseWorldPosition(), false) && ResourceManager.Instance.CanAfford(activeBuildingType.ConstructionCostArray))
            {
                spirteRenderer.color = canBuildColor;
                TooltipUI.Instance.Hide();
            }
            else
            {
                spirteRenderer.color = cantBuildColor;                
            }
        }
    }

    private void Show(Sprite ghostSprite)
    {
        spirteRenderer.sprite = ghostSprite;
        spirteRenderer.gameObject.SetActive(true);  
        isShown = true;
    }

    private void Hide()
    {
        spirteRenderer.gameObject.SetActive(false);
        isShown = false;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        if(e.activeBuildingType == null)
        {
            Hide();
            resourceNearbyOverlay.Hide();
        }
        else
        {
            activeBuildingType = e.activeBuildingType;
            Show(activeBuildingType.Sprite);

            if (activeBuildingType.HasResourceGeneratorData)
            {
                resourceNearbyOverlay.Show(activeBuildingType.ResourceGeneratorData);
            }
            else
            {
                resourceNearbyOverlay.Hide();
            }            
        }
    }
}
