using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct WaveType
{
    [field: SerializeField] public int WaveNumber { get; set; }
    [field: SerializeField] public WarbandSO[] enemies { get; private set; }
    
}
