using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct D_MISSIONITEM
{
    public int missionTypeID, dateType,rewardID;
    public D_MISSIONITEM(int missionTypeID_, int dateType_, int rewardID_)
    {
        missionTypeID = missionTypeID_;
        dateType = dateType_;
        rewardID = rewardID_;
    }
}

public class D_MISSIONSYSTEM : MonoBehaviour
{
    [SerializeField] GameObject scrollview;
    D_PAGE_PASS pagePass;
    Dictionary<int, D_MISSIONITEM> missionsData = new Dictionary<int, D_MISSIONITEM>();
    List<GameObject> missionList = new List<GameObject>();

    private void Awake()
    {
        pagePass = this.GetComponent<D_PAGE_PASS>();
    }

    public void OpenMissionSystem()
    {
        GameObject prefab = Resources.Load<GameObject>("D_PAGE_PASS_MISSIONITEM");

        var missionMain = ExcelParser.Read("MISSION_TABLE-MISSIONMAIN");
        int count = 0;
        foreach (var i in missionMain)
        {
            int type = int.Parse(i.Value["TYPE"].ToString());
           
            // mission TYPE의 미션만
            if (type == 2)
            {
                count++;
                int dateType = int.Parse(i.Value["DATETYPE"].ToString());
                int missiontypeID = int.Parse(i.Value["MISSIONTYPE_ID"].ToString());
                int reward_ID = int.Parse(i.Value["REWARD_ID"].ToString());
                
                D_MISSIONITEM temp = new D_MISSIONITEM(missiontypeID,dateType,reward_ID);
                missionsData.Add(count,temp);
            }
        }
        // 생성
        for(int i=1; i<=missionsData.Count; i++)
        {
            GameObject instance_ = Instantiate<GameObject>(prefab, scrollview.transform);
            instance_.GetComponent<D_PAGE_PASS_MISSIONITEM>().Init(missionsData[i]);
            missionList.Add(instance_);
        }
        
        scrollview.SetActive(false);
    }

    #region cheatsheet

    private void CheatFunc( int index)
    {
        foreach (var i in missionList)
        {
            D_PAGE_PASS_MISSIONITEM sc = i.GetComponent<D_PAGE_PASS_MISSIONITEM>();
            if (sc.data.missionTypeID == index)
            {
                sc.UpdateMissionCount();
            }
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("로그인");
            CheatFunc(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("게임 플레이");
            CheatFunc(2);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("뽑기");
            CheatFunc(3);

        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("강화공격");
            CheatFunc(4);

        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("강화실패");
            CheatFunc(5);


        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("아이템 구매");
            CheatFunc(6);

        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log("아이템 판매");
            CheatFunc(7);

        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Debug.Log("골드 소모");
            CheatFunc(8);

        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log("다이아 소모");
            CheatFunc(9);

        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("미션완료");
            CheatFunc(10);

        }
    }
    #endregion

    public void ActiveSystem()
    {
        scrollview.SetActive(true);
        foreach (var i in missionList)
        {
            i.SetActive(true);
        }
    }

    public void InactiveSystem()
    {
        scrollview.SetActive(false);
        foreach (var i in missionList)
        {
            i.SetActive(false);
        }
    }


    public TimeSpan GetMinTime()
    {
        TimeSpan remaintimespan = missionList[0].GetComponent<D_PAGE_PASS_MISSIONITEM>().remainTimeSapn;

        foreach (var i in missionList)
        {
            D_PAGE_PASS_MISSIONITEM sc = i.GetComponent<D_PAGE_PASS_MISSIONITEM>();

            if (remaintimespan > sc.remainTimeSapn && sc.data.dateType !=3 )
                remaintimespan = i.GetComponent<D_PAGE_PASS_MISSIONITEM>().remainTimeSapn;
        }
        return remaintimespan;
    }
}
