using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTypeHolder : MonoBehaviour
{
    [SerializeField] private BuildingTypeSO buildingType;

    public BuildingTypeSO BuildingType
    {
        get => buildingType;
        set => buildingType = value;
    }
}
