using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private ParticleSystem buildingDestroyedSFX;
    [SerializeField] private GameObject demolishButtonGameObject;
    [SerializeField] private GameObject reapirButtonGameObject;
    [SerializeField] private float delayTime = 1.5f;
    private HealthSystem healthSystem;
    private BuildingTypeSO buildingType;

    protected virtual void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
        Instantiate(buildingDestroyedSFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Awake()
    {
        buildingType = GetComponent<BuildingTypeHolder>().BuildingType;
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHeal += HealthSystem_OnHeal;
        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);
        ToggleDemolishButton(false);
        ToggleRepairButton(false);
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
        {
            return;
        }           
        demolishButtonGameObject.SetActive(isEnable);
    }

    private void ToggleRepairButton(bool isEnable)
    {        
        if (reapirButtonGameObject == null)
        {
            return;
        }     
        if(isEnable == true && healthSystem.IsFullHP())
        {
            return;
        }
        reapirButtonGameObject.SetActive(isEnable);
    }

    private IEnumerator ToggleDelay()
    {
        yield return new WaitForSeconds(delayTime);
        ToggleDemolishButton(false);        
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        ToggleRepairButton(true);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
    }

    private void HealthSystem_OnHeal(object sender, System.EventArgs e)
    {
        ToggleRepairButton(false);
    }


}
