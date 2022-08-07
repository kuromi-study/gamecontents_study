using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_LobbyMain : MonoBehaviour
{
    public void OnClickInventory()
    {
        C_UI_Inventory.Open();
    }

    private void Awake()
    {
        // 임의로 아이템 초기화
        for(int i = 1; i<=15;i++)
        {
            C_Item_FBS temp = new C_Item_FBS(i);
            C_Item_FBS temp2 = new C_Item_FBS(100000 + i);
            C_UserInfo.Instance.itemList.Add(temp);
            C_UserInfo.Instance.itemList.Add(temp2);
        }
    }
}
