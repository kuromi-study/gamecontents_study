using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_UI_InventoryPage : MonoBehaviour
{
    [Header("TOP")]
    Text backBtnTXT;
    Text titleTXT;
    Text cashTXT;
    public int cashAmount { get; private set; }

    [Header("MIDDLE")]

    [Header("LEFT")]


    bool bAllActive = true;

    bool bEquipmentActive = false;
    bool bAccessoriesActive = true;
    bool bConsumableActive = true;
    // [Header("RIGHT")]

    /*
     * 스크립트
     * 
     page 관리 -> C_UI_Inventory_Page, 재화량 관리, 아이템 고정 및 위치
     아이템 관리 -> 장착중, 선택중, 잠김 이런거 관리
     아이템 정보 읽어오는 거 -> 구조체 정의하기
     강화, 태생, 승급, 합성 관리하는 스크립트 -> 4개
     */

    private void setCash(float amount)
    {
        // 골드표현은 국제표기에 맞게 간소화된 형태를 사용한다. 짤린 숫자는 버림(Floor) 처리한다.

        cashAmount = Mathf.RoundToInt(cashAmount+amount);
        cashTXT.text = cashAmount.ToString();
    }




    #region Tab
    public void ToggleAllTab()
    {
        /*if (bAllActive) return;

        bAllActive = true;*/

        ToggleEquipment();
        ToggleAccessories();
        ToggleConsumable();
    }

    public void ToggleEquipment()
    {
        if (bEquipmentActive) return;

        bEquipmentActive = true;
    }
    public void ToggleAccessories()
    {
        if (bAccessoriesActive) return;

        bAccessoriesActive = true;
    }
    public void ToggleConsumable()
    {
        if (bConsumableActive) return;

        bConsumableActive = true;
    }

    public void SetActiveTab(Text text_, Color color_)
    {
        // 텍스트 색 변화
        text_.color = color_;
    }

    #endregion


}
