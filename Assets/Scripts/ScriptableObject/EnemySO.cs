using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy")]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField] public GameObject Prefab { get; private set; }

    [field: SerializeField] public int ScoreValue { get; private set; }

    [field: SerializeField] public float Speed { get; private set; }

    [field: SerializeField] public float TimeToAttack { get; private set; }

    [field: SerializeField] public int Dmg { get; private set; }

    [field: SerializeField] public Type AttackType { get; private set; }

    [field: SerializeField] public int Hp { get; private set; }

    [field: SerializeField] public Type ResistType { get; private set; }

    [field: SerializeField] public float ResistValue { get; private set; }

    [field: SerializeField] public float SearchRange { get; private set; }

    [field: SerializeField] public float LookForTargetTimerMax { get; private set; }

    [field: SerializeField] public ParticleSystem DeathEffect { get; private set; }


}
