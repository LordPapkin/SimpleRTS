using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    [field: SerializeField] public string NameString { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public bool HasResourceGeneratorData { get; private set; }
    [field: SerializeField] public ResourceGeneratorData ResourceGeneratorData { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public float MinConstrutionRadius { get; private set; }
    [field: SerializeField] public ResourceAmount[] ConstructionCostArray { get; private set; }
    [field: SerializeField] public int HealthAmountMax { get; private set; }
    [field: SerializeField] public float ConstructionTime { get; private set; }
    [field: SerializeField] public int ScoreValue { get; private set; }

    public string GetConstructionCostString()
    {
        string result = "";
        foreach (ResourceAmount constructionCost in ConstructionCostArray)
        {
            result += "<color="+ constructionCost.ResourceType.ColorHex+">"+ constructionCost.ResourceType.NameShort + constructionCost.Amount + "</color> ";
        }
        return result;
    }
}
