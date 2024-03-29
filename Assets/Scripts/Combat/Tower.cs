using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Tower Settings")]
    [SerializeField] private float searchRange;
    [SerializeField] private float lookForTargetTimerMax;
    [SerializeField] private float shootTimerMax;
    [SerializeField] private Transform arrowSpawn;
    [SerializeField] private GameObject arrowType;
    [Header("Arrow Hierarchy")]
    [SerializeField] private Transform arrowsParent;

    private EnemyBasic target;
    private float lookForTargetTimer;
    private float shootTimer;
    [SerializeField] private Queue<BaseProjectile> towerArrows;

    private void Start()
    {
        towerArrows = new Queue<BaseProjectile>();
        lookForTargetTimer = Random.Range(0, lookForTargetTimerMax);        
    }

    private void Update()
    {        
        HandleTargetSearch();
        HandleShooting();
    }

    private void HandleShooting()
    {
        if(target == null)
            return;
        shootTimer -= Time.deltaTime;
        if(shootTimer > 0)
        {            
            return;
        }

        if (towerArrows.Count > 0)
        {
            towerArrows.Peek().Use(target, arrowSpawn.position);
            shootTimer += shootTimerMax;
        }
        else
        {
            GameObject projectileGameObject = Instantiate(arrowType, arrowSpawn.position, Quaternion.identity, arrowsParent);
            BaseProjectile projectile = projectileGameObject.GetComponent<BaseProjectile>();
            projectile.Create(towerArrows, target);
            shootTimer += shootTimerMax;
        }
       
    }

    private void HandleTargetSearch()
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0)
        {
            lookForTargetTimer = lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void LookForTargets()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, searchRange);

        foreach (Collider2D collider in collider2Ds)
        {
            EnemyBasic enemy = collider.gameObject.GetComponent<EnemyBasic>();
            if (enemy == null)
                continue;
            if (target == null)
            {
                target = enemy;
                continue;
            }
            if (Vector2.Distance(transform.position, enemy.transform.position) < Vector2.Distance(transform.position, target.transform.position))
            {
                target = enemy;
            }
        }
    }
}
