using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : HomingProjectile
{
    [SerializeField] private SpriteRenderer missileSprite;
    [Header("Boom Settings")]
    [SerializeField] private float radiusOfBoom = 3f;
    [SerializeField] ParticleSystem boomEffect;

    protected override void Hit(Collider2D collision)
    {
        base.Hit(collision);
       
    }

    protected override void DealDamage(EnemyBasic enemy)
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, radiusOfBoom);
        foreach (Collider2D collider2D in collider2Ds)
        {
            HealthSystem healthSystem = collider2D.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                healthSystem.TakeDamage(dmg, attackType);
            }
        }
        ShowExplosion();
    }

    private void ShowExplosion()
    {
        missileSprite.enabled = false;
        boomEffect.Play();
        Destroy(gameObject, 2f);
    }
}
