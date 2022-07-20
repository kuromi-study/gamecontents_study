using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class D_PAGE_PASS : MonoBehaviour
{

    [Header("TOP")]
    [SerializeField] Text remainTimeTXT;
    [SerializeField] Button backBtn;

    [Header("MIDDLE")]
    [SerializeField] Button buyBtn;

    [Header("BOTTOM")]
    [SerializeField] Text levelTXT;
    [SerializeField] GameObject levelBtn;
    [SerializeField] Text pointTXT;
    [SerializeField] Image pointImg;
    [SerializeField] Button getAllBtn;

    [Header("ScrollView")]
    [SerializeField] GameObject passScrollView;
    [SerializeField] GameObject missionScrollView;
    [SerializeField] ScrollRect scrollview;

    string passDescription = "";
    DateTime endTime;
 
    D_PASSSYSTEM passSystem;
    D_MISSIONSYSTEM missionSystem;

    bool bIsPassPossible = true;

    public Dictionary<int, D_PASSLEVEL> passLevelData = new Dictionary<int, D_PASSLEVEL>();
    public Dictionary<int, D_REWARDMAIN> rewardmainData = new Dictionary<int, D_REWARDMAIN>(); // id


    private void OnEnable()
    {
        //  패스 시스템 오픈
        OpenPagePass();
        // 코루틴
        StartCoroutine(SetRemainTime());
    }

    private void OpenPagePass()
    {
        SetPassID();
        
        if (!bIsPassPossible) return;

        passSystem = this.GetComponent<D_PASSSYSTEM>();
        missionSystem = this.GetComponent<D_MISSIONSYSTEM>();
        

        // 버튼 텍스트 설정
        backBtn.GetComponentInChildren<Text>().text = D_StringkeyManager.Instance.GetString("ui_pass_001");
        buyBtn.transform.GetChild(0).GetComponent<Text>().text = D_StringkeyManager.Instance.GetString("ui_pass_003");
        levelBtn.GetComponentInChildren<Text>().text = D_StringkeyManager.Instance.GetString("ui_pass_004");
        getAllBtn.GetComponentInChildren<Text>().text = D_StringkeyManager.Instance.GetString("ui_pass_007"); 

        // 패스탭, 미션탭 생성
        passSystem.OpenPassSystem();
        missionSystem.OpenMissionSystem();

        UpdatePoint();
        UpdateLevel();
    }

    private void UpdatePoint()
    {
        // 포인트 설정
        pointTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_006"),
        D_PassDataManager.Instance.curPoint, D_PassDataManager.Instance.maxPoint);
        pointImg.fillAmount = (float)D_PassDataManager.Instance.curPoint / (float)D_PassDataManager.Instance.maxPoint;
    }

    private void UpdateLevel()
    {
        levelTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_005"), D_PassDataManager.Instance.curLevel);
    }

    /*
    private void UpdateLevelUp()
    {
        D_PassDataManager.Instance.curLevel++;
        // 레벨 설정
        levelTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_005"), D_PassDataManager.Instance.curLevel);

    }
    */
  
    // PASS ID가 0개 일 경우 아무런 반응X
    // 1개 일 경우 패스 시스템을 연다
    // 2개 일 경우 에러 발생
    public void SetPassID()
    {
        DateTime nowTime = DateTime.Now;

        int count = 0;

        var passMain =  ExcelParser.Read("PASS_TABLE-PASSMAIN");

        int id = 0;
        string description = null;
        var start = "";
        var end = "";

        foreach (var value in passMain)
        {
            id = int.Parse(value.Value["ID"].ToString());
            description = value.Value["description"].ToString();
            start = value.Value["STARTDATE"].ToString();
            end = value.Value["ENDDATE"].ToString();

            if (nowTime >= GetDateTime(start) && nowTime <= GetDateTime(end))
            {
                endTime = GetDateTime(end);
                // 패스 아이디 설정
                D_PassDataManager.Instance.passID = id;
                Debug.Log("id : " + id);
                // 패스 설명 설정
                passDescription = description;
                // 남은 시간 설정
                remainTimeTXT.text = GetRemainTime();
                count++;
            }
            else // 0. 예외처리 해주기
            {
                Debug.Log("현재 진행 중인 패스가 없으므로, 예외처리 해주기");
                bIsPassPossible = false;
            }
        }

        // 1. 예외처리해주기
        if (count > 1)
        {
            Debug.Log("해당되는 패스가 두개 이므로 예외처리 해주기");
            bIsPassPossible = false;
        }
        else { bIsPassPossible = true; }
    }


    public void DownBackBtn()
    {
        Debug.Log("뒤로가기");
        this.gameObject.SetActive(false);
    }


    public void DownGetAllBtn()
    {
        Debug.Log("모든 보상 받기");
        if (D_PassDataManager.Instance.curLevel == D_PassDataManager.Instance.CheckedLevel)
            return;

        passSystem.GetAllReward(D_PassDataManager.Instance.curLevel);
    }


    public void DownPassInfo()
    {
        Debug.Log("패스 정보 보기");

        GameObject prefab = Resources.Load<GameObject>("D_POPUP_PASSINFO");
        GameObject popup =  Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_PASSINFO>().Init(passDescription);
    }

    // 패스 구매하기 -> 패스 리워드 살 수 있음
    public void DownBuyPassBtn()
    {
        Debug.Log("패스 구매하기");

        GameObject prefab = Resources.Load<GameObject>("D_POPUP_BUYBASE");
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_BUYBASE>().Init(D_POPUP_BUYBASE.POPUPType.buyPass);
    }

    public void DownBuyLevelBtn()
    {
        Debug.Log(" 레벨 구매하기");

        GameObject prefab = Resources.Load<GameObject>("D_POPUP_BUYBASE");
        prefab.GetComponent<D_POPUP_BUYBASE>().Init( D_POPUP_BUYBASE.POPUPType.buyLevel);
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);

        // 레벨 업데이트 하기
        passSystem.UpdateLevelUp();
        UpdateLevel();
        UpdatePoint();
    }

    #region UpdateRemainTime

    IEnumerator SetRemainTime()
    {
        // 1초마다 검사
        while (true)
        {
            yield return new WaitForSeconds(1f);

            remainTimeTXT.text = GetRemainTime();
        }
    }

    string GetRemainTime()
    {
        DateTime nowTime = DateTime.Now;
        string str = D_StringkeyManager.Instance.GetString("ui_pass_002");

        TimeSpan remaintime = endTime - nowTime;

        if ((int)remaintime.TotalDays > 0f)
            return string.Format(str, (int)remaintime.TotalDays,"일");

        if ((int)remaintime.TotalHours > 0f)
            return string.Format(str, (int)remaintime.TotalHours, "시간");

        if ((int)remaintime.TotalMinutes > 0f)
            return string.Format(str, (int)remaintime.TotalMinutes, "분");

        if ((int)remaintime.TotalSeconds > 0f)
            return string.Format(str, (int)remaintime.TotalSeconds, "초");

        if ((int)remaintime.TotalSeconds <= 0f)
        {
            StartCoroutine(UpdatePass());
            Debug.Log("패스 갱신 시작");
        }

        return string.Format(str,"","");
    }

    IEnumerator UpdatePass()
    {
        // 랜덤 다시 설정
        yield return new WaitForSeconds(1f);
        
        // 날짜에 알맞는 패스 아이디 다시 갱신
        SetPassID();
        // 현재 레벨, 획득한 레벨, 보상 획득 레벨 랜덤으로 주기
        D_PassDataManager.Instance.SetRandomValue();
        // 아이템 리스트 리셋
        D_PassDataManager.Instance.ClearItemList();
        // 패스아이디에 맞는 패스 아이템 다시 생성
        passSystem.ResetPassSystem();
        // 레벨
        UpdateLevel();
        // 포인트
        UpdatePoint();
    }


    DateTime GetDateTime(string date)
    {
        int year = int.Parse(date.Substring(0, 4));
        int month = int.Parse(date.Substring(4, 2));
        int day = int.Parse(date.Substring(6, 2));
        int hour = int.Parse(date.Substring(8, 2));
        int min = int.Parse(date.Substring(10, 2));
        int sec = int.Parse(date.Substring(12, 2));
       
        DateTime temp = new DateTime(year,month,day,hour,min,sec);
        
        return temp;
    }
    #endregion

    #region ActiveTab
    public void DownPassTab()
    {
        if (passScrollView.isStatic) return;

        scrollview.content = passScrollView.GetComponent<RectTransform>();
        scrollview.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(950f,480f);
        passSystem.ActivePassSystem();
        missionSystem.InactiveSystem();
    }

    public void DownMissionTab()
    {
        if (missionScrollView.isStatic) return;

        scrollview.content = missionScrollView.GetComponent<RectTransform>();
        scrollview.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(950f, 600f);
        missionSystem.ActiveSystem();
        passSystem.InactivePassSystem();
    }
    #endregion
    
}

