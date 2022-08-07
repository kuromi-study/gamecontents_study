using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class C_UI_Inventory_Item : MonoBehaviour
{
    [Header("������ ����ó��")]
    [SerializeField] GameObject _select;
    [SerializeField] GameObject _equipped;
    [SerializeField] GameObject _locked;

    [Header("��� ����")]
    [SerializeField] Text _gradeTxt;
    [SerializeField] Text _enhanceTxt;

    [Header("������")]
    [SerializeField] Image _itemImg;

    [Header("�ϴ� ����")]
    [SerializeField] List<GameObject> _starList;
    [SerializeField] Text _numTxt;

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

    public void SetData(C_Item_FBS item)
    {
        var newInfo = C_ItemInfo.GetItemInfo(item.ItemUID) as ItemInfoEquip;

        _itemImg.sprite = Resources.Load<Sprite>(newInfo.ImagePath);

        _gradeTxt.text = newInfo.Grade.ToString();
        _enhanceTxt.text = newInfo.Enhance.ToString();
        //_numTxt.text = newInfo.Num.ToString();

        for (int i = 0; i < _starList.Count; i++)
        {
            if (i < newInfo.Star)
            {
                _starList[i].SetActive(true);
            }
            else
            {
                _starList[i].SetActive(false);
            }
        }

        IsSelect = false;
        IsEquip = newInfo.isEquip;
        IsLock = newInfo.isLock;
    }

    public void SetData(C_ItemInfo item)
    {
        var newInfo = C_ItemInfo.GetItemInfo(item.ItemUID);

        _gradeTxt.text = newInfo.Grade.ToString();
    }

    public void SetEmpty()
    {
        IsSelect = false;
        IsEquip = false;
        IsLock = false;
    }
}
