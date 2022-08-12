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

public enum eToggleMiddleType_C
{
    INFO,
    ENHANCE,
    GRADEUP,
    COMPOSE
}

[Serializable]
public class C_ToggleTab
{
    public Toggle _toggle;
    public eToggleType_C _type;
}

[Serializable]
public class C_ToggleTabMiddle
{
    public Toggle _toggle;
    public eToggleMiddleType_C _type;
}

public class C_UI_Inventory : MonoBehaviour
{
    [Header("��ܹٿ���")]
    [SerializeField] Button _backBtn;
    [SerializeField] Text _titleTxt;
    [SerializeField] Text _currencyTxt;

    [Header("����_�κ��丮����")]
    [SerializeField] List<C_ToggleTab> _toggleInvenList;
    [SerializeField] GameObject _scrollContent;
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] Text _numTxt;
    [SerializeField] Button _abandonBtn;

    [Header("�߾�_�����۰���������")]
    [SerializeField] List<C_ToggleTabMiddle> _toggleMiddleList;
    [SerializeField] C_UI_Inventory_InfoPage _infoPage;
    [SerializeField] C_UI_Inventory_EnhancePage _enhancePage;
    [SerializeField] C_UI_Inventory_GradeUpPage _gradePage;
    [SerializeField] C_UI_Inventory_ComposePage _composePage;

    C_UI_Inventory_Item _beforeItem;

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
        InitMiddleTab();
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
        foreach(var it in _toggleInvenList)
        {
            it._toggle.onValueChanged.RemoveAllListeners();
            it._toggle.onValueChanged.AddListener(RefreshInventoryList);
        }

        var toggleClass = _toggleInvenList.Where(x => x._type == eToggleType_C.ALL).FirstOrDefault();
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
        var toggleClass = _toggleInvenList.Where(x => x._toggle.isOn == true).FirstOrDefault();
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
                script.SetData(C_UserInfo.Instance.itemList[i], OnClickItem);
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
            var script = item.GetComponent<C_UI_Inventory_Item>();

            if(script == null)
            {
                item.gameObject.SetActive(false);
                continue;
            }

            if(script.ItemInfo == null)
            {
                item.gameObject.SetActive(false);
                continue;
            }

            if(int.Parse(script.ItemInfo.MainCategory) == (int)eItemCategory.EQUIP)
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    private void RefreshInventoryAcc()
    {
        var listCount = C_UserInfo.Instance.itemList.Count;
        for (int i = 0; i < LIST_MAX_COUNT; i++)
        {
            var item = _scrollContent.transform.GetChild(i);
            var script = item.GetComponent<C_UI_Inventory_Item>();

            if (script == null)
            {
                item.gameObject.SetActive(false);
                continue;
            }

            if (script.ItemInfo == null)
            {
                item.gameObject.SetActive(false);
                continue;
            }

            if (int.Parse(script.ItemInfo.MainCategory) == (int)eItemCategory.ACC)
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    private void RefreshInventoryPotion()
    {
        for (int i = 0; i < LIST_MAX_COUNT; i++)
        {
            var item = _scrollContent.transform.GetChild(i);
            var script = item.GetComponent<C_UI_Inventory_Item>();

            if (script == null)
            {
                item.gameObject.SetActive(false);
                continue;
            }

            if (script.ItemInfo == null)
            {
                item.gameObject.SetActive(false);
                continue;
            }

            if (int.Parse(script.ItemInfo.MainCategory) == (int)eItemCategory.POTION)
            {
                item.gameObject.SetActive(true);
            }
            else
            {
                item.gameObject.SetActive(false);
            }
        }
    }



    private void InitMiddleTab()
    {
        foreach (var it in _toggleMiddleList)
        {
            it._toggle.onValueChanged.RemoveAllListeners();
            it._toggle.onValueChanged.AddListener(RefreshMiddlePage);
        }

        SetFirstMiddleTab();
    }

    private void SetFirstMiddleTab()
    {
        var toggleClass = _toggleMiddleList.Where(x => x._type == eToggleMiddleType_C.INFO).FirstOrDefault();
        if (toggleClass == null)
        {
            Debug.LogError("toggle List is null");
            return;
        }

        var toggle = toggleClass._toggle;
        if (toggle == null)
        {
            Debug.LogError("toggle is null");
            return;
        }

        toggle.isOn = true;
    }

    private void RefreshMiddlePage(bool set)
    {
        var toggleClass = _toggleMiddleList.Where(x => x._toggle.isOn == true).FirstOrDefault();
        var type = toggleClass._type;

        _infoPage.gameObject.SetActive(type == eToggleMiddleType_C.INFO);
        _enhancePage.gameObject.SetActive(type == eToggleMiddleType_C.ENHANCE);
        _gradePage.gameObject.SetActive(type == eToggleMiddleType_C.GRADEUP);
        _composePage.gameObject.SetActive(type == eToggleMiddleType_C.COMPOSE);

        switch(type)
        {
            case eToggleMiddleType_C.INFO:
                _infoPage.InitItemInfoPage(_beforeItem.ItemInfo);
                break;
            case eToggleMiddleType_C.ENHANCE:
                _enhancePage.InitItemInfoPage(_beforeItem.ItemInfo);
                break;
            case eToggleMiddleType_C.GRADEUP:
                _gradePage.InitItemInfoPage(_beforeItem.ItemInfo);
                break;
            case eToggleMiddleType_C.COMPOSE:
                _composePage.InitItemInfoPage(_beforeItem.ItemInfo);
                break;
        }

        Debug.Log($"{type} is On");
    }

    /// <summary>
    /// �κ��丮 ������ ���� ����
    /// </summary>
    private void RefreshNumText()
    {
        // ��Ʈ���� �����ͼ� ���Ƴ������Ѵ�.
        var temp = A_StringManager.Instance.GetString("ui_inventory_001");
        _numTxt.text = string.Format(temp, C_UserInfo.Instance.itemList.Count, 300);
    }

    /// <summary>
    /// ������ ��ư Ŭ���� ó��
    /// </summary>
    private void OnClickAbandon()
    {
        // ������ ����ó��
    }

    public void OnClickItem(C_UI_Inventory_Item item)
    {
        // ó�� �����������̸�
        if(item != _beforeItem)
        {
            // ������������ ����
            SetFirstMiddleTab();
            // ���������� ����
            _infoPage.InitItemInfoPage(item.ItemInfo);

            if(_beforeItem != null)
            {
                _beforeItem.IsSelect = false;
            }
            _beforeItem = item;
        }
        else
        {
            // ������ ���ȴ� �������̸�
            // ����ó�� ���� �ʴ´�.
        }
    }
    #endregion
}
