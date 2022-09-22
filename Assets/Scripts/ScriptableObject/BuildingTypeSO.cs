using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string nameString;
    public GameObject prefab;
    public bool hasResourceGeneratorData;
    public ResourceGeneratorData resourceGeneratorData;
    public Sprite sprite;
    public float minConstrutionRadius;
    public ResourceAmount[] constructionCostArray;
    public int healthAmountMax; 

    public string GetConstructionCostString()
    {
        string result = "";
        foreach (ResourceAmount constructionCost in constructionCostArray)
        {
            result += "<color="+ constructionCost.resourceType.colorHex+">"+ constructionCost.resourceType.nameShort + constructionCost.amount + "</color> ";
        }
        return result;
    }
}
