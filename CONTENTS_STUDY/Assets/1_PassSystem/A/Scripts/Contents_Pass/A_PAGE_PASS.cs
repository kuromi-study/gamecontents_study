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

    [Header("��� ������Ʈ")]
    [SerializeField] Button _backBtn;
    [SerializeField] Button _infoBtn;
    [SerializeField] Text _remainTimeText;
    [SerializeField] Button _purchaseBtn;
    [SerializeField] Toggle _passTab;
    [SerializeField] Toggle _missionTab;

    [Header("����Ʈ ������Ʈ")]
    [SerializeField] GameObject _rewardPrefab;
    [SerializeField] GameObject _missionPrefab;
    [SerializeField] GameObject _lastReward;
    [SerializeField] GameObject _scrollView;

    [Header("�ϴ� ������Ʈ")]
    [SerializeField] Button _lvUpBtn;
    [SerializeField] Text _lvText;
    [SerializeField] Text _pointText;
    [SerializeField] Image _gaugeImg;
    [SerializeField] Button _getAllBtn;

    tabType _tabType = tabType.PASS;

    [Header("��������")]
    int _seasonID;
    int _needBeforePoint;
    Dictionary<string, object> nowLevelData = new Dictionary<string, object>();

    [Header("const������")]
    readonly int PASS_MISSION_TYPE = 2;
    readonly int GAUGE_SIZE_X = 550;

    static GameObject _thispage;

    public static void Open()
    {
        // �н���ư�� Ŭ���������
        // �н��������� ����ؾ��Ѵ�.
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
        _passTab?.onValueChanged.RemoveAllListeners();
        _missionTab?.onValueChanged.RemoveAllListeners();
    }

    void InitPage()
    {
        // ���Խÿ��� �н����� On�ǵ���
        _passTab.isOn = true;

        // �̺�Ʈ���
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
        // �����͵����� ����ð��� �´� ����id�� �����;��Ѵ�.
        var passmain = ExcelParser.Read("PASS_TABLE-PASSMAIN");

        // rewardList �̾ƿ���
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

        // �н������� ���������� �׻� �ֻ�ܿ� ǥ�õǱ� ������ ���� ǥ�ø� �ؼ��� �ȵ˴ϴ�.
        var count = rewardList.Count;

        int needAllPoint = 0;

        for (int i = 0; i < count - 1; i++)
        {
            var it = rewardList[i];

            var passLevelData = passLevelTable[it["PASSLEVEL_ID"].ToString()];
            var needPoint = int.Parse(passLevelData["NEEDPOINT"].ToString());

            if (i > 0)
            { 
                // ����ܰ� ���ޱ��� �ʿ��� ����Ʈ�� �����Ѵ�.
                needAllPoint += needPoint;
            }

            if(A_PassInfo.Instance.Point < needAllPoint
                && A_PassInfo.Instance.Point >= needAllPoint - needPoint)
            {
                // ���� �ʿ��� ����Ʈ �����.
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
        // ����
        var levelstr = A_StringManager.Instance.GetString("ui_pass_005");
        levelstr = string.Format(levelstr, "");
        _lvText.text = "";

        // ����Ʈ
        var pointstr = A_StringManager.Instance.GetString("ui_pass_006");
        var needPoint = _needBeforePoint + int.Parse(nowLevelData["NEEDPOINT"].ToString());

        pointstr = string.Format(pointstr, A_PassInfo.Instance.PointString, needPoint.ToString());
        _pointText.text = pointstr;

        // ������
        var forGaugePercent = (float)A_PassInfo.Instance.Point / (float)needPoint;
        var sizey = _gaugeImg.rectTransform.sizeDelta.y;
        _gaugeImg.rectTransform.sizeDelta = new Vector2(GAUGE_SIZE_X * forGaugePercent, sizey);
    }

    #region ��ư ������ ó��
    void OnClickClose()
    {
        _thispage.SetActive(false);
    }

    void OnClickInfo()
    {

    }

    void ShowDiffReward(int level)
    {
        // ��¥ �������� �׳� �븻���� �ϳ��ϳ� �����غ���...
        List<string> rewardItemList = new List<string>();

        var beforeStep = A_PassInfo.Instance.Step + 1;
        var nowStep = level;

        // beforeStep ~ nowStep ���� ȹ��ó���� �ؾ��Ѵ�.
        var passreward = ExcelParser.Read("PASS_TABLE-PASSREWARD");
        var passLevelTable = ExcelParser.Read("PASS_TABLE-PASSLEVEL");

        // ������ ���� ������ ���̿��ִ� ������ID�� �����´�.
        rewardItemList = passreward.Values.
            Where(x => int.Parse(x["PASSMAIN_ID"].ToString()) == _seasonID
            && int.Parse(passLevelTable[x["PASSLEVEL_ID"].ToString()]["LEVEL"].ToString()) <= nowStep
            && int.Parse(passLevelTable[x["PASSLEVEL_ID"].ToString()]["LEVEL"].ToString()) >= beforeStep).
            Select(x => x["NORMAL_REWARD_ID"].ToString()).ToList();

        A_POPUP_GETITEM.Open(rewardItemList);
    }
    #endregion
}