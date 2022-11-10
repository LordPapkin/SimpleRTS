using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    public static EnemyWaveManager Instance => instance;
    private static EnemyWaveManager instance;

    private enum State
    {
        WaitingToSpawnWave,
        SpawningWave,
    }

    public Vector3 SpawnPoint => spawnPoint;
    public string WaveNumberString => waveNumber.ToString();
    public int WaveNumber => waveNumber;
    public float WaveTimer => nextWaveTimer;

    public event EventHandler OnWaveNumberChanged;

    [Header("Spawn Settings")]
    [SerializeField] private float timeToFirstWave = 30f;
    [SerializeField] private float timeBetweenWaves = 15f;
    [SerializeField] private float nextEnemyTimerMax = 0.2f;
    [SerializeField] private float waveNumberToIncreaseSpawnPoints = 5f;
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private GameObject nextWaveSpawnPoint;
    [SerializeField] [Range(0f, 30f)] private float spawnPointRandominess;
    [SerializeField] [Range(0f, 5f)] private float spawnRandominess;

    [Header("Enemies")]
    [SerializeField] private Transform enemiesParent;
    [SerializeField] private WaveType[] waveTypes;

    private EnemySO enemyToSpawn;
    private WaveType currentWaveType;
    private int currentWaveIndex;

    private Vector3 spawnPoint;
    private float nextEnemyTimerMaxCurrent;
    //SF for testing only
    [Header("Current State")]
    [SerializeField] private State state;
    [SerializeField] private float nextWaveTimer;
    [SerializeField] private float nextEnemyTimer;
    [SerializeField] private int remainingSpawnPoints;
    [SerializeField] private int waveNumber = 0;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Start()
    {
        nextWaveTimer = timeToFirstWave;        
        SetNextSpawnPoint();
        currentWaveType = waveTypes[0];
        currentWaveIndex = 0;
        state = State.WaitingToSpawnWave;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToSpawnWave:
                HandleWaitingToSpawnWave();
                break;
            case State.SpawningWave:
                SpawningWave();
                break;
        }       
       
    }   

    private void HandleWaitingToSpawnWave()
    {
        nextWaveTimer -= Time.deltaTime;
        if (nextWaveTimer < 0)
        {
            SetWave();
        }
    }

    private void SpawningWave()
    {
        if (remainingSpawnPoints <= 0)
        {
            SetNextSpawnPoint();
            state = State.WaitingToSpawnWave;
        }

        nextEnemyTimer -= Time.deltaTime;
        if (nextEnemyTimer > 0f)
        {
            return;
        }
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        nextEnemyTimer = UnityEngine.Random.Range(0f, nextEnemyTimerMaxCurrent);
        SelectRandomEnemy();       
        GameObject spawnedEnemy = Instantiate(enemyToSpawn.Prefab, spawnPoint + Utilities.GetRandomDir() * UnityEngine.Random.Range(0f, spawnRandominess), Quaternion.identity);
        spawnedEnemy.transform.SetParent(enemiesParent);
        remainingSpawnPoints -= enemyToSpawn.SpawnPointsCost;
    }

    private void SetWave()
    {        
        nextWaveTimer = timeBetweenWaves;

        remainingSpawnPoints = 5 + (waveNumber * (3 + Mathf.FloorToInt(waveNumber/ waveNumberToIncreaseSpawnPoints)));
        nextEnemyTimerMaxCurrent = nextEnemyTimerMax - (waveNumber * 0.0005f);
        nextEnemyTimerMaxCurrent = Mathf.Clamp(nextEnemyTimerMaxCurrent, nextEnemyTimerMax / 2f, nextEnemyTimerMax );

        waveNumber++;
        CheckWaveType();
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
        state = State.SpawningWave;
    }

    private void CheckWaveType()
    {
        if (currentWaveIndex + 1 > waveTypes.Length - 1)
            return;
        if(waveNumber >= waveTypes[currentWaveIndex+1].WaveNumber)
        {
            currentWaveIndex++;
            currentWaveType = waveTypes[currentWaveIndex];
        }
           
    }

    private void SetNextSpawnPoint()
    {
        spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].transform.position + (Utilities.GetRandomDir() * spawnPointRandominess);
        nextWaveSpawnPoint.transform.position = spawnPoint;
    }

    private void SelectRandomEnemy()
    {
        while (true)
        {
            enemyToSpawn = currentWaveType.enemies[UnityEngine.Random.Range(0, currentWaveType.enemies.Length)];
            if (remainingSpawnPoints >= enemyToSpawn.SpawnPointsCost)
                break;
        }        
    }
}
