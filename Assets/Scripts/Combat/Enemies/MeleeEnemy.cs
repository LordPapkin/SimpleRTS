using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyBasic
{
    protected override void Update()
    {
        base.Update();
        HandleAttackTimer();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HitBuilding(collision);
    }

    private void HandleAttackTimer()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private void HitBuilding(Collision2D collision)
    {
        if (attackTimer > 0)
        {
            return;
        }

        Building building = collision.gameObject.GetComponent<Building>();

        if (building == null)
        {
            return;
        }

        HealthSystem healthSystem = building.GetComponent<HealthSystem>();
        healthSystem.TakeDamage(dmg, attackType);

        attackTimer += timeToAttack;
    }
}
