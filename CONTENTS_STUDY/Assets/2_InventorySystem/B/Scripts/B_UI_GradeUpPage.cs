using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class B_UI_GradeUpPage : MonoBehaviour
{
    private B_ItemDisplay itemDisplay;
    private B_InventoryItem selectedItem;
    
    private B_ItemDisplay reqItemDisplay;
    private B_InventoryItem RequiredItem;

    private Text itemNameText;
    private Text itemGradeText;
    private Text itemAbilityText;
    private Text enhanceInfoText;
    private Text alertText;

    private Button enhanceButton;

    private Text mainCategoryInfoText;
    private Text subCategoryInfoText;
    private Text gradeInfoText;

    private string sucChance;
    private string failChance;
    private string enhanceCost;
    
    void Awake()
    {
        BindObjects();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void BindObjects()
    {
        List<Transform> children = transform.GetComponentsInChildren<Transform>(true).ToList();

        itemDisplay = children.Find(x => x.name == "UI_Inventory_Item").GetComponent<B_ItemDisplay>();
        reqItemDisplay = children.Find(x => x.name == "UI_ReqItemDisplay").GetComponent<B_ItemDisplay>();
        
        
        itemNameText=children.Find(x => x.name == "txt_name").GetComponent<Text>();
        itemGradeText=children.Find(x => x.name == "txt_grade1").GetComponent<Text>();
        itemAbilityText=children.Find(x => x.name == "txt_ability").GetComponent<Text>();
        enhanceInfoText=children.Find(x => x.name == "txt_enhanceInfo").GetComponent<Text>();
        alertText=children.Find(x => x.name == "txt_alert").GetComponent<Text>();
        
        enhanceButton=children.Find(x => x.name == "btn_enhance").GetComponent<Button>();
        
        mainCategoryInfoText=children.Find(x => x.name == "txt_category_1").GetComponent<Text>();
        subCategoryInfoText=children.Find(x => x.name == "txt_category_2").GetComponent<Text>();
        gradeInfoText=children.Find(x => x.name == "txt_grade2").GetComponent<Text>();
        
        enhanceButton.onClick.AddListener(GradeUpItem);
    }

    public void DisplaySelectedItem(B_InventoryItem item)
    {
        if (selectedItem != null) selectedItem.UnsetItemDisplay();//
        
        
        
        selectedItem = item;
        selectedItem.SetItemDisplay(itemDisplay);//
        itemDisplay.SetInventoryItem(selectedItem);
        
        //reqItemDisplay.SetGold();
        
        itemNameText.text = item.itemData.itemName;
        itemGradeText.text = item.itemData.grade;
        itemAbilityText.text = B_DataHolder.Instance.GetValueFromTable("ENHANCETABLE_GRADEUP_COST",
            item.itemData.ID.ToString(),
            "ABILITY_UP");

        
        sucChance = B_DataHolder.Instance.GetValueFromTable("ENHANCETABLE_GRADEUP_COST", item.itemData.ID.ToString(),
            "SUCCESS");
        failChance=B_DataHolder.Instance.GetValueFromTable("ENHANCETABLE_GRADEUP_COST", item.itemData.ID.ToString(),
            "FAIL");
        enhanceCost = B_DataHolder.Instance.GetValueFromTable("ENHANCETABLE_GRADEUP_COST", item.itemData.ID.ToString(),
            "COST");
        enhanceInfoText.text = $"성공 확률 : {sucChance}%\n 강화 비용 : {enhanceCost}";
        
        mainCategoryInfoText.text = item.itemData.mainCategory;
        subCategoryInfoText.text = item.itemData.subCategory;
        gradeInfoText.text = item.itemData.grade;
        alertText.text = "";

        //itemDescriptionText.text = item.itemData.itemDescription;
    }

    public void DisplayRequiredItem()
    {
        
    }

    public void GradeUpItem()
    {
        if (selectedItem == null) return;
        if (selectedItem.itemData.gradeKey == 5)
        {
            alertText.text = "최고 등급 상태입니다.";
            return;
        }
        
        int sucChance = Int32.Parse(this.sucChance);
        int failChance = Int32.Parse(this.failChance);
        int enhanceCost = Int32.Parse(this.enhanceCost);

        
        
        int res = Random.Range(0, 100);
        if (res < sucChance)
        {
            selectedItem.GradeUp();
            DisplaySelectedItem(selectedItem);
            alertText.text = "강화에 성공하여 수치가 상승하였습니다.";
        }
        else if (res < sucChance + failChance)
        {
            selectedItem.GradeDown();
            DisplaySelectedItem(selectedItem);
            alertText.text = "강화에 실패하여 수치가 하락하였습니다.";
        }
        else
        {
            alertText.text = "강화에 실패하여 수치가 유지되었습니다.";
        }
    }
}
