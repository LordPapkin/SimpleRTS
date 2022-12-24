using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : BaseProjectile
{
    private Vector3 moveDir;
    
    private void Update()
    {
        HandleArrowMovementToTarget();
    }

    public override void SetTarget(EnemyBasic targetEnemy)
    {
        this.targetEnemy = targetEnemy;
        if (this.targetEnemy == null)
            Destroy(gameObject);

        moveDir = (this.targetEnemy.transform.position - transform.position).normalized;
        transform.eulerAngles = new Vector3(0, 0, Utilities.GetAngleFromVector(moveDir));
    }

    private void HandleArrowMovementToTarget()
    {
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
    }
}
