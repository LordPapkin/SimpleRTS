using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType)
    {
        GameObject buildingConstructionPrefab = Resources.Load<GameObject>("pfBuildingConstruction");
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
    private Material constructionMaterial;
    private float constructionTimer; 
    private float constructionTimerMax;
    private BuildingTypeSO buildingType;  

    private void Awake()
    {
        constructionMaterial = spriteRenderer.material;
        Instantiate(buildingBuiltSFX, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        HandleConstruction();
    }

    private void HandleConstruction()
    {
        constructionMaterial.SetFloat("_Progress", ConstructionTimerNormailezed);
        constructionTimer -= Time.deltaTime;
        if (constructionTimer <= 0f)
        {
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
            Instantiate(buildingType.prefab, transform.position, Quaternion.identity);
            Instantiate(buildingBuiltSFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void SetUpBuildingType(BuildingTypeSO buildingType)
    {
        this.buildingType = buildingType;

        boxCollider.size = buildingType.prefab.GetComponent<BoxCollider2D>().size;
        boxCollider.offset = buildingType.prefab.GetComponent<BoxCollider2D>().offset;
        spriteRenderer.sprite = buildingType.sprite;
        buildingTypeHolder.BuildingType = buildingType;

        constructionTimerMax = buildingType.constructionTime;
        constructionTimer = constructionTimerMax;
    }
}
