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
    [Header("상단바영역")]
    [SerializeField] Button _backBtn;
    [SerializeField] Text _titleTxt;
    [SerializeField] Text _currencyTxt;

    [Header("좌측_인벤토리영역")]
    [SerializeField] List<C_ToggleTab> _toggleList;
    [SerializeField] GameObject _scrollContent;
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] Text _numTxt;
    [SerializeField] Button _abandonBtn;

    readonly int LIST_MAX_COUNT = 300;

    static GameObject _thispage;

    public static void Open()
    {
        // 인벤토리 버튼 클릭했을 경우
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
    /// 버튼 리스너 등록
    /// </summary>
    private void SetBtnListner()
    {
        _backBtn.onClick.RemoveAllListeners();
        _backBtn.onClick.AddListener(OnClickBack);

        _abandonBtn.onClick.RemoveAllListeners();
        _abandonBtn.onClick.AddListener(OnClickAbandon);
    }

    #region 상단바 처리영역
    /// <summary>
    /// 뒤로가기 버튼 클릭시 처리
    /// </summary>
    private void OnClickBack()
    {
        //페이지를 닫는다.
        _thispage.SetActive(false);
    }

    /// <summary>
    /// 현재 재화 갱신
    /// </summary>
    private void RefreshCurrency()
    {
        // 재화를 갱신한다.
        _currencyTxt.text = C_UserInfo.Instance.Currency.ToString();
    }
    #endregion

    #region 인벤토리 처리 영역
    /// <summary>
    /// 인벤토리 탭 갱신
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
    /// 인벤토리 아이템 리스트 갱신
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
                // 아이템 생성시켜줘야한다
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
    /// 인벤토리 아이템 갯수 갱신
    /// </summary>
    private void RefreshNumText()
    {
        // 스트링을 가져와서 갈아끼워야한다.
        _numTxt.text = C_UserInfo.Instance.itemList.Count.ToString();
    }

    /// <summary>
    /// 버리기 버튼 클릭시 처리
    /// </summary>
    private void OnClickAbandon()
    {
        // 아이템 삭제처리
    }
    #endregion
}
