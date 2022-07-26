using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour
{
    private ResourceTypeListSO resourceTypeList;
    private Dictionary<ResourceTypeSO, TextMeshProUGUI> resourceTypeUIDictionary;

    [SerializeField] private float offsetAmount = -180f;
    private int i = 1;   

    private void Awake()
    {
        InitializationUI();
    }   

    private void Start()
    {
        ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
        UpdateResourceAmount();
    }

    private void ResourceManager_OnResourceAmountChanged(object sender, System.EventArgs e)
    {
        UpdateResourceAmount();
    }

    private void InitializationUI()
    {
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        resourceTypeUIDictionary = new Dictionary<ResourceTypeSO, TextMeshProUGUI>();

        GameObject resourceTemplate = transform.Find("resourceTemplate").gameObject;
        resourceTemplate.SetActive(false);

        foreach (ResourceTypeSO resourceType in resourceTypeList.list)
        {
            GameObject templateCopy = Instantiate(resourceTemplate, this.transform);
            templateCopy.gameObject.SetActive(true);

            templateCopy.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * i, 0);

            templateCopy.transform.Find("image").GetComponent<Image>().sprite = resourceType.Sprite;
            TextMeshProUGUI resourceAmountUI = templateCopy.transform.Find("text").GetComponent<TextMeshProUGUI>();

            resourceTypeUIDictionary.Add(resourceType, resourceAmountUI);
            i++;
        }
    }
    private void UpdateResourceAmount()
    {
        foreach(ResourceTypeSO resourceType in resourceTypeList.list)
        {
            int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);
            resourceTypeUIDictionary[resourceType].text = resourceAmount.ToString();            
        }
        
    }
}
