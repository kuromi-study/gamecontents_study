using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class B_InventoryItem : MonoBehaviour
{
    private B_ItemDisplay itemDisplay;
    
    private Button itemButton;
    private Text gradeText;
    private Text enhanceText;
    private Transform starsHolder;
    private Text numText;
    private Image itemImage;
    private Image equippedImage;
    private Image lockedImage;

    public B_ItemData itemData;
    public ItemDataSO itemDataSO;
    public int EDITOR_ID;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChanged;
    
    void Awake()
    {
        BindObjects();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SetItemData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemDisplay(B_ItemDisplay itemDisplay)
    {
        this.itemDisplay = itemDisplay;
    }

    public void UnsetItemDisplay()
    {
        itemDisplay = null;
    }

    public void SetItemData()
    {
        if (EDITOR_ID != -1)
        {
            itemData=new B_ItemData(EDITOR_ID);
        }
        
        if (itemDataSO != null)
        {
            itemData = new B_ItemData(itemDataSO);
        }
        //InitItemUI();
    }

    public void InitItemUI()
    {
        Debug.Log(itemImage);
        itemImage.sprite = B_DataHolder.Instance.spriteDictionary[itemData.itemImageKey];
        equippedImage.enabled = false;
        lockedImage.enabled = false;
        UpdateGradeUI();
        UpdateEnhanceUI();
        UpdateStarUI();
    }

    void BindObjects()
    {
        List<Transform> children = transform.GetComponentsInChildren<Transform>(true).ToList();
        /*
        itemButton = GetComponent<Button>();
        
        gradeText = children.Find(x => x.name == "GradeText").GetComponent<Text>();
        enhanceText = children.Find(x => x.name == "EnhanceText").GetComponent<Text>();
        starsHolder = children.Find(x => x.name == "STAR");
        numText = children.Find(x => x.name == "NumText").GetComponent<Text>();
        itemImage = children.Find(x => x.name == "img_item").GetComponent<Image>();
        equippedImage = children.Find(x => x.name == "EQUIPPED").GetComponent<Image>();
        lockedImage = children.Find(x => x.name == "LOCKED").GetComponent<Image>();
        
        itemButton.onClick.AddListener(OnItemSelected);
        */
    }

    void OnItemSelected()
    {
        B_UI_Inventory.Instance.SelectItem(this);
    }

    public void ItemChanged()
    {
        onItemChanged?.Invoke();
    }

    public void SetQuantity(int val)
    {
        itemData.quantity = val;
        Debug.Log(val);
        ItemChanged();
    }

    public void Equip()
    {
        B_PlayerInfo.Instance.EquipItem(this);
        itemData.isEquipped = true;
        //equippedImage.gameObject.SetActive(itemData.isEquipped);
    }

    public void Lock()
    {
        itemData.isLocked = !itemData.isLocked;
        lockedImage.gameObject.SetActive(itemData.isLocked);
    }

    public void EnhanceUp()
    {
        itemData.enhance++;
        itemData.itemAbility1 += 10;
        itemData.itemAbility2 += 10;
        itemData.itemAbility3 += 10;
        itemData.itemAbility4 += 10;
        ItemChanged();
    }

    public void EnhanceDown()
    {
        if (itemData.enhance == 0) return;
        itemData.enhance--;
        itemData.itemAbility1 -= 10;
        itemData.itemAbility2 -= 10;
        itemData.itemAbility3 -= 10;
        itemData.itemAbility4 -= 10;
        ItemChanged();
    }

    public void GradeUp()
    {
        itemData.gradeKey++;
        itemData.grade=itemData.gradeTable[itemData.gradeKey.ToString()]["sample"].ToString();
        itemData.itemAbility1 += 10;
        itemData.itemAbility2 += 10;
        itemData.itemAbility3 += 10;
        itemData.itemAbility4 += 10;
        ItemChanged();
    }

    public void GradeDown()
    {if (itemData.gradeKey == 0) return;
        itemData.gradeKey--;
        itemData.itemAbility1 -= 10;
        itemData.itemAbility2 -= 10;
        itemData.itemAbility3 -= 10;
        itemData.itemAbility4 -= 10;
        ItemChanged();
    }

    public void StarUp()
    {
        ItemChanged();
    }

    public void StarDown()
    {
        ItemChanged();
    }

    public void UpdateGradeUI()
    {
        gradeText.text = itemData.grade;
    }
    
    public void UpdateEnhanceUI()
    {
        enhanceText.text = $"+{itemData.enhance}";
    }

    public void UpdateStarUI()
    {
        for (int i = 0; i < 5; i++)
        {
            starsHolder.GetChild(i).gameObject.SetActive(false);
        }
        int num = itemData.star;
        int cnt = 0;
        while (cnt < num)
        {
            starsHolder.GetChild(cnt++).gameObject.SetActive(true);
        }
    }

    public void MirrorItem(B_InventoryItem item)
    {
        itemData = item.itemData;
        InitItemUI();
    }
}
