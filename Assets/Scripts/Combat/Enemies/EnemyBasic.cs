using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class EnemyBasic : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private EnemySO enemyData;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Rigidbody2D rb;
    private float speed;
    protected float timeToAttack;
    protected int dmg;
    protected Type attackType;
    private int hp;
    private Type resistType;
    private float resistValue;
    private float searchRange;
    private float lookForTargetTimerMax;
    private ParticleSystem deathEffect;
    private int scoreValue;
    

    [SerializeField] private GameObject targetBuilding; 
    private float lookForTargetTimer;
    protected float attackTimer;

    private void Awake()
    {
        Init();
        Debug.Log("Enemy Init");
    }
   
    private void Start()
    {
        SetDefaultTarget();
    }
   
    protected virtual void Update()
    {
        HandleMovemnet();
        HandleTargetSearch();       
    }    

    

    private void Init()
    {
        this.speed = enemyData.Speed;
        this.timeToAttack = enemyData.TimeToAttack;
        this.dmg = enemyData.Dmg;
        this.attackType= enemyData.AttackType;
        this.hp = enemyData.Hp;
        this.resistType = enemyData.ResistType;
        this.resistValue = enemyData.ResistValue;
        this.searchRange = enemyData.SearchRange;
        this.lookForTargetTimerMax = enemyData.LookForTargetTimerMax;
        this.deathEffect = enemyData.DeathEffect;
        this.scoreValue = enemyData.ScoreValue;

        healthSystem.Damaged += OnDamaged;
        healthSystem.Died += OnDied;
        healthSystem.SetUpHealthSystem(hp ,resistType ,resistValue ,true);
        lookForTargetTimer = Random.Range(0, lookForTargetTimerMax);
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
        if(HighscoreManager.Instance != null)
            HighscoreManager.Instance.AddScore(scoreValue);
        if(CinemamachineShake.Instance != null)
            CinemamachineShake.Instance.ShakeCamera(4f, 0.1f);
        if(SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);

        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    private void OnDamaged(object sender, System.EventArgs e)
    {
        if(SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
    }

    private void OnDied(object sender, System.EventArgs e)
    { 
        Die();
    }
}
