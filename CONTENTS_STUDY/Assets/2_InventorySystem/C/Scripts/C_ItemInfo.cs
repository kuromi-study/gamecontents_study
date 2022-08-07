using System.Collections;
using System.Collections.Generic;


public class C_Item_FBS
{
    public int ItemUID { get; set; }

    public C_Item_FBS(int a)
    {
        ItemUID = a;
    }
}

public class C_ItemInfo
{
    public int ItemUID { get; set; }
    public int Grade { get; set; }
    public bool isLock { get; set; }
    public bool isEquip { get; set; }
    public int Index { get; set; }

    public string ImagePath
    {
        get
        {
            return _itemMainInfo[ItemUID.ToString()]["IMAGEPATH"].ToString();
        }
    }

    public string NameString
    {
        get
        {
            return _itemMainInfo[ItemUID.ToString()]["NAME"].ToString();
        }
    }

    public string DescriptionString
    {
        get
        {
            return _itemMainInfo[ItemUID.ToString()]["DESCRIPTION"].ToString();
        }
    }

    public string MainCategoryString
    {
        get
        {
            var classifyID = _itemMainInfo[ItemUID.ToString()]["MAIN_CATEGORY"].ToString();
            return _itemClassify[classifyID]["NAME"].ToString();
        }
    }

    public string SubCategoryString
    {
        get
        {
            var classifyID = _itemMainInfo[ItemUID.ToString()]["SUB_CATEGORY"].ToString();
            return _itemClassify[classifyID]["NAME"].ToString();
        }
    }

    public string GradeString
    {
        get
        {
            var classifyID = _itemMainInfo[ItemUID.ToString()]["BIRTHGRADE_ID"].ToString();
            return _gradeInfo[classifyID]["NAME"].ToString();
        }
    }

    Dictionary<string, Dictionary<string, object>> _itemMainInfo = new Dictionary<string, Dictionary<string, object>>();
    Dictionary<string, Dictionary<string, object>> _itemClassify = new Dictionary<string, Dictionary<string, object>>();
    Dictionary<string, Dictionary<string, object>> _gradeInfo = new Dictionary<string, Dictionary<string, object>>();

    public C_ItemInfo(int uid)
    {
        this.ItemUID = uid;
        _itemMainInfo = ExcelParser.Read("ITEMTABLE_MAININFO");
        _itemMainInfo = ExcelParser.Read("ITEMTABLE_CLASSIFICATION");
        _itemMainInfo = ExcelParser.Read("GRADETABLE_GRADEINFO");
    }

    public static C_ItemInfo GetItemInfo(int uid)
    {
        if(uid < 2000)
        {
            return new ItemInfoEquip(uid);
        }
        else
        {
            return new ItemInfoPortion(uid);
        }
    }
}

public class ItemInfoEquip : C_ItemInfo
{
    public int Enhance { get; set; }
    public int Star { get; set; }
    public ItemInfoEquip(int uid) : base(uid)
    {
        Enhance = 0;
        Star = 0;
    }
}

public class ItemInfoPortion : C_ItemInfo
{
    public int Num { get; set; }

    public ItemInfoPortion(int uid) : base(uid)
    {
        Num = 1;
    }
}
