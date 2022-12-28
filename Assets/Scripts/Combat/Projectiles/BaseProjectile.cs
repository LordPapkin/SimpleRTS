using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected int dmg;
    [SerializeField] protected Type attackType;
    [SerializeField] protected float moveSpeed;
    [SerializeField] private float timeToDie;
    protected EnemyBasic targetEnemy;
    private Queue<BaseProjectile> queue;
    
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hit(collision);
    }

    private void Start()
    {
        if (targetEnemy == null)
            Destroy(this.gameObject);

        StartCoroutine(DisableAfterTime());
    }

    public abstract void SetTarget(EnemyBasic targetEnemy);

    public void Create(Queue<BaseProjectile> towerArrows, EnemyBasic targetEnemy)
    {
        queue = towerArrows;
        SetTarget(targetEnemy);
    }
    
    public void Use(EnemyBasic targetEnemy, Vector3 orgin)
    {
        transform.position = orgin;
        gameObject.SetActive(true);
        queue.Dequeue();
        SetTarget(targetEnemy);
    }

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

        Hide();
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
        queue.Enqueue(this);
    }

    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(timeToDie);
        Hide();
    }

}
