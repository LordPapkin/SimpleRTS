using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct ResourceAmount 
{
    [field: SerializeField] public ResourceTypeSO ResourceType { get; private set; }
    [field: SerializeField] public int Amount { get; set; }
}
