using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{
    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }

    public static BuildingManager Instance { get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;

    [SerializeField] private Transform buildingsParent;
    [SerializeField] private Building hqBuilding;
    [SerializeField] private float maxConstrutionRadius = 10f;
    [SerializeField] private float safeRadius;

    private PlayerInputHandler playerInputHandler;
    private BuildingTypeSO activeBuildingType;
    private BuildingTypeListSO buildingTypeList;
    private bool isOverUI;
    
    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            isOverUI = true;
            //Debug.Log("OverGameObject");
        }
        else
        {
            isOverUI = false;
            //Debug.Log("Free");
        }
    }

    private void OnEnable()
    {
        playerInputHandler = new PlayerInputHandler();
        playerInputHandler.PlaceBuildingClick += OnPlaceBuildingClick;       
        playerInputHandler.CancelBuildingClick += OnCancelBuildingClick;
    }

    private void OnDisable()
    {
        playerInputHandler.PlaceBuildingClick -= OnPlaceBuildingClick;
        playerInputHandler.CancelBuildingClick -= OnCancelBuildingClick;
    }

    public bool CanPlaceBuilding(BuildingTypeSO buildingType, Vector3 mouseWorldPosition, bool showTooltip)
    {
        BoxCollider2D boxCollider = buildingType.Prefab.GetComponent<BoxCollider2D>();
        string errorMessage;

        if (!CheckResourceNodes(buildingType, mouseWorldPosition, out errorMessage))
        {
            SetToolTip(errorMessage, showTooltip);
            return false;
        }

        if (!CheckIsAreaClear(mouseWorldPosition, boxCollider, out errorMessage))
        {
            SetToolTip(errorMessage, showTooltip);
            return false;
        }

        if (CheckForEnemies(mouseWorldPosition, safeRadius, out errorMessage))
        {
            SetToolTip(errorMessage, showTooltip);
            return false;
        }

        if (CheckForSameBuildings(buildingType, mouseWorldPosition, out errorMessage))
        {
            SetToolTip(errorMessage, showTooltip);
            return false;
        }

        if (!CheckForFriendlyBuildings(mouseWorldPosition, maxConstrutionRadius, out errorMessage))
        {
            SetToolTip(errorMessage, showTooltip);
            return false;
        }

        return true;
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChanged?.Invoke(this, new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType });
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
    }

    public Building GetHQBuilding()
    {
        return hqBuilding;
    }


    private void OnPlaceBuildingClick(InputAction.CallbackContext context)
    {
        HandleBuildingPlacing();
    }

    private void OnCancelBuildingClick(InputAction.CallbackContext context)
    {
        SetActiveBuildingType(null);
    }

    private void HandleBuildingPlacing()
    {
        if (isOverUI)
            return;

        if (!CheckSpawnRequirements())
            return;

        SpawnBuilding();

    }

    private void Init()
    {
        Instance = this;
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }

    private bool CheckSpawnRequirements()
    {
        if (activeBuildingType == null)
            return false;

        if (!ResourceManager.Instance.CanAfford(activeBuildingType.ConstructionCostArray, true))
            return false;

        if (!CanPlaceBuilding(activeBuildingType, Utilities.GetMouseWorldPosition(), true))
            return false;

        return true;
    }

    private void SpawnBuilding()
    {
        ResourceManager.Instance.SpendResources(activeBuildingType.ConstructionCostArray);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
        BuildingConstruction buildingConstruction = BuildingConstruction.Create(Utilities.GetMouseWorldPosition(), activeBuildingType);
        buildingConstruction.transform.SetParent(buildingsParent);
    }    

    private bool CheckResourceNodes(BuildingTypeSO buildingType, Vector3 mouseWorldPosition, out string errorMessage)
    {
        if (buildingType.HasResourceGeneratorData)
        {
            int nearbyResouceAmount = ResourceGenerator.GetNearbyResourceNodes(buildingType.ResourceGeneratorData, mouseWorldPosition);
            if (nearbyResouceAmount == 0)
            {
                errorMessage = "There are no resources nearby!";
                return false;
            }
        }
        errorMessage = String.Empty;
        return true;
    }

    private bool CheckIsAreaClear(Vector3 mouseWorldPosition, BoxCollider2D boxCollider, out string errorMessage)
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapBoxAll(mouseWorldPosition + (Vector3)boxCollider.offset, boxCollider.size, 0);
        if (nearbyColliders.Length != 0)
        {            
            errorMessage = "Area is not clear";
            return false;
        }
        errorMessage = String.Empty;
        return true;
    }

    private bool CheckForEnemies(Vector3 mouseWorldPosition, float safeRadius, out string errorMessage)
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(mouseWorldPosition, safeRadius);
        foreach (Collider2D collider in nearbyColliders)
        {
            EnemyBasic enemy = collider.GetComponent<EnemyBasic>();
            if (enemy != null)
            {
                errorMessage = "Can't build near enemies!";
                return true;
            }
        }
        errorMessage = String.Empty;
        return false;
    }

    private bool CheckForSameBuildings(BuildingTypeSO buildingType, Vector3 mouseWorldPosition, out string errorMessage)
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(mouseWorldPosition, buildingType.MinConstrutionRadius);
        foreach (Collider2D collider in nearbyColliders)
        {
            BuildingTypeHolder buildingTypeHolder = collider.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null && buildingTypeHolder.BuildingType == buildingType)
            {
                errorMessage = "Too close for another building of the same type";
                return true;
            }
        }
        errorMessage = String.Empty;
        return false;
    }

    private bool CheckForFriendlyBuildings(Vector3 mouseWorldPosition, float maxConstrutionRadius, out string errorMessage)
    {
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(mouseWorldPosition, maxConstrutionRadius);
        foreach (Collider2D collider in nearbyColliders)
        {
            BuildingTypeHolder buildingTypeHolder = collider.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                errorMessage = String.Empty;
                return true;
            }
        }
        errorMessage = "Too far from any other building";
        return false;
    }

    private void SetToolTip(string message, bool showTooltip)
    {
        if (!showTooltip)
            return;

        TooltipUI.Instance.Show(message, new TooltipUI.TooltipTimer { timer = 2f });
    }
}
