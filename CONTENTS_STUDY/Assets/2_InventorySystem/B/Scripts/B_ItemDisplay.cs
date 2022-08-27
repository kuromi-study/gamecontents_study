using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class B_ItemDisplay : MonoBehaviour
{
    private Button itemButton;
    private Text gradeText;
    private Text enhanceText;
    private Transform starsHolder;
    private Text numText;
    private Image itemImage;
    private Image equippedImage;
    private Image lockedImage;

    public B_InventoryItem mirroredItem;
    private B_ItemData mirroredItemData;
    
    void Awake()
    {
        BindObjects();
    }

    void Start()
    {
        InitInventoryItem();
    }

    public void InitInventoryItem()
    {
        if (mirroredItem != null)
        {
            if(mirroredItem.itemData==null) mirroredItem.SetItemData();
            mirroredItemData = mirroredItem.itemData;
            mirroredItem.onItemChanged += UpdateItemUI;
            //Debug.Log(gameObject.name + "subscribed");
            InitItemUI();
        }
    }
    
    public void SetInventoryItem(B_InventoryItem item)
    {
        if (mirroredItem != null)
        {
            mirroredItem.onItemChanged -= UpdateItemUI;
        }
        mirroredItem = item;
        if(mirroredItem.itemData==null) mirroredItem.SetItemData();
        mirroredItemData = mirroredItem.itemData;
        mirroredItem.onItemChanged += UpdateItemUI;
        InitItemUI();
    }

    public void SetRequiredItem(string key)
    {
        itemImage.sprite =
            B_DataHolder.Instance.spriteDictionary[
                B_DataHolder.Instance.GetValueFromTable("ITEMTABLE_MAININFO", key, "IMAGEPATH").Split('/')[2]];
    }

    public void SetGold()
    {
        itemImage.sprite = Resources.Load("Sprite/PASS/img_gold") as Sprite;
    }

    public void InitItemUI()
    {
        itemImage.sprite = B_DataHolder.Instance.spriteDictionary[mirroredItemData.itemImageKey];
        UpdateGradeUI();
        UpdateEnhanceUI();
        UpdateStarUI();
        UpdateNumUI();
    }

    public void UpdateItemUI()
    {
        //Debug.Log(gameObject.name+" is updating!");
        UpdateGradeUI();
        UpdateEnhanceUI();
        UpdateStarUI();
        UpdateNumUI();
    }

    void BindObjects()
    {
        itemButton = GetComponent<Button>();
        List<Transform> children = transform.GetComponentsInChildren<Transform>(true).ToList();
        
        gradeText = children.Find(x => x.name == "GradeText").GetComponent<Text>();
        enhanceText = children.Find(x => x.name == "EnhanceText").GetComponent<Text>();
        starsHolder = children.Find(x => x.name == "STAR");
        numText = children.Find(x => x.name == "NumText").GetComponent<Text>();
        itemImage = children.Find(x => x.name == "img_item").GetComponent<Image>();
        equippedImage = children.Find(x => x.name == "EQUIPPED").GetComponent<Image>();
        lockedImage = children.Find(x => x.name == "LOCKED").GetComponent<Image>();
        
        itemButton.onClick.AddListener(OnItemSelected);
    }
    
    void OnItemSelected()
    {
        B_UI_Inventory.Instance.SelectItem(mirroredItem);
    }

    public void UpdateGradeUI()
    {
        gradeText.text = mirroredItemData.grade;
    }
    
    public void UpdateEnhanceUI()
    {
        enhanceText.text = $"+{mirroredItemData.enhance}";
    }

    public void UpdateStarUI()
    {
        for (int i = 0; i < 5; i++)
        {
            starsHolder.GetChild(i).gameObject.SetActive(false);
        }
        int num = mirroredItemData.star;
        int cnt = 0;
        while (cnt < num)
        {
            starsHolder.GetChild(cnt++).gameObject.SetActive(true);
        }
    }

    public void UpdateNumUI()
    {
        numText.text = mirroredItem.itemData.quantity.ToString();
    }
}
