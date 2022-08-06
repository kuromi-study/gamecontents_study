using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Singleton;

public class C_UserInfo : MonoSingleton<C_UserInfo>
{
    public long Currency { get; set; }
    public List<C_Item_FBS> itemList = new List<C_Item_FBS>();
}
