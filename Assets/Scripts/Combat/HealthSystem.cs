using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler Damaged;
    public event EventHandler Died;
    public event EventHandler Healed;

    public int HealthAmountMax => healthAmountMax;
    public int HealthAmount => healthAmount;

    [SerializeField] private int healthAmountMax;
    private int healthAmount;
    private Type resistType;
    private float resistValue;

    public void TakeDamege(int damage, Type attackType)
    {
        int takenDamage = damage;
        if (attackType == resistType)
            takenDamage = Mathf.FloorToInt(takenDamage * (1f - resistValue));

        healthAmount -= takenDamage;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

        Damaged?.Invoke(this, EventArgs.Empty);

        if (IsDead())
            Died?.Invoke(this, EventArgs.Empty);
    }
    public void Heal()
    {
        healthAmount = healthAmountMax;
        Healed?.Invoke(this, EventArgs.Empty);  
    }
    
    public float GetHealthAmountNormalized()
    {
        return (float)healthAmount / healthAmountMax;
    }
    public bool IsFullHP()
    {
        if(healthAmount == healthAmountMax)
            return true;
        return false;
    }
    public bool IsDead()
    {
        if(healthAmount == 0)
            return true;
        return false;
    }
    public void SetUpHealthSystem(int healthAmountMax, Type resistType, float resistValue, bool updateHealthAmount)
    {
        this.resistType = resistType;
        this.resistValue = resistValue;
        this.healthAmountMax = healthAmountMax;
        if (updateHealthAmount)
        {
            healthAmount = healthAmountMax;
        }
    }
    private void Awake()
    {
        healthAmount = healthAmountMax;
    }
}
