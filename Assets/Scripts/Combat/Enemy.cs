using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private EnemySO enemyData;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Rigidbody2D rb;
    private float speed;
    private float timeToAttack;
    private int dmg;
    private int hp;
    private float searchRange;
    private float lookForTargetTimerMax;
    private ParticleSystem deathEffect;
    private int scoreValue;
    

    private GameObject targetBuilding; 
    private float lookForTargetTimer;
    private float attackTimer;

    private void Awake()
    {
        Init();
    }
   
    private void Start()
    {
        SetDefaultTarget();
    }
   
    private void Update()
    {
        HandleMovemnet();
        HandleTargetSearch();
        HandleAttackTimer();
    }    

    private void OnCollisionStay2D(Collision2D collision)
    {
        HitBuilding(collision);
    }

    private void Init()
    {
        this.speed = enemyData.speed;
        this.timeToAttack = enemyData.timeToAttack;
        this.dmg = enemyData.dmg;
        this.hp = enemyData.hp;
        this.searchRange = enemyData.searchRange;
        this.lookForTargetTimerMax = enemyData.lookForTargetTimerMax;
        this.deathEffect = enemyData.deathEffect;
        this.scoreValue = enemyData.scoreValue;

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.SetHealthAmountMax(hp, true);
        lookForTargetTimer = Random.Range(0, lookForTargetTimerMax);
    }


    private void HitBuilding(Collision2D collision)
    {
        if (attackTimer > 0)
        {
            return;
        }

        Building building = collision.gameObject.GetComponent<Building>();

        if (building == null)
        {
            return;
        }

        HealthSystem healthSystem = building.GetComponent<HealthSystem>();
        healthSystem.TakeDamege(dmg);

        attackTimer += timeToAttack;
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

    private void HandleAttackTimer()
    {
        if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
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
            {
                continue;
            } 
            
            if(targetBuilding == null)
            {
                targetBuilding = building.gameObject;
                continue;
            } 
            
            //if you find closer building change target
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

    private void Die()
    {
        HighscoreManager.Instance.AddScore(scoreValue);
        CinemamachineShake.Instance.ShakeCamera(4f, 0.1f);
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    { 
        Die();
    }
}
