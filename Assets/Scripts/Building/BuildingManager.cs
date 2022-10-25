using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;

    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }   

    [SerializeField] private Building hqBuilding;
    [SerializeField] private float maxConstrutionRadius = 10f;
    [SerializeField] private float safeRadius;    
    private BuildingTypeSO activeBuildingType;
    private BuildingTypeListSO buildingTypeList;
    

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType});
    }
    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
    } 
    public Building GetHQBuilding()
    {
        return hqBuilding;
    }

    private void Awake()
    {
        Instance = this;
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);        
    }
   
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType == null)
                return;
            if (!CanSpawnBuilding(activeBuildingType, Utilities.GetMouseWorldPosition(), out string errorMessage))
            {
                TooltipUI.Instance.Show(errorMessage, new TooltipUI.TooltipTimer { timer = 2f }); ;
                return;
                
            }               
            if (!ResourceManager.Instance.CanAfford(activeBuildingType.constructionCostArray))
            {                
                return;               
            }                

            ResourceManager.Instance.SpendResources(activeBuildingType.constructionCostArray);            
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
            BuildingConstruction.Create(Utilities.GetMouseWorldPosition(), activeBuildingType);
        }
    }
    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position, out string errorMessage)
    {
        BoxCollider2D boxCollider = buildingType.prefab.GetComponent<BoxCollider2D>();

        //checks if area for building is clear if not return false
        Collider2D[] nearbyColliders = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider.offset, boxCollider.size, 0);
        if(nearbyColliders.Length != 0)
        {
            errorMessage = "Area is not clear";
            return false;
        }

        nearbyColliders = Physics2D.OverlapCircleAll(position, safeRadius);
        foreach(Collider2D collider in nearbyColliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                errorMessage = "Can't build near enemies!";
                return false;
            }
        }

        //chceks if they are other buildings same type nearby if yes, return false
         nearbyColliders = Physics2D.OverlapCircleAll(position, buildingType.minConstrutionRadius);
        foreach (Collider2D collider in nearbyColliders)
        {
            BuildingTypeHolder buildingTypeHolder = collider.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null && buildingTypeHolder.BuildingType == buildingType)
            {
                errorMessage = "Too close for another building of the same type";
                return false;
            }
                
        }

        //checks if they are any building at all, if yes return true
        nearbyColliders = Physics2D.OverlapCircleAll(position, maxConstrutionRadius);
        foreach (Collider2D collider in nearbyColliders)
        {
            BuildingTypeHolder buildingTypeHolder = collider.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                errorMessage = "";
                return true;
            }
                
        }
        errorMessage = "Too far from any other building";
        return false;
    }

}
