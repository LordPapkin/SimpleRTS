using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomArrowProjectile : ArrowProjectile
{
    [Header("Boom Settings")]
    [SerializeField] private float radiusOfBoom = 2f;
    [SerializeField] GameObject boomEffect;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBasic enemy = collision.GetComponent<EnemyBasic>();
        if (enemy != null)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, radiusOfBoom);
            foreach (Collider2D collider2D in collider2Ds)
            {
                HealthSystem healthSystem = collider2D.GetComponent<HealthSystem>();
                if (healthSystem != null)
                {
                    healthSystem.TakeDamege(dmg, attackType);
                }
            }
            if (boomEffect != null)
                Instantiate(boomEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
