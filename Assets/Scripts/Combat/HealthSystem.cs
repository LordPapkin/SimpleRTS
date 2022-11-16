using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDamaged;
    public event EventHandler OnDied;
    public event EventHandler OnHeal;

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
            takenDamage = Mathf.FloorToInt(takenDamage * resistValue);

        healthAmount -= takenDamage;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (IsDead())
            OnDied?.Invoke(this, EventArgs.Empty);
    }
    public void Heal()
    {
        healthAmount = healthAmountMax;
        OnHeal?.Invoke(this, EventArgs.Empty);  
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
