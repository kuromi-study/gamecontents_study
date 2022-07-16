using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class D_PASSSYSTEM : MonoBehaviour
{
    [SerializeField] GameObject scrollview;
    D_PAGE_PASS pagePass;

    List<GameObject> levelList = new List<GameObject>();

    private void Awake()
    {
        pagePass = this.GetComponent<D_PAGE_PASS>();
    }

    // 생성
    public void OpenPassSystem()
    {
        GameObject prefab = Resources.Load<GameObject>("D_PAGE_PASS_PASSITEM");
        Dictionary<int, D_PASSREWARD> data = new Dictionary<int, D_PASSREWARD>();

        var passReward = ExcelParser.Read("PASS_TABLE-PASSREWARD");

        int count = 0;
        foreach (var i in passReward)
        {
            int id = int.Parse(i.Value["PASSMAIN_ID"].ToString());

            if (id == D_PassDataManager.Instance.passID)
            {
                count++;
                //PASSREWARD temp = { int.Parse(i.Value["PASSLEVEL_ID"].ToString(), )
                int passLevel_id = int.Parse(i.Value["PASSLEVEL_ID"].ToString());
                int passLevel_level = D_PassDataManager.Instance.GetPassLevelData(passLevel_id).LEVEL;
                int normal_reward_id = int.Parse(i.Value["NORMAL_REWARD_ID"].ToString());
                int pass_reward_id1 = int.Parse(i.Value["PASS_REWARD_ID_1"].ToString());
                int pass_reward_id2 = int.Parse(i.Value["PASS_REWARD_ID_2"].ToString());

                D_PASSREWARD temp = new D_PASSREWARD(id, passLevel_id, passLevel_level, normal_reward_id, pass_reward_id1, pass_reward_id2);
                data.Add(count, temp);
            }
        }


        int curPassLevel = D_PassDataManager.Instance.curLevel;
        int beforeLevelPoint = D_PassDataManager.Instance.GetPassLevelData(data[1].passLevel_ID).NEEDPOINT;
        int[] points_ = new int[10];
        points_[0] = beforeLevelPoint;
        // 포인트 설정하기
        for (int i = 2; i <= curPassLevel + 1; i++)
        {
            beforeLevelPoint = D_PassDataManager.Instance.maxPoint;
            D_PassDataManager.Instance.maxPoint += D_PassDataManager.Instance.GetPassLevelData(data[i].passLevel_ID).NEEDPOINT;
            points_[i - 2] = D_PassDataManager.Instance.maxPoint;
        }

        //int temp_ = 0;
        int beforepoint_ = 0;
        for (int i = 2; i <= 10; i++)
        {
            beforepoint_ += D_PassDataManager.Instance.GetPassLevelData(data[i].passLevel_ID).NEEDPOINT;
            points_[i - 2] = beforepoint_;
        }

        // 
        D_PassDataManager.Instance.curPoint = Random.Range(beforeLevelPoint, D_PassDataManager.Instance.maxPoint - 1);

        D_PASSSYSTEM passSystem = this.GetComponent<D_PASSSYSTEM>();

        // PASSITEM 생성
        for (int i = 1; i < count; i++)
        {
            GameObject instance_ = Instantiate<GameObject>(prefab, scrollview.transform);
            instance_.GetComponent<D_PAGE_PASS_PASSITEM>().Init(data[i], points_[i - 1], passSystem);
            levelList.Add(instance_);
        }

        // EndReward
        GameObject instance = Instantiate<GameObject>(prefab, scrollview.transform.parent.parent.GetChild(0));
        instance.GetComponent<D_PAGE_PASS_PASSITEM>().Init(data[count], beforepoint_, passSystem);
        levelList.Add(instance);

        scrollview.SetActive(true);
    }


    public void ActivePassSystem()
    {
        foreach (var i in levelList)
            i.SetActive(true);
    }

    public void InactivePassSystem()
    {
        foreach (var i in levelList)
            i.SetActive(false);
    }

    public void GetAllReward()
    {
        if (D_PassDataManager.Instance.curLevel - 1 > D_PassDataManager.Instance.CheckedLevel)
        {
            foreach (var i in levelList)
            {
                D_PAGE_PASS_PASSITEM sc = i.GetComponent<D_PAGE_PASS_PASSITEM>();
                if (!sc.normal_dimmed )
                    sc.GetNormalReward();
            }
        }
        GameObject prefab = Resources.Load<GameObject>("D_POPUP_GETITEM");
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_GETITEM>().UpdateList();
    }

    public void GetReward(int level)
    {
        bool b = false;
        if (D_PassDataManager.Instance.curLevel - 1 > D_PassDataManager.Instance.CheckedLevel)
        {
            foreach (var i in levelList)
            {
                D_PAGE_PASS_PASSITEM sc = i.GetComponent<D_PAGE_PASS_PASSITEM>();
                if (sc.data.passLevel <= level &&sc.data.passLevel > D_PassDataManager.Instance.CheckedLevel)
                    sc.GetNormalReward();
            }
        }
        GameObject prefab = Resources.Load<GameObject>("D_POPUP_GETITEM");
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_GETITEM>().UpdateList();
    }
}

