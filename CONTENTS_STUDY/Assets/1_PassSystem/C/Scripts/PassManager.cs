using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utility.Singleton;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

public class PassManager : MonoSingleton<PassManager>
{
    private Button objActiveButton;
    private const int MAX_ITEM_COUNT = 20;
    private int season;
    public bool purchasedPass = false;


    List<KeyValuePair<string, Dictionary<string, object>>> missionMainData, missionTypeData;
    private List<KeyValuePair<string, Dictionary<string, object>>> passRewardData;
    private Dictionary<string, Dictionary<string, object>> entireSeasonRewardData;
    List<KeyValuePair<string, Dictionary<string, object>>> currentSeasonRewardData;

    private List<MissionItem> missionItemList=new List<MissionItem>();
    private List<RewardItem> rewardItemList=new List<RewardItem>();
    private RewardItem lastReward;
    List<Action> rewardItemActions=new List<Action>();

    [SerializeField] private GameObject passObj;
    private Transform passTr;
    [SerializeField]
    private GameObject missionItemObj, rewardItemObj;
    
    private Button closeButton;
    private Button infoButton;
    private Image infoPanel;
    private Text infoText;
    private Button infoCloseButton;
    private Button purchasePassButton;
    
    private Toggle rewardToggle;
    private Toggle missionToggle;
    private GameObject missionPanel;
    private GameObject rewardPanel;
    private GameObject missionScroll;
    private GameObject rewardScroll;

    private GameObject itemInfoPanel;
    private Image itemImage;
    private Text itemText;
    private Button itemCloseBtn;
    

    private GameObject buyBasePanel;
    private Text buyText;
    private Button buyCancelBtn;
    private Button buyConfirmBtn;

    private GameObject getItemsPanel;
    
    private Button purchaseLevelButton;
    private Button receiveAllRewardButton;
    private Image expFillImage;
    private Text levelText;
    private Text expText;
    
    
    void BindObjects()
    {
        //passObj = GameObject.Find("PAGE_PASS");
        objActiveButton = GameObject.Find("OpenButton").GetComponent<Button>();
        passTr = passObj.transform;
        List<Transform> children = passTr.GetComponentsInChildren<Transform>(true).ToList();

        
        
        closeButton = children.Find(x => x.name == "btn_back").GetComponent<Button>();
        infoButton = children.Find(x => x.name =="btn_info").GetComponent<Button>();
        infoPanel = children.Find(x => x.name =="POPUP_PASSINFO").GetComponent<Image>();
        infoText = children.Find(x => x.name == "txt_description").GetComponent<Text>();
        infoCloseButton = children.Find(x => x.name == "btn_close").GetComponent<Button>();
        purchasePassButton = children.Find(x => x.name =="btn_passbuy").GetComponent<Button>();
        
        missionToggle = children.Find(x => x.name =="toggle_mission").GetComponent<Toggle>();
        rewardToggle = children.Find(x => x.name =="toggle_pass").GetComponent<Toggle>();
        missionPanel = children.Find(x => x.name == "PassMissionPanel").gameObject;
        rewardPanel = children.Find(x => x.name == "PassRewardPanel").gameObject;
        missionScroll = children.Find(x => x.name == "MissionScroll").gameObject;
        rewardScroll = children.Find(x => x.name == "RewardScroll").gameObject;
        lastReward = children.Find(x => x.name == "ENDREWARD").GetComponent<RewardItem>();

        itemInfoPanel=children.Find(x => x.name == "POPUP_ITEMINFO").gameObject;
        itemImage = itemInfoPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        itemText = itemInfoPanel.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        itemCloseBtn = itemInfoPanel.transform.GetChild(1).GetChild(2).GetComponent<Button>();
        
        buyBasePanel=children.Find(x => x.name == "POPUP_BUYBASE").gameObject;
        buyText = buyBasePanel.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        buyCancelBtn = buyBasePanel.transform.GetChild(1).GetChild(1).GetComponent<Button>();
        buyConfirmBtn = buyBasePanel.transform.GetChild(1).GetChild(2).GetComponent<Button>();

        getItemsPanel = children.Find(x => x.name == "POPUP_GETITEM").gameObject;

        purchaseLevelButton = children.Find(x => x.name =="btn_lv").GetComponent<Button>();
        receiveAllRewardButton = children.Find(x => x.name =="btn_getall").GetComponent<Button>();
        expFillImage = children.Find(x => x.name =="gauge_fill").GetComponent<Image>();
        levelText = children.Find(x => x.name =="txt_lv").GetComponent<Text>();
        expText = children.Find(x => x.name =="txt_point").GetComponent<Text>();

        timerText = children.Find(x => x.name == "txt_time").GetComponent<Text>();
    }
    
