using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public static ArrowProjectile Create(Vector3 position, Enemy enemy)
    {
        GameObject pfArrowProjectile = Resources.Load<GameObject>("pfArrowProjectile");
        GameObject arrow = Instantiate(pfArrowProjectile, position, Quaternion.identity);

        ArrowProjectile arrowProjectile = arrow.GetComponent<ArrowProjectile>();
        arrowProjectile.SetTarget(enemy);
        return arrowProjectile;
    }

    [SerializeField] private int dmg;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float timeToDie;
    private Enemy targetEnemy;   
    private Vector3 lastMoveDir;
    private Vector3 moveDir;

    private void Update()
    {
        HandleArrowMovementToTarget();
        timeToDie -= Time.deltaTime;
        if(timeToDie < 0)
            Destroy(gameObject);
    }

    private void HandleArrowMovementToTarget()
    {
        if (targetEnemy == null)
        {
            moveDir = lastMoveDir;
        }
        else
        {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        }

        transform.eulerAngles = new Vector3(0, 0, Utilities.GetAngleFromVector(moveDir));
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void SetTarget(Enemy targetEnemy)
    {
        this.targetEnemy = targetEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
        {
            HealthSystem healthSystem = enemy.GetComponent<HealthSystem>();
            healthSystem.TakeDamege(dmg);

            Destroy(gameObject);
        }
    }
}
