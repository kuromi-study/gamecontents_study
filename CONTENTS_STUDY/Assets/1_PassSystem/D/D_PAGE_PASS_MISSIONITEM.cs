using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class D_PAGE_PASS_MISSIONITEM : MonoBehaviour
{

    [SerializeField] Image itemImg;
    [SerializeField] Text descriptionTXT;
    [SerializeField] Text remainTimeTXT;
    [SerializeField] Text countTXT;
    [SerializeField] GameObject dimmedImg;

    int remainTime;
    int count;
    int defaultCount = 0;

    D_MISSIONITEM data;

    public void Init(D_MISSIONITEM data_)
    {
        data = data_;
        int missionTypeID = data.missionTypeID;

        // 이미지
        int rewardID = data.rewardID;
        itemImg.sprite = Resources.Load<Sprite>(D_PassDataManager.Instance.GetRewardMainData(rewardID).IMAGEPATH);
        // 임무수행도
        defaultCount = D_PassDataManager.Instance.GetMissionData(missionTypeID).COUNT;
        count = defaultCount; 
        countTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_008"),count);
        // 임무 설명
        descriptionTXT.text = D_StringkeyManager.Instance.GetString(D_PassDataManager.Instance.GetMissionData(missionTypeID).STRINGKEY);

        var itemList = D_PassDataManager.Instance.GetItemList();


        // 획득한 아이템인 경우 딤드 처리하기
        for(int i =0; i< itemList.Count; i++)
        {
            if (itemList[i].ID == rewardID)
                dimmedImg.SetActive(true);
        }

        remainTimeTXT.text = GetRemainTime();
    }

    private void OnEnable()
    {
        StartCoroutine(SetRemainTime());
    }


    IEnumerator SetRemainTime()
    {
        // 1초마다 검사
        while (true)
        {
            yield return new WaitForSeconds(1f);

            remainTimeTXT.text = GetRemainTime();
        }
    }



    public string GetRemainTime()
    {
        DateTime nowTime = DateTime.Now;

        DateTime mondayDate = nowTime.AddDays(Convert.ToInt32(DayOfWeek.Monday) - Convert.ToInt32(nowTime.DayOfWeek) + 7);
        DateTime monthfirst = new DateTime(nowTime.Year, nowTime.Month + 1, 1);
        
        // 매일 초기화
        DateTime dayLimit = new DateTime(nowTime.AddDays(1).Year, nowTime.AddDays(1).Month, nowTime.AddDays(1).Day,0,0,0);

        // 월요일 초기화
        DateTime weekLimit = new DateTime(mondayDate.Year, mondayDate.Month, mondayDate.Day,0,0,0);

        // 매달 초기화
        DateTime monthLimit = new DateTime(monthfirst.Year, monthfirst.Month, monthfirst.Day, 0,0,0);

        TimeSpan temp;

        string str = D_StringkeyManager.Instance.GetString("ui_pass_002");

        switch (data.dateType)
        {
            case 0: { temp = dayLimit - nowTime; } break;
            case 1: { temp = weekLimit - nowTime; } break;
            case 2: { temp = monthLimit - nowTime; } break;
            case 3: { return ""; }
        }

        if ((int)temp.TotalDays > 0f)
            return string.Format(str,(int)temp.TotalDays,"일");

        if ((int)temp.TotalHours > 0f)
            return string.Format(str,(int)temp.TotalHours, "시간");

        if ((int)temp.TotalMinutes > 0f)
            return string.Format(str,(int)temp.TotalMinutes, "분");

        if ((int)temp.TotalSeconds > 0f)
            return string.Format(str,(int)temp.TotalSeconds, "초");


        // 갱신
        if ((int)temp.TotalSeconds <= 0)
            count = defaultCount;

        return str;
    }


    public void UpdaateMissionCount()
    {
        Debug.Log("임무수행완료");
        if(count>0)
            count--;

        countTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_008"), count);
    }

    public void DownGetRewardBtn()
    {
        Debug.Log("보상 받기 버튼");

        if (count > 0) return;

        // 획득한 아이템 리스트에 추가
        D_PassDataManager.Instance.AddList(D_PassDataManager.Instance.GetRewardMainData(data.rewardID));

        // 팝업 생성
        GameObject prefab = Resources.Load<GameObject>("D_POPUP_GETITEM");
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_GETITEM>().UpdateList();

        // 이미지 딤드 처리
        dimmedImg.SetActive(true);

    }
}
