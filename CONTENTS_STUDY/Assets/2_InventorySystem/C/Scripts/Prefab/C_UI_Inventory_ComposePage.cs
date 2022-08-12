using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class C_UI_Inventory_ComposePage : MonoBehaviour
{
    [Header("상단")]
    [SerializeField] C_UI_Inventory_Item _item;
    [SerializeField] Text _category01Txt;
    [SerializeField] Text _category02Txt;
    [SerializeField] Text _gradeTxt;

    [Header("하단")]
    [SerializeField] Text _nameTxt;
    [SerializeField] Text _desTxt;
    [SerializeField] Text _abiltiyTxt;
    [SerializeField] C_UI_Inventory_Item _needItem;
    [SerializeField] Button _gradeupBtn;

    private C_ItemInfo _iteminfo;

    public void InitItemInfoPage(C_ItemInfo iteminfo)
    {
        _iteminfo = iteminfo;

        RefreshTop();
        RefreshBottom();
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
}
