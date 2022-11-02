using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceGeneratorData 
{
    [field: SerializeField] public int AmountPerCycle { get; private set; }
    [field: SerializeField] public float SecondsPerCycle { get; private set; }
    [field: SerializeField] public ResourceTypeSO ResourceType { get; private set; }
    [field: SerializeField] public float ResourceDetectionRadius { get; private set; }
    [field: SerializeField] public int MaxResourceNodes { get; private set; }
}
