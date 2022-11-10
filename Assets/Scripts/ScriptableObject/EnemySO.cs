using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField] public GameObject Prefab { get; private set; }

    [field: SerializeField] public int SpawnPointsCost { get; private set; }

    [field: SerializeField] public int scoreValue;

    [Header("Enemy Settings")]
    [field: SerializeField] public float speed;

    [field: SerializeField] public float timeToAttack;

    [field: SerializeField] public int dmg;

    [field: SerializeField] public int hp;

    [field: SerializeField] public float searchRange;

    [field: SerializeField] public float lookForTargetTimerMax;

    [field: SerializeField] public ParticleSystem deathEffect;

    
}
