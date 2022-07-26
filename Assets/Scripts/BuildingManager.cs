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
            Instantiate(activeBuildingType.prefab, Utilities.GetMouseWorldPosition(), Quaternion.identity);
        }
    }
    
    
}
