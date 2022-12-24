using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : BaseProjectile
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

        CalculateMoveDir();
        RotateArrowToTarget();
    }

    private void HandleArrowMovementToTarget()
    {
        if (targetEnemy != null)
        {
            CalculateMoveDir();
            RotateArrowToTarget();
        }
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
    }
    
    private void RotateArrowToTarget()
    {
        transform.eulerAngles = new Vector3(0, 0, Utilities.GetAngleFromVector(moveDir));
    }

    private void CalculateMoveDir()
    {
        moveDir = (targetEnemy.transform.position - transform.position).normalized;
    }

}
