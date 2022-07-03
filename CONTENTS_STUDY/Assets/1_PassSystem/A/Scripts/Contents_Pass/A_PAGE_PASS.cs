using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class A_PAGE_PASS : MonoBehaviour
{
    enum tabType
    {
        NONE,
        PASS,
        MISSION,
    };

    [Header("상단 오브젝트")]
    [SerializeField] Button _backBtn;
    [SerializeField] Text _backBtnText;
    [SerializeField] Button _infoBtn;
    [SerializeField] Text _remainTimeText;
    [SerializeField] Button _purchaseBtn;
    [SerializeField] Text _purchaseBtnText;
    [SerializeField] Toggle _passTab;
    [SerializeField] Toggle _missionTab;

    [Header("리스트 오브젝트")]
    [SerializeField] GameObject _rewardPrefab;
    [SerializeField] GameObject _missionPrefab;
    [SerializeField] GameObject _lastReward;
    [SerializeField] GameObject _scrollView;

    tabType _tabType = tabType.PASS;

    [Header("지역변수")]
    int _seasonID;

    [Header("const변수용")]
    readonly int PASS_MISSION_TYPE = 2;

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
        _passTab?.onValueChanged.RemoveAllListeners();
        _missionTab?.onValueChanged.RemoveAllListeners();
    }

    void InitPage()
    {
        AddBtnListner();
        SetSeason();
        Refresh();
    }

    void AddBtnListner()
    {
        _backBtn?.onClick.AddListener(OnClickClose);
        _infoBtn?.onClick.AddListener(OnClickInfo);

        _passTab?.onValueChanged.AddListener((set) =>
        {
            if(set == true)
            {
                _tabType = tabType.PASS;
                RefreshScroll(_tabType);
            }
        });

        _missionTab?.onValueChanged.AddListener((set) =>
        {
            if (set == true)
            {
                _tabType = tabType.MISSION;
                RefreshScroll(_tabType);
            }
        });
    }

    void SetSeason()
    {
        // 데이터뒤져서 현재시간에 맞는 시즌id를 가져와야한다.

        var passmain = ExcelParser.Read("PASS_TABLE-PASSMAIN");

        // rewardList 뽑아오기
        _seasonID = 2;
    }

    void Refresh()
    {
        RefreshTopLayer();
        RefreshScroll(_tabType);
        RefreshBottomLayer();
    }

    void RefreshTopLayer()
    {
        _backBtnText.SetTextWithStringKey("ui_pass_001");

        var remainStr = A_StringManager.Instance.GetString("ui_pass_002");
        if(remainStr != null)
        {
            remainStr = string.Format(remainStr, "1", "시간");
            _remainTimeText.SetTextWithString(remainStr);
        }

        _purchaseBtnText.SetTextWithStringKey("ui_pass_003");
    }

    void RefreshScroll(tabType type)
    {
        ResetScroll();
        switch (type)
        {
            case tabType.PASS:
                _lastReward.SetActive(true);
                RefreshScrollPass();
                break;
            case tabType.MISSION:
                _lastReward.SetActive(false);
                RefreshScrollMission();
                break;
            case tabType.NONE:
                Debug.LogError($"CHECK TYPE {type.ToString()}");
                break;
        }
    }

    void RefreshScrollPass()
    {
        var passreward = ExcelParser.Read("PASS_TABLE-PASSREWARD");
        var rewardList = passreward.Values.Where(x => int.Parse(x["PASSMAIN_ID"].ToString()) == _seasonID).ToList();

        // 패스보상은 최종보상이 항상 최상단에 표시되기 때문에 전부 표시를 해서는 안됩니다.
        var count = rewardList.Count;
        for (int i = 0; i < count - 1; i++)
        {
            var it = rewardList[i];

            var passItem = Resources.Load<GameObject>("A_PAGE_PASS_PASSITEM");
            var passItemGO = Instantiate<GameObject>(passItem);
            passItemGO.transform.SetParent(_scrollView.transform);

            var script = passItemGO.GetComponent<A_PAGE_PASS_PASSITEM>();
            script.SetData(it);
        }

        var lastRewardData = rewardList[count - 1];
        var lastRewardScript = _lastReward.GetComponent<A_PAGE_PASS_PASSITEM>();
        lastRewardScript.SetData(lastRewardData);
    }

    void RefreshScrollMission()
    {
        var passreward = ExcelParser.Read("MISSION_TABLE-MISSIONMAIN");
        var rewardList = passreward.Values.Where(x => int.Parse(x["TYPE"].ToString()) == PASS_MISSION_TYPE).ToList();

        foreach (var it in rewardList)
        {
            var missionItem = Resources.Load<GameObject>("A_PAGE_PASS_MISSIONITEM");
            var missionItemGO = Instantiate<GameObject>(missionItem);
            missionItemGO.transform.SetParent(_scrollView.transform);

            var script = missionItemGO.GetComponent<A_PAGE_PASS_MISSIONITEM>();
            script.SetData(it);
        }
    }

    void ResetScroll()
    {
        var childCount = _scrollView.transform.childCount;
        for(int i = 0; i<childCount; i++)
        {
            Destroy(_scrollView.transform.GetChild(i).gameObject);
        }
    }

    void RefreshBottomLayer()
    {

    }

    #region 버튼 리스너 처리
    void OnClickClose()
    {
        _thispage.SetActive(false);
    }

    void OnClickInfo()
    {

    }
    #endregion
}
