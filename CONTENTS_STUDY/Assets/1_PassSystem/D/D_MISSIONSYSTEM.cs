using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
