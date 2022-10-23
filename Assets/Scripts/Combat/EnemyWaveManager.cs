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
    [SerializeField] private float timeToFirstWave;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private GameObject nextWaveSpawnPoint;
    [SerializeField] [Range(0f, 30f)] private float spawnRandominess;

    [Header("Enemies")]
    [SerializeField] private Transform enemiesParent;

    private Vector3 spawnPoint;
    //SF for testing only
    [SerializeField] private State state;
    [SerializeField] private float nextWaveTimer;
    [SerializeField] private float nextEnemyTimer;
    [SerializeField] private int remainingEnemySpawnAmount;
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
                SpawnWave();
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
    private void SpawnWave()
    {
        if (remainingEnemySpawnAmount > 0)
        {
            nextEnemyTimer -= Time.deltaTime;
            if (nextEnemyTimer < 0f)
            {
                nextEnemyTimer = UnityEngine.Random.Range(0f, 0.2f);
                Enemy enemy = Enemy.Create(spawnPoint + Utilities.GetRandomDir() * UnityEngine.Random.Range(0f, 5f));
                enemy.gameObject.transform.SetParent(enemiesParent);
                remainingEnemySpawnAmount--;
            }
        }
        else
        {
            SetNextSpawnPoint();
            state = State.WaitingToSpawnWave;
        }
    }

    private void SetWave()
    {        
        nextWaveTimer = timeBetweenWaves;
        remainingEnemySpawnAmount = 5 + (waveNumber * 3);
        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
        state = State.SpawningWave;
    }
    private void SetNextSpawnPoint()
    {
        spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].transform.position + (Utilities.GetRandomDir() * spawnRandominess);
        nextWaveSpawnPoint.transform.position = spawnPoint;
    }
}
