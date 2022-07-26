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

    private BuildingTypeSO activeBuildingType;
    private BuildingTypeListSO buildingTypeList;
    [SerializeField] private float maxConstrutionRadius = 10f;

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType});
    }
    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
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
            if (!CanSpawnBuilding(activeBuildingType, Utilities.GetMouseWorldPosition()))
                return;
            Instantiate(activeBuildingType.prefab, Utilities.GetMouseWorldPosition(), Quaternion.identity);
        }
    }
    private bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position)
    {
        BoxCollider2D boxCollider = buildingType.prefab.GetComponent<BoxCollider2D>();

        //checks if area for building is clear if not return false
        Collider2D[] collidersOnBuildingArea = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider.offset, boxCollider.size, 0);
        if(collidersOnBuildingArea.Length != 0)
            return false;

        //chceks if they are other buildings same type nearby if yes, return false
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(position, buildingType.minConstrutionRadius);
        foreach (Collider2D collider in nearbyColliders)
        {
            BuildingTypeHolder buildingTypeHolder = collider.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null && buildingTypeHolder.BuildingType == buildingType)
            {
                return false;
            }
                
        }

        //checks if they are any building at all, if yes return true
        nearbyColliders = Physics2D.OverlapCircleAll(position, maxConstrutionRadius);
        foreach (Collider2D collider in nearbyColliders)
        {
            BuildingTypeHolder buildingTypeHolder = collider.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
                return true;
        }

        return false;
    }
    
    
}
