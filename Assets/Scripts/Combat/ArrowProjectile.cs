using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [SerializeField] private int dmg;
    [SerializeField] private Type attackType;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float timeToDie;
    private EnemyBasic targetEnemy;
    private Vector3 moveDir;

    public void SetTarget(EnemyBasic targetEnemy)
    {
        this.targetEnemy = targetEnemy;
        if (this.targetEnemy == null)
            Destroy(gameObject);

        moveDir = (this.targetEnemy.transform.position - transform.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, Utilities.GetAngleFromVector(moveDir));
         
    }

    private void Start()
    {
        if (targetEnemy == null)
            Destroy(this.gameObject);

        Destroy(this.gameObject, timeToDie);
    }

    private void Update()
    {
        HandleArrowMovementToTarget();
    }

    private void HandleArrowMovementToTarget()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBasic enemy = collision.GetComponent<EnemyBasic>();
        if(enemy != null)
        {
            HealthSystem healthSystem = enemy.GetComponent<HealthSystem>();
            healthSystem.TakeDamege(dmg, attackType);

            Destroy(gameObject);
        }
    }
}
