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
        //  �н� �ý��� ����
        OpenPagePass();
        missionSystem.InactiveSystem();
    }

    private void OnEnable()
    {
        // �ڷ�ƾ
        StartCoroutine(SetRemainTime());
    }


    private void OpenPagePass()
    {
        SetPassID();
        
        if (!bIsPassPossible) return;
        
        passSystem = this.GetComponent<D_PASSSYSTEM>();
        missionSystem = this.GetComponent<D_MISSIONSYSTEM>();
        

        // ��ư �ؽ�Ʈ ����
        backBtn.GetComponentInChildren<Text>().text = D_StringkeyManager.Instance.GetString("ui_pass_001");
        buyBtn.transform.GetChild(0).GetComponent<Text>().text = D_StringkeyManager.Instance.GetString("ui_pass_003");
        levelBtn.GetComponentInChildren<Text>().text = D_StringkeyManager.Instance.GetString("ui_pass_004");
        getAllBtn.GetComponentInChildren<Text>().text = D_StringkeyManager.Instance.GetString("ui_pass_007"); 

        // �н���, �̼��� ����
        passSystem.OpenPassSystem();
        missionSystem.OpenMissionSystem();

        UpdatePoint();
        UpdateLevel();

      //  StartCoroutine(AutoScroll());
    }

    #region Update
    private void UpdatePoint()
    {
        // ����Ʈ ����
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
                // �н� ���̵� ����
                D_PassDataManager.Instance.passID = id;
                Debug.Log("id : " + id);
                // �н� ���� ����
                passDescription = description;
                // ���� �ð� ����
                remainTimeTXT.text = GetRemainTime();
                count++;
            }
            else // 0. ����ó�� ���ֱ�
            {
                bIsPassPossible = false;
            }

            
        }

        // 1. ����ó�����ֱ�
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
        Debug.Log("�ڷΰ���");
        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }


    public void DownGetAllBtn()
    {
        Debug.Log("��� ���� �ޱ�");
        if (D_PassDataManager.Instance.curLevel == D_PassDataManager.Instance.CheckedLevel)
            return;

        passSystem.GetAllReward();
    }


    public void DownPassInfo()
    {
        Debug.Log("�н� ���� ����");

        GameObject prefab = Resources.Load<GameObject>("D_POPUP_PASSINFO");
        GameObject popup =  Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_PASSINFO>().Init(passDescription);
    }

    // �н� �����ϱ� -> �н� ������ �� �� ����
    public void DownBuyPassBtn()
    {
        // �н� ���� �������� Ȯ���ϱ�
        if (!passSystem.DownBuyPassBtn()) return;
        
        // �����ϸ� �˾�â ����
        Debug.Log("�н� �����ϱ�");
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
        Debug.Log(" ���� �����ϱ�");

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
        // 1�ʸ��� �˻�
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
            return string.Format(str, (int)timesapn.TotalDays, "��");

        if ((int)timesapn.TotalHours > 0f)
            return string.Format(str, (int)timesapn.TotalHours, "�ð�");

        if ((int)timesapn.TotalMinutes > 0f)
            return string.Format(str, (int)timesapn.TotalMinutes, "��");

        if ((int)timesapn.TotalSeconds > 0f)
            return string.Format(str, (int)timesapn.TotalSeconds, "��");

        if ((int)timesapn.TotalSeconds <= 0f)
        {
            StartCoroutine(UpdatePass());
            Debug.Log("�н� ���� ����");
        }

        return string.Format(str, "", "");
    }

    IEnumerator UpdatePass()
    {
        // ���� �ٽ� ����
        yield return new WaitForSeconds(0.2f);
        
        // ��¥�� �˸´� �н� ���̵� �ٽ� ����
        SetPassID();
        // ���� ����, ȹ���� ����, ���� ȹ�� ���� �������� �ֱ�
        D_PassDataManager.Instance.SetRandomValue();
        // ������ ����Ʈ ����
        //D_PassDataManager.Instance.ClearItemList();
        D_PassDataManager.Instance.ResetPass();
        // �н����̵� �´� �н� ������ �ٽ� ����
        passSystem.ResetPassSystem();
        // ����
        UpdateLevel();
        // ����Ʈ
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

        Debug.Log("�н���Ȱ��ȭ");
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

