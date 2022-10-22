using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector3 position)
    {
        GameObject pfEnemy = Resources.Load<GameObject>("pfEnemy");
        GameObject enemyGameObject = Instantiate(pfEnemy, position, Quaternion.identity); 

        Enemy enemy = enemyGameObject.GetComponent<Enemy>();
        return enemy;
    }

    [Header("Enemy Settings")]
    [SerializeField] private float speed;
    [SerializeField] private int dmg;
    [SerializeField] private float searchRange;
    [SerializeField] private float lookForTargetTimerMax;

    private GameObject targetBuilding;
    private Rigidbody2D rb;
    private HealthSystem healthSystem;
    private float lookForTargetTimer;


    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDied += HealthSystem_OnDied;

        rb = GetComponent<Rigidbody2D>();
        lookForTargetTimer = Random.Range(0, lookForTargetTimerMax);
    }

    private void Start()
    {
        SetDefaultTarget();
    }
   
    private void Update()
    {
        HandleMovemnet();
        HandleTargetSearch();
    }    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HitBuilding(collision);
    }

    private void HitBuilding(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();
        if (building != null)
        {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.TakeDamege(dmg);

            Destroy(this.gameObject);
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

    private void HandleMovemnet()
    {
        if (targetBuilding != null)
        {
            Vector3 moveDir = (targetBuilding.transform.position - transform.position).normalized;
            rb.velocity = moveDir * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void LookForTargets()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, searchRange);

        foreach (Collider2D collider in collider2Ds)
        {
            Building building = collider.gameObject.GetComponent<Building>();
            if (building == null)
                continue;
            if(targetBuilding == null)
            {
                targetBuilding = building.gameObject;
                continue;
            }                
            if (Vector2.Distance(transform.position, building.transform.position) < Vector2.Distance(transform.position, targetBuilding.transform.position))
            {
                targetBuilding = building.gameObject;
            }
        }

        if(targetBuilding == null)
        {
            SetDefaultTarget();
        }
    }

    private void SetDefaultTarget()
    {
        if (BuildingManager.Instance.GetHQBuilding() != null)
        {
            targetBuilding = BuildingManager.Instance.GetHQBuilding().gameObject;
        }
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        Destroy(this.gameObject);
    }
}
