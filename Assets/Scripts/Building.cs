using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private HealthSystem healthSystem;
    private BuildingTypeSO buildingType;

    void Awake()
    {
        buildingType = GetComponent<BuildingTypeHolder>().BuildingType;
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);
    } 
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            healthSystem.TakeDamege(10);
    }
    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }
}
