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
    [SerializeField] private float waveNumberToIncreaseSpawnAmount = 5f;
    [SerializeField] private List<GameObject> spawnPoints;
    [SerializeField] private GameObject nextWaveSpawnPoint;
    [SerializeField] [Range(0f, 30f)] private float spawnPointRandominess;
    [SerializeField] [Range(0f, 5f)] private float spawnRandominess;

    [Header("Enemies")]
    [SerializeField] private Transform enemiesParent;
    [SerializeField] private GameObject[] enemyUnits;

    private Vector3 spawnPoint;
    private float nextEnemyTimerMaxCurrent;
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
        if(remainingEnemySpawnAmount <= 0)
        {
            SetNextSpawnPoint();
            state = State.WaitingToSpawnWave;
        }

        nextEnemyTimer -= Time.deltaTime;
        if (nextEnemyTimer > 0f)
        {
            return;
        }

        switch (waveNumber)
        {
            //case int n when (n >= 100):
            //    Debug.Log($"I am 100 or above: {n}");
            //    break;

            //case int n when (n < 100 && n >= 50):
            //    Debug.Log($"I am between 99 and 50: {n}");
            //    break;

            //case int n when (n < 50):
            //    Debug.Log($"I am less than 50: {n}");
            //    break;

            default:
                nextEnemyTimer = UnityEngine.Random.Range(0f, nextEnemyTimerMaxCurrent);
                GameObject spawnedEnemy = Instantiate(enemyUnits[0], spawnPoint + Utilities.GetRandomDir() * UnityEngine.Random.Range(0f, spawnRandominess), Quaternion.identity);
                spawnedEnemy.transform.SetParent(enemiesParent);
                remainingEnemySpawnAmount--;
                break;
        }
       
    }

    private void SetWave()
    {        
        nextWaveTimer = timeBetweenWaves;

        remainingEnemySpawnAmount = 5 + (waveNumber * (3 + Mathf.FloorToInt(waveNumber/ waveNumberToIncreaseSpawnAmount)));

        nextEnemyTimerMaxCurrent = nextEnemyTimerMax - (waveNumber * 0.0005f);
        nextEnemyTimerMaxCurrent = Mathf.Clamp(nextEnemyTimerMaxCurrent, nextEnemyTimerMax / 2f, nextEnemyTimerMax );

        waveNumber++;
        OnWaveNumberChanged?.Invoke(this, EventArgs.Empty);
        state = State.SpawningWave;
    }

    private void SetNextSpawnPoint()
    {
        spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].transform.position + (Utilities.GetRandomDir() * spawnPointRandominess);
        nextWaveSpawnPoint.transform.position = spawnPoint;
    }
}
