using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Warband")]
public class WarbandSO : ScriptableObject
{
    [field: SerializeField] public int SpawnPointsCost { get; private set; }
    [field: SerializeField] public EnemySO[] Warband { get; private set; }
}
