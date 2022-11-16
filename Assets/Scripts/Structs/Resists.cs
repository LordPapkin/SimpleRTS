using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Resists
{    
    [field: SerializeField] public Levels ResistLevel { get; private set; }
    [field: SerializeField] public int ResistValue { get; private set; }
}
