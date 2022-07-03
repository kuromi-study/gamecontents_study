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
    [SerializeField] Button _infoBtn;
    [SerializeField] Text _remainTimeText;
    [SerializeField] Button _purchaseBtn;
    [SerializeField] Toggle _passTab;
    [SerializeField] Toggle _missionTab;

    [Header("리스트 오브젝트")]
    [SerializeField] GameObject _rewardPrefab;
    [SerializeField] GameObject _missionPrefab;
    [SerializeField] GameObject _lastReward;
    [SerializeField] GameObject _scrollView;

    [Header("하단 오브젝트")]
    [SerializeField] Button _lvUpBtn;
    [SerializeField] Text _lvText;
    [SerializeField] Text _pointText;
    [SerializeField] Image _gaugeImg;
    [SerializeField] Button _getAllBtn;

    tabType _tabType = tabType.PASS;

    [Header("지역변수")]
    int _seasonID;
    int _needBeforePoint;
    Dictionary<string, object> nowLevelData = new Dictionary<string, object>();
    Dictionary<string, object> nowPassData = new Dictionary<string, object>();

    [Header("const변수용")]
    readonly int PASS_MISSION_TYPE = 2;
    readonly int GAUGE_SIZE_X = 550;

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
        A_PassInfo.Instance.RemoveEventAll(A_PassInfo.Instance.PASS_EVENT_NAME);

        _backBtn?.onClick.RemoveAllListeners();
        _infoBtn?.onClick.RemoveAllListeners();
        _purchaseBtn?.onClick.RemoveAllListeners();

        _passTab?.onValueChanged.RemoveAllListeners();
        _missionTab?.onValueChanged.RemoveAllListeners();
    }

    void InitPage()
    {
        // 진입시에는 패스탭이 On되도록
        _passTab.isOn = true;

        // 이벤트등록
        A_PassInfo.Instance.AddEvent(A_PassInfo.Instance.PASS_EVENT_NAME, Refresh);

        AddBtnListner();
        SetSeason();
        SetTabType(tabType.PASS);

        Refresh();
    }

    void AddBtnListner()
    {
        _backBtn?.onClick.AddListener(OnClickClose);
        _infoBtn?.onClick.AddListener(OnClickInfo);
        _purchaseBtn?.onClick.AddListener(OnClickPurchase);

        _passTab?.onValueChanged.AddListener((set) =>
        {
            if(set == true)
            {
                SetTabType(tabType.PASS);
            }
        });

        _missionTab?.onValueChanged.AddListener((set) =>
        {
            if (set == true)
            {
                SetTabType(tabType.MISSION);
            }
        });
    }

    void SetSeason()
    {
        // 데이터뒤져서 현재시간에 맞는 시즌id를 가져와야한다.
        var passmain = ExcelParser.Read("PASS_TABLE-PASSMAIN");
        
        nowPassData = passmain.Where(x => int.Parse(x.Key.ToString()) == 2)
            .Select(x=>x.Value).First();

        // rewardList 뽑아오기
        _seasonID = 2;
    }

    void SetTabType(tabType type)
    {
        _tabType = type;
        RefreshScroll(_tabType);
    }

    void Refresh()
    {
        RefreshTopLayer();
        RefreshScroll(_tabType);
        RefreshBottomLayer();
    }

    void RefreshTopLayer()
    {
        _remainTimeText.SetTextForRemainTime("20220707000000");
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
        var passLevelTable = ExcelParser.Read("PASS_TABLE-PASSLEVEL");
        var rewardList = passreward.Values.Where(x => int.Parse(x["PASSMAIN_ID"].ToString()) == _seasonID).ToList();

        // 패스보상은 최종보상이 항상 최상단에 표시되기 때문에 전부 표시를 해서는 안됩니다.
        var count = rewardList.Count;

        int needAllPoint = 0;

        for (int i = 0; i < count - 1; i++)
        {
            var it = rewardList[i];

            var passLevelData = passLevelTable[it["PASSLEVEL_ID"].ToString()];
            var needPoint = int.Parse(passLevelData["NEEDPOINT"].ToString());

            if (i > 0)
            { 
                // 현재단계 도달까지 필요한 포인트를 저장한다.
                needAllPoint += needPoint;
            }

            if(A_PassInfo.Instance.Point < needAllPoint
                && A_PassInfo.Instance.Point >= needAllPoint - needPoint)
            {
                // 현재 필요한 포인트 저장용.
                _needBeforePoint = needAllPoint - needPoint;
                nowLevelData = passLevelData;
            }

            var passItem = Resources.Load<GameObject>("A_PAGE_PASS_PASSITEM");
            var passItemGO = Instantiate<GameObject>(passItem);
            passItemGO.transform.SetParent(_scrollView.transform);

            var script = passItemGO.GetComponent<A_PAGE_PASS_PASSITEM>();
            script.SetData(it, needAllPoint, ShowDiffReward);
        }

        var lastRewardData = rewardList[count - 1];
        var lastRewardScript = _lastReward.GetComponent<A_PAGE_PASS_PASSITEM>();
        var needLastPoint = int.Parse(passLevelTable[lastRewardData["PASSLEVEL_ID"].ToString()]["NEEDPOINT"].ToString());
        needAllPoint += needLastPoint;
        lastRewardScript.SetData(lastRewardData,needAllPoint, ShowDiffReward);
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
        // 레벨
        var levelstr = A_StringManager.Instance.GetString("ui_pass_005");
        levelstr = string.Format(levelstr, "");
        _lvText.text = "";

        // 포인트
        var pointstr = A_StringManager.Instance.GetString("ui_pass_006");
        var needPoint = _needBeforePoint + int.Parse(nowLevelData["NEEDPOINT"].ToString());

        pointstr = string.Format(pointstr, A_PassInfo.Instance.PointString, needPoint.ToString());
        _pointText.text = pointstr;

        // 게이지
        var forGaugePercent = (float)A_PassInfo.Instance.Point / (float)needPoint;
        var sizey = _gaugeImg.rectTransform.sizeDelta.y;
        _gaugeImg.rectTransform.sizeDelta = new Vector2(GAUGE_SIZE_X * forGaugePercent, sizey);
    }

    #region 버튼 리스너 처리
    void OnClickClose()
    {
        _thispage.SetActive(false);
    }

    void OnClickInfo()
    {
        A_POPUP_PASSINFO.Open(nowPassData["description"].ToString());
    }

    void OnClickPurchase()
    {

    }

    void ShowDiffReward(int level)
    {
        // 진짜 귀찮지만 그냥 노말보상만 하나하나 정리해보자...
        List<string> rewardItemList = new List<string>();

        var beforeStep = A_PassInfo.Instance.Step + 1;
        var nowStep = level;

        // beforeStep ~ nowStep 까지 획득처리를 해야한다.
        var passreward = ExcelParser.Read("PASS_TABLE-PASSREWARD");
        var passLevelTable = ExcelParser.Read("PASS_TABLE-PASSLEVEL");

        // 시즌이 같고 레벨이 사이에있는 아이템ID를 가져온다.
        rewardItemList = passreward.Values.
            Where(x => int.Parse(x["PASSMAIN_ID"].ToString()) == _seasonID
            && int.Parse(passLevelTable[x["PASSLEVEL_ID"].ToString()]["LEVEL"].ToString()) <= nowStep
            && int.Parse(passLevelTable[x["PASSLEVEL_ID"].ToString()]["LEVEL"].ToString()) >= beforeStep).
            Select(x => x["NORMAL_REWARD_ID"].ToString()).ToList();

        A_POPUP_GETITEM.Open(rewardItemList);
    }
    #endregion
}
