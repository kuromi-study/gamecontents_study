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

    public Dictionary<int, D_PASSLEVEL> passLevelData = new Dictionary<int, D_PASSLEVEL>();
    public Dictionary<int, D_REWARDMAIN> rewardmainData = new Dictionary<int, D_REWARDMAIN>(); // id
    

    private void Awake()
    {
        OPEN();
    }

    private void OPEN()
    {
        passSystem = this.GetComponent<D_PASSSYSTEM>();
        missionSystem = this.GetComponent<D_MISSIONSYSTEM>();
        SetText();
        SetPassID();

        // ����
        passSystem.OpenPassSystem();
        missionSystem.OpenMissionSystem();

        // ���� ����
        levelTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_005"), D_PassDataManager.Instance.curLevel);
        // ����Ʈ ����
        pointTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_006"),
        D_PassDataManager.Instance.curPoint, D_PassDataManager.Instance.maxPoint);
        pointImg.fillAmount = (float)D_PassDataManager.Instance.curPoint / (float)D_PassDataManager.Instance.maxPoint;
    }
  


    public void SetText()
    {
        // ��ư �ؽ�Ʈ ����
        backBtn.GetComponentInChildren<Text>().text = D_StringkeyManager.Instance.GetString("ui_pass_001");
        buyBtn.transform.GetChild(0).GetComponent<Text>().text = D_StringkeyManager.Instance.GetString("ui_pass_003");
        levelBtn.GetComponentInChildren<Text>().text = D_StringkeyManager.Instance.GetString("ui_pass_004");
        getAllBtn.GetComponentInChildren<Text>().text = D_StringkeyManager.Instance.GetString("ui_pass_007");
    }

  
    // PASS ID�� 0�� �� ��� �ƹ��� ����X
    // 1�� �� ��� �н� �ý����� ����
    // 2�� �� ��� ���� �߻�
    public void SetPassID()
    {
        DateTime nowTime = DateTime.Now;
        var passMain =  ExcelParser.Read("PASS_TABLE-PASSMAIN");

        foreach (var value in passMain)
        {
            int id = int.Parse(value.Value["ID"].ToString());
            string description = value.Value["description"].ToString();
            var start = value.Value["STARTDATE"].ToString();
            var end = value.Value["ENDDATE"].ToString();
            endTime = GetDateTime(end);

            if (nowTime>=GetDateTime(start)&& nowTime < GetDateTime(end))
            {
                // �н� ���̵� ����
               D_PassDataManager.Instance.passID = id;
                // �н� ���� ����
                passDescription = description;
                // ���� �ð� ����
                remainTimeTXT.text = GetRemainTime(GetDateTime(end));
            }
        }
    }


    public void DownBackBtn()
    {
        this.gameObject.SetActive(false);
    }
    public void DownGetAllBtn()
    {
        Debug.Log("��� ���� �ޱ�");
        passSystem.GetAllReward();
    }


    public void DownPassInfo()
    {
        Debug.Log("�н� ���� ����");
        GameObject prefab = Resources.Load<GameObject>("D_POPUP_PASSINFO");
        GameObject popup =  Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_PASSINFO>().Init(passDescription);
    }

    public void DownBuyPassBtn()
    {
        Debug.Log("�н� �����ϱ�");
        GameObject prefab = Resources.Load<GameObject>("D_POPUP_BUYBASE");
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_BUYBASE>().Init(D_POPUP_BUYBASE.POPUPType.buyPass);
    }

    public void DownBuyLevelBtn()
    {
        Debug.Log(" ���� �����ϱ�");
        GameObject prefab = Resources.Load<GameObject>("D_POPUP_BUYBASE");
        prefab.GetComponent<D_POPUP_BUYBASE>().Init( D_POPUP_BUYBASE.POPUPType.buyLevel);
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);

        // ���� ����
        // ������Ʈ
    }

    #region UpdateRemainTime

    private void OnEnable()
    {
        StartCoroutine(SetRemainTime());
    }

    IEnumerator SetRemainTime()
    {
        // 1�ʸ��� �˻�
        while (true)
        {
            yield return new WaitForSeconds(1f);

            remainTimeTXT.text = GetRemainTime(endTime);
        }
    }

    string GetRemainTime(DateTime endTime)
    {
        DateTime nowTime = DateTime.Now;
        string str = D_StringkeyManager.Instance.GetString("ui_pass_002");

        TimeSpan remaintime = endTime - nowTime;

        if ((int)remaintime.TotalDays > 0f)
            return string.Format(str, (int)remaintime.TotalDays,"��");

        if ((int)remaintime.TotalHours > 0f)
            return string.Format(str, (int)remaintime.TotalHours, "�ð�");

        if ((int)remaintime.TotalMinutes > 0f)
            return string.Format(str, (int)remaintime.TotalMinutes, "��");

        if ((int)remaintime.TotalSeconds > 0f)
            return string.Format(str, (int)remaintime.TotalSeconds, "��");
        return string.Format(str,"","");
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
        this.passID = passID_;
        this.passLevel_ID = passLevel_ID_;
        this.passLevel = passLevel_;
        this.normal_reward_ID = normal_reward_ID_;
        this.pass_reward_ID1 = pass_reward_ID1_;
        this.pass_reward_ID2 = pass_reward_ID2_;
    }
}