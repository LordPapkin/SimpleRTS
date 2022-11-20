using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] protected ParticleSystem buildingDestroyedSFX;
    [SerializeField] protected GameObject demolishButtonGameObject;
    [SerializeField] protected GameObject reapirButtonGameObject;
    [SerializeField] protected float delayTime = 1.5f;
    [SerializeField] protected HealthSystem healthSystem;
    [SerializeField] protected BuildingTypeHolder buildingTypeHolder;
    protected BuildingTypeSO buildingType;

    protected virtual void OnDied(object sender, System.EventArgs e)
    {
        HighscoreManager.Instance.SubtractionScore(buildingType.ScoreValue);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
        Instantiate(buildingDestroyedSFX, transform.position, Quaternion.identity);      
        Destroy(gameObject);
    }

    protected virtual void OnDamaged(object sender, System.EventArgs e)
    {
        ToggleRepairButton(true);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
    }


    private void Awake()
    {
        Init();
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
   
    private void OnHeal(object sender, System.EventArgs e)
    {
        ToggleRepairButton(false);
    }

    private void Init()
    {
        buildingType = buildingTypeHolder.BuildingType;
        healthSystem.Died += OnDied;
        healthSystem.Damaged += OnDamaged;
        healthSystem.Healed += OnHeal;
        healthSystem.SetUpHealthSystem(buildingType.HealthAmountMax, buildingType.ResistType, buildingType.ResistValue, true);
        ToggleDemolishButton(false);
        ToggleRepairButton(false);
    }



}
