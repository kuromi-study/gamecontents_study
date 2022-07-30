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

    bool bIsPassPossible = true; // ??????

    public Dictionary<int, D_PASSLEVEL> passLevelData = new Dictionary<int, D_PASSLEVEL>();
    public Dictionary<int, D_REWARDMAIN> rewardmainData = new Dictionary<int, D_REWARDMAIN>();

    private void Awake()
    {
        //  패스 시스템 오픈
        OpenPagePass();
        missionSystem.InactiveSystem();
    }

    private void OnEnable()
    {
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

      //  StartCoroutine(AutoScroll());
    }

    #region Update
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
    #endregion


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
                bIsPassPossible = false;
            }

            
        }

        // 1. 예외처리해주기
        if (count > 1)
        {
            bIsPassPossible = false;
        }
        else { bIsPassPossible = true; }


    }
   
    float speed = 0.2f;
    IEnumerator AutoScroll()
    {
        int curlevel = D_PassDataManager.Instance.curLevel;

        yield return new WaitForSeconds(0.1f);
        float targetPos = 1f - ((float)(curlevel) / (passScrollView.transform.childCount));
       
        Vector2 targetPosV2 = new Vector2(0,targetPos);
        targetPosV2 = new Vector2(0, targetPos);
        /*
        while (scrollview.normalizedPosition.y >= targetPos)
        {
            yield return new WaitForSeconds(0.1f);
            
            scrollview.normalizedPosition -= new Vector2(0, speed);
            var defaulttransfrom = scrollview.normalizedPosition;
        }*/
    }

    #region DownButton
    public void DownBackBtn()
    {
        Debug.Log("뒤로가기");
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }


    public void DownGetAllBtn()
    {
        Debug.Log("모든 보상 받기");
        if (D_PassDataManager.Instance.curLevel == D_PassDataManager.Instance.CheckedLevel)
            return;

        passSystem.GetAllReward();
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
        // 패스 구매 가능한지 확인하기
        if (!passSystem.DownBuyPassBtn()) return;
        
        // 가능하면 팝업창 생성
        Debug.Log("패스 구매하기");
        GameObject prefab = Resources.Load<GameObject>("D_POPUP_BUYBASE");
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_BUYBASE>().Init(D_POPUP_BUYBASE.POPUPType.buyPass, this.GetComponent<D_PAGE_PASS>());
    }

    public void BuyPass()
    {
        passSystem.BuyPass();
    }


    public void DownBuyLevelBtn()
    {
        Debug.Log(" 레벨 구매하기");

        GameObject prefab = Resources.Load<GameObject>("D_POPUP_BUYBASE");
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_BUYBASE>().Init( D_POPUP_BUYBASE.POPUPType.buyLevel, this.GetComponent<D_PAGE_PASS>());
    }

    public void BuyLevel()
    {
        passSystem.UpdateLevelUp();
        UpdateLevel();
        UpdatePoint();

       // StartCoroutine(AutoScroll());



    }

    #endregion

    #region UpdateRemainTime

    IEnumerator SetRemainTime()
    {
        // 1초마다 검사
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (MissionTab_)
            {
               if (missionSystem.GetMinTime().TotalSeconds < remaintimespan.TotalSeconds)
                    remainTimeTXT.text = GetTimeStr(missionSystem.GetMinTime());
                else
                    remainTimeTXT.text = GetRemainTime();
            }
            else
                remainTimeTXT.text = GetRemainTime();

        }
    }
    TimeSpan remaintimespan;

    string GetRemainTime()
    {
        DateTime nowTime = DateTime.Now;
        string str = D_StringkeyManager.Instance.GetString("ui_pass_002");

        TimeSpan remaintime = remaintimespan = endTime - nowTime;

        return GetTimeStr(remaintime);
    }

    private string GetTimeStr(TimeSpan timesapn)
    {
        string str = D_StringkeyManager.Instance.GetString("ui_pass_002");

        if ((int)timesapn.TotalDays > 0f)
            return string.Format(str, (int)timesapn.TotalDays, "일");

        if ((int)timesapn.TotalHours > 0f)
            return string.Format(str, (int)timesapn.TotalHours, "시간");

        if ((int)timesapn.TotalMinutes > 0f)
            return string.Format(str, (int)timesapn.TotalMinutes, "분");

        if ((int)timesapn.TotalSeconds > 0f)
            return string.Format(str, (int)timesapn.TotalSeconds, "초");

        if ((int)timesapn.TotalSeconds <= 0f)
        {
            StartCoroutine(UpdatePass());
            Debug.Log("패스 갱신 시작");
        }

        return string.Format(str, "", "");
    }

    IEnumerator UpdatePass()
    {
        // 랜덤 다시 설정
        yield return new WaitForSeconds(0.2f);
        
        // 날짜에 알맞는 패스 아이디 다시 갱신
        SetPassID();
        // 현재 레벨, 획득한 레벨, 보상 획득 레벨 랜덤으로 주기
        D_PassDataManager.Instance.SetRandomValue();
        // 아이템 리스트 리셋
        //D_PassDataManager.Instance.ClearItemList();
        D_PassDataManager.Instance.ResetPass();
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
        if (!MissionTab_) return;

        Debug.Log("패스탭활성화");
        MissionTab_ = false;
        scrollview.content = passScrollView.GetComponent<RectTransform>();
        scrollview.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(950f,480f);
        passSystem.ActivePassSystem();
        missionSystem.InactiveSystem();

         // StartCoroutine(AutoScroll());
    }
    bool MissionTab_ = false;
    public void DownMissionTab()
    {
        if (MissionTab_) return;
        MissionTab_ = true;
        scrollview.content = missionScrollView.GetComponent<RectTransform>();
        scrollview.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(950f, 600f);
        missionSystem.ActiveSystem();
        passSystem.InactivePassSystem();
    }
    #endregion
    
}

