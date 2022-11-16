using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DefenceType")]
public class DefenceTypeSO : ScriptableObject
{
    [field: SerializeField] private Resists[] Resists;
}
