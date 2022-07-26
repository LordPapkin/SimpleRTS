using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private GameObject spirteGameObject;
    private void Awake()
    {
        spirteGameObject = transform.Find("sprite").gameObject;
    }
    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }    

    private void Update()
    {
        transform.position = Utilities.GetMouseWorldPosition();
    }
    private void Show(Sprite ghostSprite)
    {
        spirteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
        spirteGameObject.SetActive(true);       
    }
    private void Hide()
    {
        spirteGameObject.SetActive(false);
    }
    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        if(e.activeBuildingType == null)
        {
            Hide();
        }
        else
        {
            Show(e.activeBuildingType.sprite);
        }
    }
}
