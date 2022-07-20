using System.Collections;
using System.Collections.Generic;
using Utility.Singleton;
using UnityEngine;

public class D_PassDataManager : MonoSingleton<D_PassDataManager>
{

    Dictionary<int, D_PASSLEVEL> passLevelData = new Dictionary<int, D_PASSLEVEL>();
    Dictionary<int, D_REWARDMAIN> rewardmainData = new Dictionary<int, D_REWARDMAIN>(); // id
    Dictionary<int, D_MISSIONTYPE> missionTypeData = new Dictionary<int, D_MISSIONTYPE>(); // id
    public int tempCnt = 0;
    // ==== �ӽ� ���� //
    public int curLevel;//{ get; private set; }
    public int curPoint = 0;
    public int maxPoint = 0;
    public int passID = 0;
    public int CheckedLevel = 0;
    public int CheckedRewardLevel = 0;

    // ȹ���� ������ ����Ʈ
    // ======

    Dictionary<int, D_REWARDMAIN> checkedItemList = new Dictionary<int, D_REWARDMAIN>();
    int index = 0;
    public Dictionary<int, D_REWARDMAIN> GetItemList() { return checkedItemList; }


    private void Awake()
    {
        SetRandomValue();

        ReadPassLevel();
        ReadRewardmain();
        ReadMissionData();
    }

    #region ItemList
    public void AddList(D_REWARDMAIN item)
    {
        checkedItemList.Add(index,item);
        index++;
        Debug.Log("������ ���� :" + index);
    }

    // �н� ���� �� ������ ����Ʈ �����
    public void ClearItemList()
    {
        checkedItemList.Clear();
    }

    #endregion

    public void SetRandomValue()
    {
        // ���� ���� ���� ����
        curLevel = Random.Range(1, 9);
        //curLevel = 8;
        Debug.Log("���� ����: " + curLevel);
        //ȹ���� ����
        CheckedLevel = Random.Range(0, curLevel);
        //CheckedLevel = 2;
        Debug.Log("ȹ���� ����: " + CheckedLevel);
        // ���� ����
        CheckedRewardLevel = Random.Range(0, CheckedLevel);
        Debug.Log("���� ȹ���� ����: " + CheckedRewardLevel);

    }




    // ���� ���� ���ϱ�


    // =================== PASSLEVEL =================== //
    public void ReadPassLevel()
    {
        var passLevel = ExcelParser.Read("PASS_TABLE-PASSLEVEL");

        foreach (var i in passLevel)
        {
            int id = int.Parse(i.Value["ID"].ToString());
            int level = int.Parse(i.Value["LEVEL"].ToString());
            int point = int.Parse(i.Value["NEEDPOINT"].ToString());
            passLevelData.Add(id, new D_PASSLEVEL(level, point));
        }
    }

    public D_PASSLEVEL GetPassLevelData(int id)
    {
        return passLevelData[id];
    }

    // ============================================================================ //

    // =================== PASSLEVEL =================== //
    private void ReadRewardmain()
    {
        var rewardmain = ExcelParser.Read("REWARD_TABLE-REWARDMAIN");

        foreach (var i in rewardmain)
        {
            int id = int.Parse(i.Value["ID"].ToString());
            int num = int.Parse(i.Value["NUM"].ToString());
            string imagePath = i.Value["IMAGEPATH"].ToString();
            string stringkey = i.Value["STRINGKEY"].ToString();
            rewardmainData.Add(id, new D_REWARDMAIN(id, num, imagePath, stringkey));
        }
    }

    public D_REWARDMAIN GetRewardMainData(int id)
    {
        return rewardmainData[id];
    }
    // ============================================================================ //


    // =================== MISSION =================== //
    private void ReadMissionData()
    {
        var missionType = ExcelParser.Read("MISSION_TABLE-MISSIONTYPE");
        // ID COUNT   STRINGKEY DESCRIPTION
        foreach (var i in missionType)
        {
            int id = int.Parse(i.Value["ID"].ToString());
            int count = int.Parse(i.Value["COUNT"].ToString());
            string stringkey = i.Value["STRINGKEY"].ToString();
            var des = i.Value["DESCRIPTION"].ToString();
            missionTypeData.Add(id, new D_MISSIONTYPE(id, count, stringkey, des));
        }
    }

    public D_MISSIONTYPE GetMissionData(int id)
    {
        return missionTypeData[id];
    }
  


    // ============================================================================ //
}
