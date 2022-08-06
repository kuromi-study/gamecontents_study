using System.Collections;
using System.Collections.Generic;


public class C_Item_FBS
{
    public int ItemUID { get; set; }
}

public class C_ItemInfo
{
    public int ItemUID { get; set; }
    public int Grade { get; set; }
    public bool isLock { get; set; }
    public int Index { get; set; }

    public C_ItemInfo(int uid)
    {
        this.ItemUID = uid;
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
    }
}

public class ItemInfoPortion : C_ItemInfo
{
    public int Num { get; set; }

    public ItemInfoPortion(int uid) : base(uid)
    {
    }
}