    #region Timer

    [HideInInspector]
    public DateTime startTime, endTime, currentTime;
    private int lastDay;
    private Text timerText;
    public TimeSpan remainTimeSpan;
    private int updateDelay = 120, updateDelayCount;

    #endregion

    void Awake()
    {
        missionItemObj = Resources.Load("C_PassMissionObj") as GameObject;
        rewardItemObj = Resources.Load("C_PassRewardObj") as GameObject;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        BindObjects();
        
        missionMainData = ExcelParser.Read("MISSION_TABLE-MISSIONMAIN").ToList();
        missionTypeData = ExcelParser.Read("MISSION_TABLE-MISSIONTYPE").ToList();
        entireSeasonRewardData = ExcelParser.Read("PASS_TABLE-PASSREWARD");
        passRewardData = entireSeasonRewardData.ToList();

        infoText.text = DataHolder.Instance.GetValueFromTable("STRINGTABLE", "ui_pass_003", "DESCRIPTION");
        
        closeButton.onClick.AddListener(ActivePassObj);
        objActiveButton.onClick.AddListener(ActivePassObj);
        purchasePassButton.onClick.AddListener(PurchasePass);
        infoButton.onClick.AddListener(ActiveInfoObj);
        infoCloseButton.onClick.AddListener(ActiveInfoObj);
        missionToggle.onValueChanged.AddListener(EnableMissionScroll);
        rewardToggle.onValueChanged.AddListener(EnableRewardScroll);

        itemCloseBtn.onClick.AddListener(CloseItemInfoObj);
        
        purchaseLevelButton.onClick.AddListener(ActiveBuyBaseObj);
        buyCancelBtn.onClick.AddListener(ActiveBuyBaseObj);
        buyConfirmBtn.onClick.AddListener(PurchaseLevel);
        
        receiveAllRewardButton.onClick.AddListener(ReceiveAll);

        for (int i = 0; i < MAX_ITEM_COUNT; i++)
        {
            var missionObj = Instantiate(missionItemObj, missionScroll.transform);
            missionItemList.Add(missionObj.GetComponent<MissionItem>());
            
            var rewardObj=Instantiate(rewardItemObj, rewardScroll.transform);
            rewardItemList.Add(rewardObj.GetComponent<RewardItem>());
        }
        
        SetSeason();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    

    #region ScrollView

    void EnableMissionScroll(bool value)
    {
        missionPanel.SetActive(value);
    }
    
    void EnableRewardScroll(bool value)
    {
        rewardPanel.SetActive(value);
    }
    
    void InitMissionScroll()
    {
        int cnt = missionMainData.Count, idx = 0;
        for (int i = 0; i < missionItemList.Count; i++)
        {
            if (idx < cnt)
            {
                missionItemList[i].InitItem();
                missionItemList[i].InitData(missionMainData[idx]);
                idx++;
            }
            else
            {
                missionItemList[i].gameObject.SetActive(false);
            }
        }
    }

    void InitRewardScroll()
    {
        int cnt = passRewardData.Count, idx = 0;
        for (int i = 0; i < passRewardData.Count; i++)
        {
            if (Int32.Parse(passRewardData[idx].Value["PASSMAIN_ID"].ToString()) != season) idx++;
        }
        for (int i = 0; i < rewardItemList.Count; i++)
        {
            
            if (idx < cnt && Int32.Parse(passRewardData[idx].Value["PASSMAIN_ID"].ToString()) == season)
            {
                if (Int32.Parse(passRewardData[idx+1].Value["PASSMAIN_ID"].ToString()) == season)
                {
                    rewardItemList[i].InitItem();
                    rewardItemList[i].InitData(passRewardData[idx]);
                    idx++;
                    rewardItemActions.Add(rewardItemList[i].ReceiveAll);
                    Debug.Log("1");
                }
                else
                {
                    rewardItemList[i].gameObject.SetActive(false);
                    lastReward.InitItem();
                    lastReward.InitData(passRewardData[idx]);
                    idx++;
                    rewardItemActions.Add(lastReward.ReceiveAll);
                    Debug.Log("2");
                }
            }
            else
            {
                rewardItemList[i].gameObject.SetActive(false);
            }
        }
    }
    
    void UpdateRewardScroll()
    {
        for (int i = 0; i < rewardItemList.Count; i++)
        {
            rewardItemList[i].UpdateProgress();
        }
    }
    

    #endregion

    
    #region TimerLogic
    
    void SetSeason()
    {
        bool foundSeason = false;
        var curTime = DateTime.Now;
        var data = ExcelParser.Read("PASS_TABLE-PASSMAIN");
        foreach (var item in data)
        {
            startTime = DateTime.ParseExact(item.Value["STARTDATE"].ToString(), "yyyyMMddHHmmss",
                null);
            endTime = DateTime.ParseExact(item.Value["ENDDATE"].ToString(), "yyyyMMddHHmmss",
                null);
            if (DateTime.Compare(startTime, curTime) < 0 && DateTime.Compare(curTime, endTime) < 0)
            {
                foundSeason = true;
                season = Int32.Parse(item.Value["ID"].ToString());
                SetCurrentSeasonRewardData(season);
                break;
            }
        }

        if (!foundSeason)
        {
            startTime = endTime = DateTime.MinValue;
            Debug.Log("���� ������ �Էµ��� �ʾҽ��ϴ�.");
        }
    }

    void SetCurrentSeasonRewardData(int season)
    {
        currentSeasonRewardData = entireSeasonRewardData
            .Where(pair => Int32.Parse(pair.Value["PASSMAIN_ID"].ToString()) == season).ToList();
        InitMissionScroll();
        InitRewardScroll();
    }

    DateTime GetDateTime(string str)
    {
        int year = Int32.Parse(str.Substring(0, 4));
        int month = Int32.Parse(str.Substring(4, 2));
        int day = Int32.Parse(str.Substring(6, 2));
        int hour = Int32.Parse(str.Substring(8, 2));
        int minute = Int32.Parse(str.Substring(10, 2));
        int second = Int32.Parse(str.Substring(12, 2));
        return new DateTime(year, month, day, hour, minute, second);
    }

    void UpdateTimer()
    {
        updateDelayCount++;
        if (updateDelayCount >= updateDelay)
        {
            remainTimeSpan = endTime.Subtract(DateTime.Now);
            if (remainTimeSpan.Days > 0)
            {
                timerText.text = $"{remainTimeSpan.Days}D ����";
            }
            else if (remainTimeSpan.Hours > 0)
            {
                timerText.text = $"{remainTimeSpan.Hours}H ����";
            }
            else if (remainTimeSpan.Minutes > 0)
            {
                timerText.text = $"{remainTimeSpan.Minutes}M ����";
            }
            else if (remainTimeSpan.Seconds > 0)
            {
                timerText.text = $"{remainTimeSpan.Seconds}S ����";
            }
            else
            {
                Debug.Log("OVER!");
                SetSeason();
            }
            updateDelayCount = 0;
        }
    }

    #endregion
    
    
    void ActivePassObj()
    {
        if (passObj.activeInHierarchy)
        {
            passObj.SetActive(false);
        }
        else
        {
            passObj.SetActive(true);
        }
    }

    void PurchasePass()
    {
        if (!purchasedPass)
        {
            purchasedPass = true;
            purchasePassButton.transform.GetChild(0).GetComponent<Text>().text = "�н� ���� �Ϸ�!";
            UpdateRewardScroll();
        }
    }

    void ActiveInfoObj()
    {
        if (infoPanel.gameObject.activeInHierarchy)
        {
            infoPanel.gameObject.SetActive(false);
        }
        else
        {
            infoPanel.gameObject.SetActive(true);
        }
    }

    public void ActiveItemInfoObj(Sprite sprite, string text)
    {
        if (itemInfoPanel.gameObject.activeInHierarchy)
        {
            itemInfoPanel.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log(text);
            itemImage.sprite = sprite;
            itemText.text = text;
            itemInfoPanel.gameObject.SetActive(true);
        }
    }

    void CloseItemInfoObj()
    {
        itemInfoPanel.gameObject.SetActive(false);
    }

    void ActiveBuyBaseObj()
    {
        if (buyBasePanel.gameObject.activeInHierarchy)
        {
            buyBasePanel.gameObject.SetActive(false);
        }
        else
        {
            buyText.text = $"50다이아를 사용하여 {MagicBox.Instance.playerLevel + 1}을 달성합니다.";
            buyBasePanel.gameObject.SetActive(true);
        }
    }
    
    void PurchaseLevel()
    {
        ActiveBuyBaseObj();
        MagicBox.Instance.LevelUp();
    }

    void ReceiveAll()
    {
        List<Sprite> sprites=new List<Sprite>();
        foreach (var action in rewardItemActions)
        {
            action();
        }
    }

    public void UpdateLevelText(int num)
    {
        levelText.text = $"Lv.{num}";
    }

    public void UpdateEXPText(int cur, int req)
    {
        expText.text = $"{cur},{req}";
    }

}
