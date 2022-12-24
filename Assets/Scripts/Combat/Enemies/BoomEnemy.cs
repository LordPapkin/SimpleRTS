using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEnemy : EnemyBasic

{
    [Header("Boom Settings")]
    [SerializeField] private float radiusOfBoom = 5f;
    private bool isBoomed;

    private void OnCollisionStay2D(Collision2D collision)
    {
        HitBuilding(collision);
    }

    protected override void OnDied(object sender, EventArgs e)
    {
        Boom();
    }

    private void HitBuilding(Collision2D collision)
    { 
        Building building = collision.gameObject.GetComponent<Building>();

        if (building == null)
        {
            return;
        }
        Boom();   
    }
    
    private void Boom()
    {
        if (isBoomed)
            return;

        isBoomed = true;
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, radiusOfBoom);
        foreach(Collider2D collider2D in collider2Ds)
        {  
            HealthSystem healthSystem = collider2D.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.TakeDamage(dmg, attackType);
            }
        }      
        Die();
    }
}
