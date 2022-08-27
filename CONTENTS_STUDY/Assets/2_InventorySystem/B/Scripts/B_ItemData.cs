using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.IK;
using UnityEngine;

public class B_ItemData
{
    
    
    public int ID;
    public string mainCategory, mainCategoryKey;
    public string subCategory, subCategoryKey;
    public string grade;
    public int gradeKey;

    public string itemName, itemNameKey;
    public string itemDescription, itemDescriptionKey;
    public string itemImageKey;

    public int itemAbility1, itemAbility2, itemAbility3, itemAbility4;
    
    public int enhance;
    public int star;
    public int quantity;
    public bool isEquipped;
    public bool isLocked;

    public Dictionary<string, Dictionary<string, object>> infoTable;
    public Dictionary<string, Dictionary<string, object>> classTable;
    public Dictionary<string, Dictionary<string, object>> gradeTable;
    public Dictionary<string, Dictionary<string, object>> stringTable;

    public B_ItemData(ItemDataSO itemDataSO)
    {
        ID = itemDataSO.ID;
        var table = B_DataHolder.Instance.ITEMTABLE_MAININFO;
        mainCategory = table[ID.ToString()]["MAIN_CATEGORY"].ToString();
        subCategory = table[ID.ToString()]["SUB_CATEGORY"].ToString();
        grade = table[ID.ToString()]["BIRTHGRADE_ID"].ToString();
        
        enhance = itemDataSO.enhance;
        star = itemDataSO.star;
        quantity = itemDataSO.quantity;
        isEquipped = itemDataSO.isEquipped;
        isLocked = itemDataSO.isLocked;
    }

    public B_ItemData(int ID)
    {
        this.ID = ID;
        infoTable = B_DataHolder.Instance.ITEMTABLE_MAININFO;
        classTable = B_DataHolder.Instance.ITEMTABLE_CLASSIFICATION;
        gradeTable = B_DataHolder.Instance.GRADETABLE_GRADEINFO;
        stringTable = B_DataHolder.Instance.STRINGTABLE;
        
        mainCategoryKey = infoTable[ID.ToString()]["MAIN_CATEGORY"].ToString();
        mainCategory = classTable[mainCategoryKey]["SAMPLE"].ToString();
        subCategoryKey = infoTable[ID.ToString()]["SUB_CATEGORY"].ToString();
        subCategory = classTable[subCategoryKey]["SAMPLE"].ToString();
        gradeKey = Int32.Parse(infoTable[ID.ToString()]["BIRTHGRADE_ID"].ToString());
        grade = gradeTable[gradeKey.ToString()]["sample"].ToString();

        itemAbility1 =  Int32.Parse( infoTable[ID.ToString()]["ABILITY_1"].ToString());
        itemAbility2 = Int32.Parse( infoTable[ID.ToString()]["ABILITY_2"].ToString());
        itemAbility3 = Int32.Parse( infoTable[ID.ToString()]["ABILITY_3"].ToString());
        itemAbility4 = Int32.Parse( infoTable[ID.ToString()]["ABILITY_4"].ToString());
        
        itemNameKey = infoTable[ID.ToString()]["NAME"].ToString();
        itemName = stringTable[itemNameKey]["DESCRIPTION"].ToString();
        itemDescriptionKey = infoTable[ID.ToString()]["DESCRIPTION"].ToString();
        itemDescription = stringTable[itemDescriptionKey]["DESCRIPTION"].ToString();
        itemImageKey = infoTable[ID.ToString()]["IMAGEPATH"].ToString().Split('/')[2];
    }
}
