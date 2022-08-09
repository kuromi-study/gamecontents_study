using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eItemCategory
{
    EQUIP = 1,
    ACC = 1001,
    POTION = 2001,
}
public class C_UI_Inventory_Item : MonoBehaviour
{
    [Header("버튼")]
    [SerializeField] Button _btn;

    [Header("아이템 상태처리")]
    [SerializeField] GameObject _select;
    [SerializeField] GameObject _equipped;
    [SerializeField] GameObject _locked;

    [Header("상단 정보")]
    [SerializeField] Text _gradeTxt;
    [SerializeField] Text _enhanceTxt;

    [Header("아이템")]
    [SerializeField] Image _itemImg;

    [Header("하단 정보")]
    [SerializeField] List<GameObject> _starList;
    [SerializeField] Text _numTxt;

    public C_ItemInfo ItemInfo;
    Action<C_ItemInfo> _onClick;

    public bool IsSelect
    {
        get => _select.activeSelf;
        set => _select.SetActive(value);
    }

    public bool IsEquip
    {
        get => _equipped.activeSelf;
        set => _equipped.SetActive(value);
    }

    public bool IsLock
    {
        get => _locked.activeSelf;
        set => _locked.SetActive(value);
    }

    public void SetData(C_Item_FBS item, Action<C_ItemInfo> onClick = null)
    {
        _onClick = onClick;
        _btn.onClick.RemoveAllListeners();
        _btn.onClick.AddListener(() =>
        {
            _onClick.Invoke(ItemInfo);
            IsSelect = true;
        });

        ItemInfo = C_ItemInfo.GetItemInfo(item.ItemUID);

        SetData(ItemInfo);
    }

    public void SetData(C_ItemInfo item)
    {
        var itemInfoConvert = item as ItemInfoEquip;

        _itemImg.sprite = Resources.Load<Sprite>(item.ImagePath);

        _gradeTxt.text = itemInfoConvert.Grade.ToString();
        _enhanceTxt.text = itemInfoConvert.Enhance.ToString();
        //_numTxt.text = newInfo.Num.ToString();

        for (int i = 0; i < _starList.Count; i++)
        {
            if (i < itemInfoConvert.Star)
            {
                _starList[i].SetActive(true);
            }
            else
            {
                _starList[i].SetActive(false);
            }
        }

        IsSelect = false;
        IsEquip = itemInfoConvert.isEquip;
        IsLock = itemInfoConvert.isLock;
    }

    public void SetEmpty()
    {
        IsSelect = false;
        IsEquip = false;
        IsLock = false;
    }
}
