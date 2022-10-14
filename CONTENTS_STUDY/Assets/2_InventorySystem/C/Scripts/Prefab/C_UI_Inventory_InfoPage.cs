using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class C_UI_Inventory_InfoPage : MonoBehaviour
{
    [Header("상단")]
    [SerializeField] Button _lockBtn;
    [SerializeField] C_UI_Inventory_Item _item;
    [SerializeField] Text _category01Txt;
    [SerializeField] Text _category02Txt;
    [SerializeField] Text _gradeTxt;

    [Header("하단")]
    [SerializeField] Text _nameTxt;
    [SerializeField] Text _desTxt;
    [SerializeField] Text _abiltiyTxt;
    [SerializeField] Button _equipBtn;

    private C_ItemInfo _iteminfo;

    public void InitItemInfoPage(C_ItemInfo iteminfo)
    {
        _iteminfo = iteminfo;

        InitButtonListner();
        RefreshTop();
        RefreshBottom();
    }

    private void InitButtonListner()
    {
        _lockBtn.onClick.RemoveAllListeners();
        _lockBtn.onClick.AddListener(OnClickLock);
        _equipBtn.onClick.RemoveAllListeners();
        _equipBtn.onClick.AddListener(OnClickEquip);
    }

    private void RefreshTop()
    {
        _item.SetData(_iteminfo);
        _category01Txt.SetTextWithStringKey(_iteminfo.MainCategoryString);
        _category02Txt.SetTextWithStringKey(_iteminfo.SubCategoryString);
        _gradeTxt.SetTextWithStringKey(_iteminfo.GradeString);
    }

    private void RefreshBottom()
    {
        _nameTxt.SetTextWithStringKey(_iteminfo.NameString);
        _desTxt.SetTextWithStringKey(_iteminfo.DescriptionString);
    }


    private void OnClickLock()
    {
        if(_iteminfo.isLock == true)
        {
            // 해제 처리를 수행한다.

        }
        else
        {
            // 잠금 처리를 수행한다.

        }
    }

    private void OnClickEquip()
    {
        if(_iteminfo.isEquip == true)
        {
            // 해제 처리를 수행한다.

        }
        else
        {
            // 장착 처리를 수행한다.

        }
    }
}
