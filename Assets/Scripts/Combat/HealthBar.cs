using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Transform barTransform;
    [SerializeField] private Transform barSprite;
    [SerializeField] private Transform separatorContainer;
    [SerializeField] private Transform separatorTemplate;
    [SerializeField] private float hpSegment = 50f;
    private float barSize;

    private void Awake()
    {
        separatorTemplate.gameObject.SetActive(false);
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHeal += HealthSystem_OnHeal;
    }
    private void Start()
    {
        barSize = barSprite.transform.localScale.x;
        float oneHealthSegmentSize = (barSize / (float)healthSystem.HealthAmountMax) * hpSegment;
        int healthSeparatorCount = Mathf.FloorToInt(healthSystem.HealthAmountMax / hpSegment);
        for (int i = 1; i < healthSeparatorCount; i++)
        {
            Transform separator = Instantiate(separatorTemplate, separatorContainer);
            separator.gameObject.SetActive(true);
            separator.localPosition = new Vector3(oneHealthSegmentSize * i, 0, 0);
        }
        UpdateHeatlhBarVisibility();
        UpdateBar();
    }

    private void HealthSystem_OnHeal(object sender, System.EventArgs e)
    {
        UpdateBar();
        UpdateHeatlhBarVisibility();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        UpdateBar();
        UpdateHeatlhBarVisibility();
    }

    private void UpdateBar()
    {
        barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
    }
    private void UpdateHeatlhBarVisibility()
    {
        if(healthSystem.IsFullHP())
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
