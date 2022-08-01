using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo
{
    public uint ItemUID { get; set; }
    public int Grade { get; set; }
    public int Enhance { get; set; }
    public int Star { get; set; }
    public int Num { get; set; }
}

public class C_UI_Inventory_Item : MonoBehaviour
{
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

    public void SetData(ItemInfo iteminfo)
    {
        _gradeTxt.text = iteminfo.Grade.ToString();
        _enhanceTxt.text = iteminfo.Enhance.ToString();
        _numTxt.text = iteminfo.Num.ToString();
        
        for(int i = 0; i<_starList.Count;i++)
        {
            if(i<iteminfo.Star)
            {
                _starList[i].SetActive(true);
            }
            else
            {
                _starList[i].SetActive(false);
            }
        }
    }
}