public class D_REWARDMAIN
{
    public int ID, NUM;
    public string IMAGEPATH,STRINGKEY;
    public D_REWARDMAIN(int ID_,int NUM_, string IMAGEPATH_,string STRINGKEY_)
    {
        ID = ID_;
        NUM = NUM_;
        IMAGEPATH = IMAGEPATH_;
        STRINGKEY = STRINGKEY_;
    }
}


public class D_PASSITEM
{
    public int ID;
    public string DESCRIPTION;
    public long STARTDATE;
    public long ENDDATE;

    public D_PASSITEM(int ID_, string DESCRIPTION_, long STARTDATE_, long ENDDATE_)
    {
        ID = ID_;
        DESCRIPTION = DESCRIPTION_;
        STARTDATE = STARTDATE_;
        ENDDATE = ENDDATE_;
    }
}


public class D_PASSLEVEL
{
    public int LEVEL, NEEDPOINT;

    public D_PASSLEVEL(int LEVEL_, int NEEDPOINT_)
    {
        NEEDPOINT = NEEDPOINT_;
        LEVEL = LEVEL_;
    }
}


    public class D_MISSIONTYPE
    {
        public int ID, COUNT;
        public string STRINGKEY, DESCRIPTION;
        public D_MISSIONTYPE(int ID_, int COUNT_, string STRINGKEY_, string DESCRIPTION_)
        {
            ID = ID_;
            COUNT = COUNT_;
            STRINGKEY = STRINGKEY_;
            DESCRIPTION = DESCRIPTION_;
        }
    }

public class D_PASSREWARD
{
    public int passID, passLevel_ID, passLevel, normal_reward_ID, pass_reward_ID1, pass_reward_ID2;

    public D_PASSREWARD(int passID_, int passLevel_ID_, int passLevel_, int normal_reward_ID_, int pass_reward_ID1_, int pass_reward_ID2_)
    {
        passID = passID_;
        passLevel_ID = passLevel_ID_;
        passLevel = passLevel_;
        normal_reward_ID = normal_reward_ID_;
        pass_reward_ID1 = pass_reward_ID1_;
        pass_reward_ID2 = pass_reward_ID2_;
    }
}