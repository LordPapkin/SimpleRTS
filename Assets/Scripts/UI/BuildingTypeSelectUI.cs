using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private List<BuildingTypeSO> buildingsToIgnore;
    private GameObject arrowButton;
    private GameObject arrowButtonSelected;

    private GameObject buildingSelectTemplate;
    private BuildingTypeListSO buildingTypeList;
    private Dictionary<BuildingTypeSO, GameObject> buildingTypeUIDictionary;

    private int i = 0;
    private float offsetAmount = 180f;

    private void Awake()
    {
        InitializationUI();        
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
        UpdateActiveBuildingTypeButton();
    }

    private void InitializationUI()
    {

        buildingSelectTemplate = transform.Find("btnTemplate").gameObject;
        buildingSelectTemplate.SetActive(false);

        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        buildingTypeUIDictionary = new Dictionary<BuildingTypeSO, GameObject>();

        #region ArrowButton
        arrowButton = Instantiate(buildingSelectTemplate, this.transform);
        arrowButton.SetActive(true);

        GameObject arrowButtonImage = arrowButton.transform.Find("buildingImage").gameObject;
        arrowButtonImage.GetComponent<Image>().sprite = arrowSprite;
        arrowButtonImage.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -50f);

        arrowButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * i, 0);
        arrowButton.GetComponent<Button>().onClick.AddListener(() => { BuildingManager.Instance.SetActiveBuildingType(null); });

        MouseEnterExitEvents mouseEnterExitEvents = arrowButton.GetComponent<MouseEnterExitEvents>();

        mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) => { TooltipUI.Instance.Show("Pointer"); };
        mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) => { TooltipUI.Instance.Hide(); };

        arrowButtonSelected = arrowButton.transform.Find("selected").gameObject;        
        i++;
        #endregion

        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            if (buildingsToIgnore.Contains(buildingType))
                continue;

            GameObject copyTemplate = Instantiate(buildingSelectTemplate, this.transform);
            copyTemplate.SetActive(true);

            copyTemplate.transform.Find("buildingImage").GetComponent<Image>().sprite = buildingType.Sprite;

            copyTemplate.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * i, 0);
            copyTemplate.GetComponent<Button>().onClick.AddListener(() => { BuildingManager.Instance.SetActiveBuildingType(buildingType); });


            mouseEnterExitEvents = copyTemplate.GetComponent<MouseEnterExitEvents>();

            mouseEnterExitEvents.OnMouseEnter += (object sender, EventArgs e) => { TooltipUI.Instance.Show(buildingType.NameString + "\n" + buildingType.GetConstructionCostString() ); };
            mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) => { TooltipUI.Instance.Hide(); };

            GameObject copyTemplateSelected = copyTemplate.transform.Find("selected").gameObject;
            buildingTypeUIDictionary.Add(buildingType, copyTemplateSelected);
            i++;
        }
    }

    private void UpdateActiveBuildingTypeButton()
    {
        arrowButtonSelected.SetActive(false);
        foreach (BuildingTypeSO buildingType in buildingTypeUIDictionary.Keys)
        {
            GameObject buildingButtonSelected = buildingTypeUIDictionary[buildingType];
            buildingButtonSelected.SetActive(false);
        }

        BuildingTypeSO activebuildingType = BuildingManager.Instance.GetActiveBuildingType();
        if(activebuildingType == null)
        {
            arrowButtonSelected.SetActive(true);
        }
        else
        {
            buildingTypeUIDictionary[activebuildingType].SetActive(true);
        }
        
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        UpdateActiveBuildingTypeButton();
    }
}
