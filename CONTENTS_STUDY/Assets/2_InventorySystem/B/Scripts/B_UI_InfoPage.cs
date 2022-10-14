using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class B_UI_InfoPage : MonoBehaviour
{
    private B_ItemDisplay itemDisplay;
    private B_InventoryItem selectedItem;
    
    private Button lockBtn;
    private Text mainCategoryText;
    private Text subCategoryText;
    private Text gradeText;
    private Text itemNameText;
    private Text itemDescriptionText;
    private Text itemAbilityText;
    private Button equipBtn;

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
        lockBtn=children.Find(x => x.name == "Btn_Lock").GetComponent<Button>();
        mainCategoryText=children.Find(x => x.name == "txt_category_1").GetComponent<Text>();
        subCategoryText=children.Find(x => x.name == "txt_category_2").GetComponent<Text>();
        gradeText=children.Find(x => x.name == "txt_grade").GetComponent<Text>();
        itemNameText=children.Find(x => x.name == "txt_name").GetComponent<Text>();
        itemDescriptionText=children.Find(x => x.name == "txt_des").GetComponent<Text>();
        itemAbilityText=children.Find(x => x.name == "txt_ability").GetComponent<Text>();
        equipBtn=children.Find(x => x.name == "btn_equipped").GetComponent<Button>();

        lockBtn.onClick.AddListener(LockItem);
        equipBtn.onClick.AddListener(EquipItem);
    }

    public void DisplayItem(B_InventoryItem item)
    {
        selectedItem = item;
        itemDisplay.SetInventoryItem(selectedItem);
        
        mainCategoryText.text = item.itemData.mainCategory;
        subCategoryText.text = item.itemData.subCategory;
        gradeText.text = item.itemData.grade;
        itemNameText.text = item.itemData.itemName;
        itemDescriptionText.text = item.itemData.itemDescription;
    }

    void EquipItem()
    {
        if (selectedItem == null) return;
        selectedItem.Equip();
    }

    void LockItem()
    {
        if (selectedItem == null) return;
        selectedItem.Lock();
    }
}
