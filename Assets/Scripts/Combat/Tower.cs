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

    private Enemy target;
    private float lookForTargetTimer;
    private float shootTimer;   

    private void Start()
    {
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
        if(shootTimer < 0)
        {            
            GameObject arrow = Instantiate(arrowType, arrowSpawn.position, Quaternion.identity);
            ArrowProjectile arrowProjectile = arrow.GetComponent<ArrowProjectile>();
            arrowProjectile.SetTarget(target);  
            arrow.gameObject.transform.SetParent(arrowsParent);
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
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
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
