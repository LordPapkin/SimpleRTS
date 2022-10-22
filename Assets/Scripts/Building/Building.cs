using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject demolishButtonGameObject;
    [SerializeField] private float delayTime = 1.5f;
    private HealthSystem healthSystem;
    private BuildingTypeSO buildingType;

    void Awake()
    {
        buildingType = GetComponent<BuildingTypeHolder>().BuildingType;
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);
        ToggleDemolishButton(false);
    }
    private void OnMouseEnter()
    {
        ToggleDemolishButton(true);
    }    

    private void OnMouseExit()
    {
        StartCoroutine("ToggleDelay");
    }
    private void ToggleDemolishButton(bool isEnable)
    {
        if (demolishButtonGameObject == null)
            return;
        demolishButtonGameObject.SetActive(isEnable);
    }
    IEnumerator ToggleDelay()
    {
        yield return new WaitForSeconds(delayTime);
        ToggleDemolishButton(false);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }
   

}
