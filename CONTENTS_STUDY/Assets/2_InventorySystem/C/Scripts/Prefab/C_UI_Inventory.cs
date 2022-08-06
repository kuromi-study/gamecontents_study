using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public enum eToggleType_C
{
    ALL,
    EQUIP,
    ACCESSORIES,
    PORTION,
}

[Serializable]
public class C_ToggleTab
{
    public Toggle _toggle;
    public eToggleType_C _type;
}

public class C_UI_Inventory : MonoBehaviour
{
    [Header("��ܹٿ���")]
    [SerializeField] Button _backBtn;
    [SerializeField] Text _titleTxt;
    [SerializeField] Text _currencyTxt;

    [Header("����_�κ��丮����")]
    [SerializeField] List<C_ToggleTab> _toggleList;
    [SerializeField] GameObject _scrollContent;
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] Text _numTxt;
    [SerializeField] Button _abandonBtn;

    readonly int LIST_MAX_COUNT = 300;

    static GameObject _thispage;

    public static void Open()
    {
        // �κ��丮 ��ư Ŭ������ ���
        if (_thispage == null)
        {
            var uiroot = GameObject.Find("UIRoot");

            var passui = Resources.Load<GameObject>("C_UI_Inventory");
            _thispage = Instantiate<GameObject>(passui);
            _thispage.transform.SetParent(uiroot.transform);

            var recttransform = _thispage.GetComponent<RectTransform>();
            recttransform.sizeDelta = Vector2.zero;
            recttransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            _thispage.SetActive(true);
        }

        _thispage.GetComponent<C_UI_Inventory>().InitPage();
    }

    public void InitPage()
    {
        RefreshCurrency();

        InitInventoryTab();
        RefreshNumText();

        SetBtnListner();
    }

    /// <summary>
    /// ��ư ������ ���
    /// </summary>
    private void SetBtnListner()
    {
        _backBtn.onClick.RemoveAllListeners();
        _backBtn.onClick.AddListener(OnClickBack);

        _abandonBtn.onClick.RemoveAllListeners();
        _abandonBtn.onClick.AddListener(OnClickAbandon);
    }

    #region ��ܹ� ó������
    /// <summary>
    /// �ڷΰ��� ��ư Ŭ���� ó��
    /// </summary>
    private void OnClickBack()
    {
        //�������� �ݴ´�.
        _thispage.SetActive(false);
    }

    /// <summary>
    /// ���� ��ȭ ����
    /// </summary>
    private void RefreshCurrency()
    {
        // ��ȭ�� �����Ѵ�.
        _currencyTxt.text = C_UserInfo.Instance.Currency.ToString();
    }
    #endregion

    #region �κ��丮 ó�� ����
    /// <summary>
    /// �κ��丮 �� ����
    /// </summary>
    private void InitInventoryTab()
    {
        foreach(var it in _toggleList)
        {
            it._toggle.onValueChanged.RemoveAllListeners();
            it._toggle.onValueChanged.AddListener(RefreshInventoryList);
        }

        var toggleClass = _toggleList.Where(x => x._type == eToggleType_C.ALL).FirstOrDefault();
        if (toggleClass == null)
        {
            Debug.LogError("toggle List is null");
            return;
        }

        var toggle = toggleClass._toggle;
        if(toggle == null)
        {
            Debug.LogError("toggle is null");
            return;
        }

        toggle.onValueChanged.Invoke(toggle.isOn);
    }
    
    /// <summary>
    /// �κ��丮 ������ ����Ʈ ����
    /// </summary>
    private void RefreshInventoryList(bool set)
    {
        var toggleClass = _toggleList.Where(x => x._toggle.isOn == true).FirstOrDefault();
        var type = toggleClass._type;

        switch(type)
        {
            case eToggleType_C.ALL:
                RefreshInventoryALL();
                break;
            case eToggleType_C.EQUIP:
                RefreshInventoryEquip();
                break;
            case eToggleType_C.ACCESSORIES:
                RefreshInventoryAcc();
                break;
            case eToggleType_C.PORTION:
                RefreshInventoryPotion();
                break;
        }

        Debug.Log($"{type} is On");
    }

    private void RefreshInventoryALL()
    {
        var listCount = C_UserInfo.Instance.itemList.Count;
        var childCount = _scrollContent.transform.childCount;

        for (int i = 0; i < LIST_MAX_COUNT; i++)
        {
            GameObject item;
            if(i<childCount)
            {
                item = _scrollContent.transform.GetChild(i).gameObject;
                item.SetActive(true);
            }
            else
            {
                // ������ ������������Ѵ�
                item = Instantiate<GameObject>(_itemPrefab);
                item.transform.SetParent(_scrollContent.transform);
            }

            var script = item.GetComponent<C_UI_Inventory_Item>();
            if (i<listCount)
            {
                script.SetData(C_UserInfo.Instance.itemList[i]);
            }
            else
            {
                script.SetEmpty();
            }
        }
    }

    private void RefreshInventoryEquip()
    {
        var listCount = C_UserInfo.Instance.itemList.Count;
        for (int i = 0; i < LIST_MAX_COUNT; i++)
        {
            var item = _scrollContent.transform.GetChild(i);

            if(i>=listCount)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    private void RefreshInventoryAcc()
    {
        var listCount = C_UserInfo.Instance.itemList.Count;
        for (int i = 0; i < LIST_MAX_COUNT; i++)
        {
            var item = _scrollContent.transform.GetChild(i);

            if (i >= listCount)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    private void RefreshInventoryPotion()
    {
        var listCount = C_UserInfo.Instance.itemList.Count;
        for (int i = 0; i < LIST_MAX_COUNT; i++)
        {
            var item = _scrollContent.transform.GetChild(i);

            if (i >= listCount)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// �κ��丮 ������ ���� ����
    /// </summary>
    private void RefreshNumText()
    {
        // ��Ʈ���� �����ͼ� ���Ƴ������Ѵ�.
        _numTxt.text = C_UserInfo.Instance.itemList.Count.ToString();
    }

    /// <summary>
    /// ������ ��ư Ŭ���� ó��
    /// </summary>
    private void OnClickAbandon()
    {
        // ������ ����ó��
    }
    #endregion
}
