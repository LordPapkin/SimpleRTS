using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType)
    {
        GameObject buildingConstructionPrefab = Resources.Load<GameObject>("BuildingConstruction");
        GameObject buildingConstructionGameObject = Instantiate(buildingConstructionPrefab, position, Quaternion.identity);        

        BuildingConstruction buildingConstruction = buildingConstructionGameObject.GetComponent<BuildingConstruction>();
        buildingConstruction.SetUpBuildingType(buildingType);

        return buildingConstruction;
    }

    public float ConstructionTimerNormailezed { get { return 1f - constructionTimer / constructionTimerMax; } }

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private BuildingTypeHolder buildingTypeHolder;
    [SerializeField] private ParticleSystem buildingBuiltSFX;
   
    private float constructionTimer; 
    private float constructionTimerMax;
    private Transform buildingsParent;
    private BuildingTypeSO buildingType;  

    private void Awake()
    {        
        Instantiate(buildingBuiltSFX, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        HandleConstruction();
    }

    private void HandleConstruction()
    {
        
        constructionTimer -= Time.deltaTime;
        if (constructionTimer <= 0f)
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
            Instantiate(buildingType.Prefab, transform.position, Quaternion.identity, this.transform.parent);
            Instantiate(buildingBuiltSFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void SetUpBuildingType(BuildingTypeSO buildingType)
    {
        this.buildingType = buildingType;

        boxCollider.size = buildingType.Prefab.GetComponent<BoxCollider2D>().size;
        boxCollider.offset = buildingType.Prefab.GetComponent<BoxCollider2D>().offset;
        spriteRenderer.sprite = buildingType.Sprite;
        buildingTypeHolder.BuildingType = buildingType;        

        constructionTimerMax = buildingType.ConstructionTime;
        constructionTimer = constructionTimerMax;
    }
}
