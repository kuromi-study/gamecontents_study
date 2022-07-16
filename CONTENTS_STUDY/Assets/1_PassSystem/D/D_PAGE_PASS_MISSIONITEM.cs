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

        // �̹���
        int rewardID = data.rewardID;
        itemImg.sprite = Resources.Load<Sprite>(D_PassDataManager.Instance.GetRewardMainData(rewardID).IMAGEPATH);
        // �ӹ����൵
        defaultCount = D_PassDataManager.Instance.GetMissionData(missionTypeID).COUNT;
        count = defaultCount; 
        countTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_008"),count);
        // �ӹ� ����
        descriptionTXT.text = D_StringkeyManager.Instance.GetString(D_PassDataManager.Instance.GetMissionData(missionTypeID).STRINGKEY);

        var itemList = D_PassDataManager.Instance.GetItemList();


        // ȹ���� �������� ��� ���� ó���ϱ�
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
        // 1�ʸ��� �˻�
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
        
        // ���� �ʱ�ȭ
        DateTime dayLimit = new DateTime(nowTime.AddDays(1).Year, nowTime.AddDays(1).Month, nowTime.AddDays(1).Day,0,0,0);

        // ������ �ʱ�ȭ
        DateTime weekLimit = new DateTime(mondayDate.Year, mondayDate.Month, mondayDate.Day,0,0,0);

        // �Ŵ� �ʱ�ȭ
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
            return string.Format(str,(int)temp.TotalDays,"��");

        if ((int)temp.TotalHours > 0f)
            return string.Format(str,(int)temp.TotalHours, "�ð�");

        if ((int)temp.TotalMinutes > 0f)
            return string.Format(str,(int)temp.TotalMinutes, "��");

        if ((int)temp.TotalSeconds > 0f)
            return string.Format(str,(int)temp.TotalSeconds, "��");


        // ����
        if ((int)temp.TotalSeconds <= 0)
            count = defaultCount;

        return str;
    }


    public void UpdaateMissionCount()
    {
        Debug.Log("�ӹ�����Ϸ�");
        if(count>0)
            count--;

        countTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_008"), count);
    }

    public void DownGetRewardBtn()
    {
        Debug.Log("���� �ޱ� ��ư");

        if (count > 0) return;

        // ȹ���� ������ ����Ʈ�� �߰�
        D_PassDataManager.Instance.AddList(D_PassDataManager.Instance.GetRewardMainData(data.rewardID));

        // �˾� ����
        GameObject prefab = Resources.Load<GameObject>("D_POPUP_GETITEM");
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_GETITEM>().UpdateList();

        // �̹��� ���� ó��
        dimmedImg.SetActive(true);

    }
}
