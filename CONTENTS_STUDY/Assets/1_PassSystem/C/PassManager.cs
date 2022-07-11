using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    private bool purchasedPass = false;


    List<KeyValuePair<string, Dictionary<string, object>>> missionMainData, missionTypeData;
    private Dictionary<string, Dictionary<string, object>> entireSeasonRewardData;
    List<KeyValuePair<string, Dictionary<string, object>>> currentSeasonRewardData;

    private List<MissionItem> missionItemList=new List<MissionItem>();
    private List<RewardItem> rewardItemList=new List<RewardItem>();
    
    private GameObject passObj;
    private Transform passTr;
    [SerializeField]
    private GameObject missionItemObj, rewardItemObj;
    
    private Button closeButton;
    private Button infoButton;
    private Image infoPanel;
    private Button purchasePassButton;
    
    private Toggle rewardToggle;
    private Toggle missionToggle;
    private GameObject missionPanel;
    private GameObject rewardPanel;
    private GameObject missionScroll;
    private GameObject rewardScroll;
    
    
    private Button purchaseLevelButton;
    private Button receiveAllRewardButton;
    private Image expFillImage;
    private Text levelText;
    private Text expText;
    
    
    void BindObjects()
    {
        passObj = GameObject.Find("PAGE_PASS");
        objActiveButton = GameObject.Find("OpenButton").GetComponent<Button>();
        passTr = passObj.transform;
        List<Transform> children = passTr.GetComponentsInChildren<Transform>(true).ToList();

        
        
        closeButton = children.Find(x => x.name == "btn_back").GetComponent<Button>();
        infoButton = children.Find(x => x.name =="btn_info").GetComponent<Button>();
        infoPanel = children.Find(x => x.name =="POPUP_PASSINFO").GetComponent<Image>();
        purchasePassButton = children.Find(x => x.name =="btn_passbuy").GetComponent<Button>();
        
        missionToggle = children.Find(x => x.name =="toggle_mission").GetComponent<Toggle>();
        rewardToggle = children.Find(x => x.name =="toggle_pass").GetComponent<Toggle>();
        missionPanel = children.Find(x => x.name == "PassMissionPanel").gameObject;
        rewardPanel = children.Find(x => x.name == "PassRewardPanel").gameObject;
        missionScroll = children.Find(x => x.name == "MissionScroll").gameObject;
        rewardScroll = children.Find(x => x.name == "RewardScroll").gameObject;

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
        
        
        objActiveButton.onClick.AddListener(ActivePassObj);
        purchasePassButton.onClick.AddListener(PurchasePass);
        missionToggle.onValueChanged.AddListener(EnableMissionScroll);
        rewardToggle.onValueChanged.AddListener(EnableRewardScroll);

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
    
    void UpdateRewardScroll()
    {
        
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
            Debug.Log("시즌 정보가 입력되지 않았습니다.");
        }
    }

    void SetCurrentSeasonRewardData(int season)
    {
        currentSeasonRewardData = entireSeasonRewardData
            .Where(pair => Int32.Parse(pair.Value["PASSMAIN_ID"].ToString()) == season).ToList();
        InitMissionScroll();
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
                timerText.text = $"{remainTimeSpan.Days}D 남음";
            }
            else if (remainTimeSpan.Hours > 0)
            {
                timerText.text = $"{remainTimeSpan.Hours}H 남음";
            }
            else if (remainTimeSpan.Minutes > 0)
            {
                timerText.text = $"{remainTimeSpan.Minutes}M 남음";
            }
            else if (remainTimeSpan.Seconds > 0)
            {
                timerText.text = $"{remainTimeSpan.Seconds}S 남음";
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
            purchasePassButton.transform.GetChild(0).GetComponent<Text>().text = "패스 구매 완료!";
        }
    }

}
