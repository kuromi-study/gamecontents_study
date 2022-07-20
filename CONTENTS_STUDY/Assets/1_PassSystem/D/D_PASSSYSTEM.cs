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

        // 포인트 계산하기
        int curPassLevel = D_PassDataManager.Instance.curLevel;

        List<int> points = new List<int>();
        points.Add (D_PassDataManager.Instance.GetPassLevelData(data[1].passLevel_ID).NEEDPOINT + 
                    D_PassDataManager.Instance.GetPassLevelData(data[2].passLevel_ID).NEEDPOINT);

        for (int i = 1; i < count-1; i++)
        {
            points.Add(points[i-1]+ D_PassDataManager.Instance.GetPassLevelData(data[i+1].passLevel_ID).NEEDPOINT);
        }

        points.Add(points[count-2]);

        // 현재 레벨 최대 포인트 계산하기
        D_PassDataManager.Instance.maxPoint = points[curPassLevel-1];

        // 현재 레벨 포인트 랜덤으로 설정하기
        if (curPassLevel != 1)
        {
            D_PassDataManager.Instance.curPoint = Random.Range(points[curPassLevel - 2], points[curPassLevel - 1] - 1);
        }
        else
        {
            int levelID = data[1].passLevel;
            D_PassDataManager.Instance.curPoint = Random.Range(D_PassDataManager.Instance.GetPassLevelData(levelID).NEEDPOINT, points[curPassLevel - 1] - 1);
        }


        D_PASSSYSTEM passSystem = this.GetComponent<D_PASSSYSTEM>();
        
        // PASSITEM 생성
        for (int i = 1; i < count; i++)
        {
            GameObject instance_ = Instantiate<GameObject>(prefab, scrollview.transform);
           instance_.GetComponent<D_PAGE_PASS_PASSITEM>().Init(data[i], points[i - 1], passSystem);
            Debug.Log("Level : "+i+" ,POINT : "+ points[i-1]);
            levelList.Add(instance_);
        }

        // EndReward
        GameObject instance = Instantiate<GameObject>(prefab, scrollview.transform.parent.parent.GetChild(0));
        instance.GetComponent<D_PAGE_PASS_PASSITEM>().Init(data[count], points[count - 1], passSystem);
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

    public void GetAllReward(int level)
    {
        if (D_PassDataManager.Instance.curLevel - 1 < D_PassDataManager.Instance.CheckedLevel)
            return;

        foreach (var i in levelList)
        {
            D_PAGE_PASS_PASSITEM sc = i.GetComponent<D_PAGE_PASS_PASSITEM>();
            if (!sc.normal_dimmed)
                sc.GetNormalReward();
        }

        GameObject prefab = Resources.Load<GameObject>("D_POPUP_GETITEM");
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_GETITEM>().UpdateList(level);
    }

    public void GetReward(int level)
    {
        if (D_PassDataManager.Instance.curLevel - 1 < D_PassDataManager.Instance.CheckedLevel)
            return;

        foreach (var i in levelList)
        {
            D_PAGE_PASS_PASSITEM sc = i.GetComponent<D_PAGE_PASS_PASSITEM>();
            if (sc.data.passLevel <= level && sc.data.passLevel > D_PassDataManager.Instance.CheckedLevel)
                sc.GetNormalReward();
        }

        GameObject prefab = Resources.Load<GameObject>("D_POPUP_GETITEM");
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_GETITEM>().UpdateList(level);
    }

    public void ResetPassSystem()
    {
        for (int i = levelList.Count - 1; i >= 0; i--)
        {
            GameObject obj = levelList[i];
            // 리스트에서 삭제
            levelList.Remove(levelList[i]);
            // 게임오브젝트 제거
            Destroy(obj);
        }
        levelList.Clear();

        OpenPassSystem();
    }

    public void UpdateLevelUp()
    {
        // 현재 포인트 다시 설정
        D_PassDataManager.Instance.curPoint = 
            levelList[D_PassDataManager.Instance.curLevel-1].GetComponent<D_PAGE_PASS_PASSITEM>().needPoint;
        // 레벨 업
        D_PassDataManager.Instance.curLevel++;
        int curPassLevel = D_PassDataManager.Instance.curLevel;
        // 맥스 포인트 다시 설정
        D_PassDataManager.Instance.maxPoint = 
            levelList[curPassLevel - 1].GetComponent<D_PAGE_PASS_PASSITEM>().needPoint;

        // 업데이트된 레벨 텍스트 바꾸기
        levelList[curPassLevel - 1].GetComponent<D_PAGE_PASS_PASSITEM>().UpdatePassLevel();
        // 딤드 처리 풀기

    }
}

