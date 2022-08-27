using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A_UI_InventoryPage : MonoBehaviour
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

    public void InventoryOpen()
    {
        // 버튼 텍스트
        backBtnTXT.text = D_StringkeyManager.Instance.GetString("ui_pass_001");
        titleTXT.text = D_StringkeyManager.Instance.GetString("ui_title_001");
        
    }


    private void UpdateCash(float amount)
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
