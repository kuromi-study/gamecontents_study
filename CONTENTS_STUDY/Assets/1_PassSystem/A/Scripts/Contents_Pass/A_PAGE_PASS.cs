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
    [SerializeField] Text _backBtnText;
    [SerializeField] Button _infoBtn;
    [SerializeField] Text _remainTimeText;
    [SerializeField] Button _purchaseBtn;
    [SerializeField] Text _purchaseBtnText;
    [SerializeField] Toggle _passTab;
    [SerializeField] Toggle _missionTab;

    [Header("����Ʈ ������Ʈ")]
    [SerializeField] GameObject _rewardPrefab;
    [SerializeField] GameObject _missionPrefab;
    [SerializeField] GameObject _lastReward;
    [SerializeField] GameObject _scrollView;

    tabType _tabType = tabType.PASS;

    [Header("��������")]
    int _seasonID;

    [Header("const������")]
    readonly int PASS_MISSION_TYPE = 2;

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
        // �����͵����� ����ð��� �´� ����id�� �����;��Ѵ�.

        var passmain = ExcelParser.Read("PASS_TABLE-PASSMAIN");

        // rewardList �̾ƿ���
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
            remainStr = string.Format(remainStr, "1", "�ð�");
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

        foreach (var it in rewardList)
        {
            var passItem = Resources.Load<GameObject>("A_PAGE_PASS_PASSITEM");
            var passItemGO = Instantiate<GameObject>(passItem);
            passItemGO.transform.SetParent(_scrollView.transform);

            var script = passItemGO.GetComponent<A_PAGE_PASS_PASSITEM>();
            script.SetData(it);
        }
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

    #region ��ư ������ ó��
    void OnClickClose()
    {
        _thispage.SetActive(false);
    }

    void OnClickInfo()
    {

    }
    #endregion
}
