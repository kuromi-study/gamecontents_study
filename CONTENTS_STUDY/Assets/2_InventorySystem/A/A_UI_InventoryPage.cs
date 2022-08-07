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
        // ��ư �ؽ�Ʈ
        backBtnTXT.text = D_StringkeyManager.Instance.GetString("ui_pass_001");
        titleTXT.text = D_StringkeyManager.Instance.GetString("ui_title_001");
        
    }


    private void UpdateCash(float amount)
    {
        // ���ǥ���� ����ǥ�⿡ �°� ����ȭ�� ���¸� ����Ѵ�. ©�� ���ڴ� ����(Floor) ó���Ѵ�.

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
        // �ؽ�Ʈ �� ��ȭ
        text_.color = color_;
    }

    #endregion


}
