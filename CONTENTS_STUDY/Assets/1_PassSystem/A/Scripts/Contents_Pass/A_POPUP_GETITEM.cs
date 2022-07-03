using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class A_POPUP_GETITEM : MonoBehaviour
{
    [SerializeField] GameObject _scrollView;
    [SerializeField] GameObject _itemImagePrefab;
    [SerializeField] Button _closeBtn;

    List<string> _rewardList;

    static GameObject _thisPopup;

    public static void Open(List<string> rewardList)
    {
        // 패스버튼을 클릭했을경우
        // 패스페이지를 출력해야한다.
        if (_thisPopup == null)
        {
            var uiroot = GameObject.Find("UIRoot");

            var passui = Resources.Load<GameObject>("A_POPUP_GETITEM");
            _thisPopup = Instantiate<GameObject>(passui);
            _thisPopup.transform.SetParent(uiroot.transform);

            var recttransform = _thisPopup.GetComponent<RectTransform>();
            recttransform.sizeDelta = Vector2.zero;
            recttransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            _thisPopup.SetActive(true);
        }

        _thisPopup.GetComponent<A_POPUP_GETITEM>().InitPopup(rewardList);
    }

    void OnDisable()
    {
        _closeBtn?.onClick.RemoveAllListeners();
    }

    void InitPopup(List<string> rewardList)
    {
        _rewardList = rewardList;
        AddBtnListner();

        // 아이템 이미지 갱신용
        ResetScroll();
        RefreshScroll();
    }

    void ResetScroll()
    {
        var childCount = _scrollView.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(_scrollView.transform.GetChild(i).gameObject);
        }
    }
    
    void RefreshScroll()
    {
        foreach (var it in _rewardList)
        {
            var missionItem = Resources.Load<GameObject>("A_POPUP_GETITEM_ITEM");
            var missionItemGO = Instantiate<GameObject>(missionItem);
            missionItemGO.transform.SetParent(_scrollView.transform);

            var script = missionItemGO.GetComponent<A_POPUP_GETITEM_ITEM>();
            script.SetData(it);
        }
    }

    void AddBtnListner()
    {
        _closeBtn?.onClick.AddListener(OnClickClose);
    }

    void OnClickClose()
    {
        _thisPopup.SetActive(false);
    }
}
