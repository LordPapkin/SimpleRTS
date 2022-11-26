using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    public bool IsNextWaveBossWave => isNextWaveBossWave;
    public float WaveTimer => nextWaveTimer;

    public event EventHandler OnWaveNumberChanged;

    [Header("Spawn Settings")]
    [SerializeField] private float timeToFirstWave = 30f;
    [SerializeField] private float minTimeBetweenWaves = 15f;
    [SerializeField] private float timeDecreaseAfterWave = 1f;
    [SerializeField] private float nextEnemyTimerMax = 0.2f;
    [SerializeField] private int startingSpawnPoints = 5;
    [SerializeField] private int startingSpawnPointsIncrementation = 3;
    [SerializeField] private int waveNumberToIncreaseSpawnPointsIncrementation = 5;
    [SerializeField] private int WaveToBossWave = 10;
    [SerializeField] private float bossWaveMultiplayer = 1.5f;
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private GameObject nextWaveSpawnPoint;
    [SerializeField] [Range(0f, 30f)] private float spawnPointRandominess;
    [SerializeField] [Range(0f, 5f)] private float spawnRandominess;

    [Header("Enemies")]
    [SerializeField] private Transform enemiesParent;
    [SerializeField] private WaveType[] waveTypes;

    private WarbandSO warbandToSpawn;
    private WaveType currentWaveType;
    private int currentWaveIndex;

    private Vector3 spawnPoint;
    private float nextEnemyTimerMaxCurrent;
    private float nextWaveTimerMax;
    private int leftoverSpawnPoints;
    private bool isNextWaveBossWave;
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
        Init();
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

    private void Init()
    {        
        nextWaveTimerMax = timeToFirstWave;
        nextWaveTimer = nextWaveTimerMax;
        SetNextSpawnPoint();
        currentWaveType = waveTypes[0];
        currentWaveIndex = 0;
        state = State.WaitingToSpawnWave;
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
        SelectRandomWarband();

        if (CheckSpawnPoints())
        {
            HandleSpawnPoints(true);
        }
        else
        {
            HandleSpawnPoints(false);
            return;
        }
        for (int i = 0; i < warbandToSpawn.Warband.Length; i++)
        {
            GameObject spawnedEnemy = Instantiate(warbandToSpawn.Warband[i].Prefab, spawnPoint + Utilities.GetRandomDir() * UnityEngine.Random.Range(0f, spawnRandominess), Quaternion.identity);
            spawnedEnemy.transform.SetParent(enemiesParent);
        }
    }

    private void SetWave()
    {
        nextWaveTimerMax -= timeDecreaseAfterWave;
        nextWaveTimerMax = Mathf.Clamp(nextWaveTimerMax, minTimeBetweenWaves, timeToFirstWave);
        nextWaveTimer = nextWaveTimerMax;

        SetSpawnPoints();

        nextEnemyTimerMaxCurrent = nextEnemyTimerMax - (waveNumber * 0.0005f);
        nextEnemyTimerMaxCurrent = Mathf.Clamp(nextEnemyTimerMaxCurrent, nextEnemyTimerMax / 2f, nextEnemyTimerMax);

        waveNumber++;
        CheckForBossWave();
        CheckWaveType();
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
        state = State.SpawningWave;
    }

    private void SetSpawnPoints()
    {
        remainingSpawnPoints = startingSpawnPoints + (waveNumber * (startingSpawnPointsIncrementation + (waveNumber / waveNumberToIncreaseSpawnPointsIncrementation)));

        if (isNextWaveBossWave)
        {
            remainingSpawnPoints = Mathf.FloorToInt(remainingSpawnPoints * bossWaveMultiplayer);
            isNextWaveBossWave = false;
        }

        remainingSpawnPoints += leftoverSpawnPoints;
        leftoverSpawnPoints = 0;
    }

    private void CheckForBossWave()
    {
        if((waveNumber + 1) % WaveToBossWave == 0)
        {
            isNextWaveBossWave = true;
        }
        else
        {
            isNextWaveBossWave = false;
        }
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

    private void SelectRandomWarband()
    {
        warbandToSpawn = currentWaveType.enemies[UnityEngine.Random.Range(0, currentWaveType.enemies.Length)];                      
    }

    private bool CheckSpawnPoints()
    {
        if (remainingSpawnPoints >= warbandToSpawn.SpawnPointsCost)
        {            
            return true;
        }            
        else
        {            
            return false;
        }
    }

    private void HandleSpawnPoints(bool value)
    {
        if (value)
        {
            remainingSpawnPoints -= warbandToSpawn.SpawnPointsCost;
        }
        else
        {
            leftoverSpawnPoints = remainingSpawnPoints;
            remainingSpawnPoints = 0;
        }
    }
}
