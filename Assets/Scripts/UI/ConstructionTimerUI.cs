using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionTimerUI : MonoBehaviour
{
    [SerializeField] private BuildingConstruction buildingConstruction;   
    [SerializeField] private Image image;

    private void Update()
    {
        image.fillAmount = buildingConstruction.ConstructionTimerNormailezed;
    }
}
