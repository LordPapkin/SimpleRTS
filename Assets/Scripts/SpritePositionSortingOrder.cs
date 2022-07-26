using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSortingOrder : MonoBehaviour
{
    [SerializeField] private bool runOnce;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        float precisionMultiplayer = 5f;
        spriteRenderer.sortingOrder = (int)(-transform.position.y * precisionMultiplayer);

        if(runOnce)
            Destroy(this);
    }
}
