using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A_PAGE_PASS : MonoBehaviour
{
    [Header("상단 오브젝트")]
    [SerializeField] Button _backBtn;
    [SerializeField] Button _infoBtn;
    [SerializeField] Text _remainTime;
    [SerializeField] Button _purchaseBtn;

    [Header("리스트 오브젝트")]
    [SerializeField] GameObject _rewardPrefab;
    [SerializeField] GameObject _missionPrefab;
    [SerializeField] GameObject _lastReward;
    [SerializeField] GameObject _scrollView;

    static GameObject _thispage;

    public static void Open()
    {
        // 패스버튼을 클릭했을경우
        // 패스페이지를 출력해야한다.
        if (_thispage == null)
        {
            var uiroot = GameObject.Find("UIRoot");

            var passui = Resources.Load<GameObject>("A_PAGE_PASS");
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

        _thispage.GetComponent<A_PAGE_PASS>().InitPage();
    }

    void OnDisable()
    {
        _backBtn?.onClick.RemoveAllListeners();
        _infoBtn?.onClick.RemoveAllListeners();
    }

    void InitPage()
    {
        _backBtn?.onClick.AddListener(OnClickClose);
        _infoBtn?.onClick.AddListener(OnClickInfo);
    }

    void OnClickClose()
    {
        _thispage.SetActive(false);
    }

    void OnClickInfo()
    {

    }
}
