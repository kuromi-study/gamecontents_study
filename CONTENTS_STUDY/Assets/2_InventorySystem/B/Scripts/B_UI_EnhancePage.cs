using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class B_UI_EnhancePage : MonoBehaviour
{
    private B_ItemDisplay itemDisplay;
    private B_InventoryItem selectedItem;
    
    private B_ItemDisplay reqItemDisplay;
    private B_InventoryItem RequiredItem;

    private Text itemNameText;
    private Text itemEnhanceText;
    private Text itemAbilityText;
    private Text enhanceInfoText;
    private Text alertText;

    private Button enhanceButton;

    private Text mainCategoryInfoText;
    private Text subCategoryInfoText;
    private Text gradeInfoText;

    public int needID;
    private string needCount;
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
        itemEnhanceText=children.Find(x => x.name == "txt_enhance1").GetComponent<Text>();
        itemAbilityText=children.Find(x => x.name == "txt_ability").GetComponent<Text>();
        enhanceInfoText=children.Find(x => x.name == "txt_enhanceInfo").GetComponent<Text>();
        alertText=children.Find(x => x.name == "txt_alert").GetComponent<Text>();
        
        enhanceButton=children.Find(x => x.name == "btn_enhance").GetComponent<Button>();
        
        mainCategoryInfoText=children.Find(x => x.name == "txt_category_1").GetComponent<Text>();
        subCategoryInfoText=children.Find(x => x.name == "txt_category_2").GetComponent<Text>();
        gradeInfoText=children.Find(x => x.name == "txt_enhance2").GetComponent<Text>();
        
        enhanceButton.onClick.AddListener(EnhanceItem);
    }

    public void DisplaySelectedItem(B_InventoryItem item)
    {
        if (selectedItem != null) selectedItem.UnsetItemDisplay();//

        var check = B_DataHolder.Instance.GetValueFromTable("ENHANCETABLE_ENHANCE_COST", item.itemData.ID.ToString(),
            "NEEDID");
        if (check == null) return;
        
        
        selectedItem = item;
        selectedItem.SetItemDisplay(itemDisplay);//
        itemDisplay.SetInventoryItem(selectedItem);

        Debug.Log(B_DataHolder.Instance.GetValueFromTable("ENHANCETABLE_ENHANCE_COST", item.itemData.ID.ToString(),
            "NEEDID"));
        needID = Int32.Parse( B_DataHolder.Instance.GetValueFromTable("ENHANCETABLE_ENHANCE_COST", item.itemData.ID.ToString(),
            "NEEDID"));
        reqItemDisplay.SetRequiredItem(needID.ToString());
        
        itemNameText.text = item.itemData.itemName;
        itemEnhanceText.text = item.itemData.enhance.ToString();
        itemAbilityText.text = B_DataHolder.Instance.GetValueFromTable("ENHANCETABLE_ENHANCE_COST",
            item.itemData.ID.ToString(),
            "ABILITY_UP");

        needCount = B_DataHolder.Instance.GetValueFromTable("ENHANCETABLE_ENHANCE_COST", item.itemData.ID.ToString(),
            "NEEDNUM");
        sucChance = B_DataHolder.Instance.GetValueFromTable("ENHANCETABLE_ENHANCE_COST", item.itemData.ID.ToString(),
            "SUCCESS");
        failChance=B_DataHolder.Instance.GetValueFromTable("ENHANCETABLE_ENHANCE_COST", item.itemData.ID.ToString(),
            "FAIL");
        enhanceCost = B_DataHolder.Instance.GetValueFromTable("ENHANCETABLE_ENHANCE_COST", item.itemData.ID.ToString(),
            "COST");
        enhanceInfoText.text = $"필요 개수 : {needCount}\n 성공 확률 : {sucChance}%\n 강화 비용 : {enhanceCost}";
        
        mainCategoryInfoText.text = item.itemData.mainCategory;
        subCategoryInfoText.text = item.itemData.subCategory;
        gradeInfoText.text = item.itemData.enhance.ToString();
        alertText.text = "";

        //itemDescriptionText.text = item.itemData.itemDescription;
    }

    public void DisplayRequiredItem()
    {
        
    }

    public void EnhanceItem()
    {
        if (selectedItem.itemData.enhance == 5)
        {
            alertText.text = "최고 강화 상태입니다.";
            return;
        }
        
        int needCount = Int32.Parse(this.needCount);
        int sucChance = Int32.Parse(this.sucChance);
        int failChance = Int32.Parse(this.failChance);
        int enhanceCost = Int32.Parse(this.enhanceCost);

        if (B_Inventory.Instance.GetItemQuantity(needID) < needCount)
        {
            alertText.text = "재료 수량이 부족합니다.";
            return;
        }

        if (B_Inventory.Instance.GetGold() < enhanceCost)
        {
            alertText.text = "골드가 부족합니다.";
            return;
        }

        B_Inventory.Instance.SetItemQuantity(needID, B_Inventory.Instance.GetItemQuantity(needID) - needCount);
        B_Inventory.Instance.SubtractGold(enhanceCost);
        
        int res = Random.Range(0, 100);
        if (res < sucChance)
        {
            selectedItem.EnhanceUp();
            DisplaySelectedItem(selectedItem);
            alertText.text = "강화에 성공하여 수치가 상승하였습니다.";
        }
        else if (res < sucChance + failChance)
        {
            selectedItem.EnhanceDown();
            DisplaySelectedItem(selectedItem);
            alertText.text = "강화에 실패하여 수치가 하락하였습니다.";
        }
        else
        {
            alertText.text = "강화에 실패하여 수치가 유지되었습니다.";
        }
    }
}
