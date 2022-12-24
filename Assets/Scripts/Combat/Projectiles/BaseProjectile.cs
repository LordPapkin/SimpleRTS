using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected int dmg;
    [SerializeField] protected Type attackType;
    [SerializeField] protected float moveSpeed;
    [SerializeField] private float timeToDie;
    protected EnemyBasic targetEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hit(collision);
    }

    private void Start()
    {
        if (targetEnemy == null)
            Destroy(this.gameObject);

        Destroy(this.gameObject, timeToDie);
    }

    public abstract void SetTarget(EnemyBasic targetEnemy);

    protected virtual void Hit(Collider2D collision)
    {
        EnemyBasic enemy = collision.GetComponent<EnemyBasic>();
        if (enemy == null)
        {
            return;
        }
        
        DealDamage(enemy);
    }
    
    protected virtual void DealDamage(EnemyBasic enemy)
    {
        HealthSystem healthSystem = enemy.GetComponent<HealthSystem>();
        healthSystem.TakeDamage(dmg, attackType);

        Destroy(gameObject);
    }

}
