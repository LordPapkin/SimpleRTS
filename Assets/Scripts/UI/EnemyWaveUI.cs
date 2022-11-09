using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyWaveUI : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private EnemyWaveManager enemyWaveManager;
    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private RectTransform waveSpawnIndicator;
    [SerializeField] private float offset;

    private void Start()
    {
        enemyWaveManager.OnWaveNumberChanged += EnemyWaveManager_OnWaveNumberChanged;
        SetNumberText($"Wave: {enemyWaveManager.WaveNumberString}");
        mainCamera = Camera.main;
    }   

    private void Update()
    {
        HandleTimerUI();
        HandleIndicator();
    }

    private void HandleIndicator()
    {
        Vector3 dirToNextSpawnPoint = (enemyWaveManager.SpawnPoint - mainCamera.transform.position).normalized;

        float distance = Vector3.Distance(enemyWaveManager.SpawnPoint, mainCamera.transform.position);
        if(distance < mainCamera.orthographicSize)
        {
            waveSpawnIndicator.gameObject.SetActive(false);
        }
        else
        {
            waveSpawnIndicator.gameObject.SetActive(true);
        }

        waveSpawnIndicator.anchoredPosition = dirToNextSpawnPoint * offset;
        waveSpawnIndicator.eulerAngles = new Vector3(0, 0, Utilities.GetAngleFromVector(dirToNextSpawnPoint));
    }

    private void HandleTimerUI()
    {
        if (enemyWaveManager.WaveTimer < 0f)
        {
            SetTimerText("");
        }
        else
        {
            SetTimerText($"Next Wave in {enemyWaveManager.WaveTimer.ToString("F1")} s");
        }
    }

    private void EnemyWaveManager_OnWaveNumberChanged(object sender, System.EventArgs e)
    {
        SetNumberText($"Wave: {enemyWaveManager.WaveNumberString}");
    }
    private void SetNumberText(string waveNumber)
    {
        waveNumberText.SetText(waveNumber);
    }
    private void SetTimerText(string timerNumber)
    {
        waveTimerText.SetText(timerNumber); 
    }
}
