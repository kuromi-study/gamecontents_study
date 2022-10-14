using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utility.Singleton;

public class B_UI_Inventory : MonoSingleton<B_UI_Inventory>
{
    private Transform inventoryTr;
    private GameObject inventoryObj;
    
    #region Top

    private Button closeBtn;
    private Text objTitleText;
    private Text currencyText;

    #endregion
    
    #region Left

    private Button filterAllBtn;
    private Button filterEquipmentBtn;
    private Button filterAccessaryBtn;
    private Button filterConsumableBtn;

    private Transform inventoryDisplay;
    private GameObject inventoryScroll;
    
    private Text itemCountText;
    private Button itemRemoveBtn;
    
    #endregion

    #region Middle

    private int selectedPage;
    private B_UI_InfoPage infoPage;
    private B_UI_EnhancePage enhancePage;
    private B_UI_GradeUpPage gradeUpPage;
    private B_UI_ComposePage composePage;
    private Button selectInfoBtn;
    private Button selectEnhanceBtn;
    private Button selectGradeUpBtn;
    private Button selectComposeBtn;

    #endregion

    #region Right

    private B_UI_CharacterInfo characterInfoPanel;

    #endregion

    
    
    void Awake()
    {
        BindObjects();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SelectPage(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectItem(B_InventoryItem item)
    {
        switch (selectedPage)
        {
            case 0:
                infoPage.DisplayItem(item);
                break;
            case 1:
                enhancePage.DisplaySelectedItem(item);
                break;
            case 2:
                gradeUpPage.DisplaySelectedItem(item);
                break;
            case 3:
                composePage.DisplayItem(item);
                break;
        }
    }
    
    public void SelectPage(int num)
    {
        switch (num)
        {
            case 0:
                infoPage.gameObject.SetActive(true);
                enhancePage.gameObject.SetActive(false);
                gradeUpPage.gameObject.SetActive(false);
                composePage.gameObject.SetActive(false);
                break;
            case 1:
                infoPage.gameObject.SetActive(false);
                enhancePage.gameObject.SetActive(true);
                gradeUpPage.gameObject.SetActive(false);
                composePage.gameObject.SetActive(false);
                break;
            case 2:
                infoPage.gameObject.SetActive(false);
                enhancePage.gameObject.SetActive(false);
                gradeUpPage.gameObject.SetActive(true);
                composePage.gameObject.SetActive(false);
                break;
            case 3:
                infoPage.gameObject.SetActive(false);
                enhancePage.gameObject.SetActive(false);
                gradeUpPage.gameObject.SetActive(false);
                composePage.gameObject.SetActive(true);
                break;
        }
        selectedPage = num;
    }

    void BindObjects()
    {
        inventoryObj = GameObject.Find("B_UI_Inventory");
        inventoryTr = inventoryObj.transform;
        List<Transform> children = inventoryTr.GetComponentsInChildren<Transform>(true).ToList();


        closeBtn = children.Find(x => x.name == "btn_back").GetComponent<Button>();
        objTitleText= children.Find(x => x.name == "txt_title").GetComponent<Text>();
        currencyText= children.Find(x => x.name == "txt_currency").GetComponent<Text>();
        
        filterAllBtn= children.Find(x => x.name == "ALL").GetComponent<Button>();
        filterEquipmentBtn= children.Find(x => x.name == "Equipment").GetComponent<Button>();
        filterAccessaryBtn= children.Find(x => x.name == "Accessary").GetComponent<Button>();
        filterConsumableBtn= children.Find(x => x.name == "Consumable").GetComponent<Button>();

        inventoryDisplay = children.Find(x => x.name == "InventoryDisplay").transform;
        inventoryScroll= children.Find(x => x.name == "InventoryScroll").gameObject;

        itemCountText= children.Find(x => x.name == "txt_exist").GetComponent<Text>();
        itemRemoveBtn= children.Find(x => x.name == "btn_trash").GetComponent<Button>();

        infoPage = children.Find(x => x.name == "InfoPage").GetComponent<B_UI_InfoPage>();
        enhancePage= children.Find(x => x.name == "EnhancePage").GetComponent<B_UI_EnhancePage>();
        gradeUpPage= children.Find(x => x.name == "GradeUpPage").GetComponent<B_UI_GradeUpPage>();
        composePage= children.Find(x => x.name == "ComposePage").GetComponent<B_UI_ComposePage>();
        selectInfoBtn= children.Find(x => x.name == "InfoTab").GetComponent<Button>();
        selectEnhanceBtn= children.Find(x => x.name == "EnhanceTab").GetComponent<Button>();
        selectGradeUpBtn= children.Find(x => x.name == "GradeUpTab").GetComponent<Button>();
        selectComposeBtn= children.Find(x => x.name == "ComposeTab").GetComponent<Button>();
        
        characterInfoPanel= children.Find(x => x.name == "ComposeTab").GetComponent<B_UI_CharacterInfo>();
        
        filterAllBtn.onClick.AddListener(FilterAll);
        filterEquipmentBtn.onClick.AddListener(FilterEquipment);
        filterAccessaryBtn.onClick.AddListener(FilterAccessary);
        filterConsumableBtn.onClick.AddListener(FilterConsumable);
        
        selectInfoBtn.onClick.AddListener(delegate { SelectPage(0);});
        selectEnhanceBtn.onClick.AddListener(delegate { SelectPage(1);});
        selectGradeUpBtn.onClick.AddListener(delegate { SelectPage(2);});
        selectComposeBtn.onClick.AddListener(delegate { SelectPage(3);});
        
        //Debug.Log(selectInfoBtn);
        B_Inventory.Instance.goldChanged+=SetCurrencyText;
    }

    void SetCurrencyText()
    {
        currencyText.text = B_Inventory.Instance.GetGold().ToString();
    }

    void FilterAll()
    {
        filterAllBtn.GetComponent<Image>().color=Color.white;
        filterAllBtn.GetComponentInChildren<Text>().color=Color.black;
        
        filterEquipmentBtn.GetComponent<Image>().color=Color.black;
        filterEquipmentBtn.GetComponentInChildren<Text>().color=Color.white;
        
        filterAccessaryBtn.GetComponent<Image>().color=Color.black;
        filterAccessaryBtn.GetComponentInChildren<Text>().color=Color.white;
        
        filterConsumableBtn.GetComponent<Image>().color=Color.black;
        filterConsumableBtn.GetComponentInChildren<Text>().color=Color.white;
        
        for (int i = 0; i < inventoryScroll.transform.childCount; i++)
        {
            inventoryScroll.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    void FilterEquipment()
    {
        for (int i = 0; i < inventoryScroll.transform.childCount; i++)
        {
            inventoryScroll.transform.GetChild(i).gameObject.SetActive(true);
        }
        
        filterAllBtn.GetComponent<Image>().color=Color.black;
        filterAllBtn.GetComponentInChildren<Text>().color=Color.white;
        
        filterEquipmentBtn.GetComponent<Image>().color=Color.white;
        filterEquipmentBtn.GetComponentInChildren<Text>().color=Color.black;
        
        filterAccessaryBtn.GetComponent<Image>().color=Color.black;
        filterAccessaryBtn.GetComponentInChildren<Text>().color=Color.white;
        
        filterConsumableBtn.GetComponent<Image>().color=Color.black;
        filterConsumableBtn.GetComponentInChildren<Text>().color=Color.white;
        
        List<B_InventoryItem> items = inventoryScroll.GetComponentsInChildren<B_InventoryItem>(true).ToList();
        foreach (var item in items)
        {
            if(item.itemData.mainCategoryKey!="1") item.gameObject.SetActive(false);
        }
    }

    void FilterAccessary()
    {
        for (int i = 0; i < inventoryScroll.transform.childCount; i++)
        {
            inventoryScroll.transform.GetChild(i).gameObject.SetActive(true);
        }
        
        filterAllBtn.GetComponent<Image>().color=Color.black;
        filterAllBtn.GetComponentInChildren<Text>().color=Color.white;
        
        filterEquipmentBtn.GetComponent<Image>().color=Color.black;
        filterEquipmentBtn.GetComponentInChildren<Text>().color=Color.white;
        
        filterAccessaryBtn.GetComponent<Image>().color=Color.white;
        filterAccessaryBtn.GetComponentInChildren<Text>().color=Color.black;
        
        filterConsumableBtn.GetComponent<Image>().color=Color.black;
        filterConsumableBtn.GetComponentInChildren<Text>().color=Color.white;
        
        List<B_InventoryItem> items = inventoryScroll.GetComponentsInChildren<B_InventoryItem>(true).ToList();
        foreach (var item in items)
        {
            if(item.itemData.mainCategoryKey!="1001") item.gameObject.SetActive(false);
        }
    }

    void FilterConsumable()
    {
        for (int i = 0; i < inventoryScroll.transform.childCount; i++)
        {
            inventoryScroll.transform.GetChild(i).gameObject.SetActive(true);
        }
        
        filterAllBtn.GetComponent<Image>().color=Color.black;
        filterAllBtn.GetComponentInChildren<Text>().color=Color.white;
        
        filterEquipmentBtn.GetComponent<Image>().color=Color.black;
        filterEquipmentBtn.GetComponentInChildren<Text>().color=Color.white;
        
        filterAccessaryBtn.GetComponent<Image>().color=Color.black;
        filterAccessaryBtn.GetComponentInChildren<Text>().color=Color.white;
        
        filterConsumableBtn.GetComponent<Image>().color=Color.white;
        filterConsumableBtn.GetComponentInChildren<Text>().color=Color.black;
        
        List<B_InventoryItem> items = inventoryScroll.GetComponentsInChildren<B_InventoryItem>(true).ToList();
        foreach (var item in items)
        {
            if(item.itemData.mainCategoryKey!="2001") item.gameObject.SetActive(false);
        }
    }
}
