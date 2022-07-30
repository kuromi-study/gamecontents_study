using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class D_PASSSYSTEM : MonoBehaviour
{
    [SerializeField] GameObject scrollview;
    D_PAGE_PASS pagePass;

    [HideInInspector]
    public List<GameObject> levelList = new List<GameObject>();

    private void Awake()
    {
        pagePass = this.GetComponent<D_PAGE_PASS>();
    }


    // ����
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
                int passLevel_id = int.Parse(i.Value["PASSLEVEL_ID"].ToString());
                int passLevel_level = D_PassDataManager.Instance.GetPassLevelData(passLevel_id).LEVEL;
                int normal_reward_id = int.Parse(i.Value["NORMAL_REWARD_ID"].ToString());
                int pass_reward_id1 = int.Parse(i.Value["PASS_REWARD_ID_1"].ToString());
                int pass_reward_id2 = int.Parse(i.Value["PASS_REWARD_ID_2"].ToString());

                D_PASSREWARD temp = new D_PASSREWARD(id, passLevel_id, passLevel_level, normal_reward_id, pass_reward_id1, pass_reward_id2);
                data.Add(count, temp);
            }
        }

        // ����Ʈ ����ϱ�
        int curPassLevel = D_PassDataManager.Instance.curLevel;

        List<int> points = new List<int>();
        points.Add (D_PassDataManager.Instance.GetPassLevelData(data[1].passLevel_ID).NEEDPOINT + 
                    D_PassDataManager.Instance.GetPassLevelData(data[2].passLevel_ID).NEEDPOINT);

        for (int i = 1; i < count-1; i++)
        {
            points.Add(points[i-1]+ D_PassDataManager.Instance.GetPassLevelData(data[i+2].passLevel_ID).NEEDPOINT);
        }

        points.Add(points[count-2]);

        // ���� ���� �ִ� ����Ʈ ����ϱ�
        D_PassDataManager.Instance.maxPoint = points[curPassLevel-1];
        
        // ���� ���� ����Ʈ �������� �����ϱ�
        if (curPassLevel != 1)
        {
            D_PassDataManager.Instance.curPoint = Random.Range(points[curPassLevel - 2], points[curPassLevel - 1] - 1);
        }
        else
        {
            int levelID = data[1].passLevel_ID;
            D_PassDataManager.Instance.curPoint = Random.Range(D_PassDataManager.Instance.GetPassLevelData(levelID).NEEDPOINT, points[curPassLevel - 1] - 1);
        }

        D_PASSSYSTEM passSystem = this.GetComponent<D_PASSSYSTEM>();

        // firstReward
        GameObject instance__ = Instantiate<GameObject>(prefab, scrollview.transform);
        instance__.GetComponent<D_PAGE_PASS_PASSITEM>().Init(data[1], points[0], D_PassDataManager.Instance.GetPassLevelData(data[1].passLevel_ID).NEEDPOINT,passSystem);
        levelList.Add(instance__);

        // PASSITEM ����
        for (int i = 2; i < count; i++)
        {
            GameObject instance_ = Instantiate<GameObject>(prefab, scrollview.transform);
           instance_.GetComponent<D_PAGE_PASS_PASSITEM>().Init(data[i], points[i - 1], points[i - 2], passSystem);
            levelList.Add(instance_);
        }

        // EndReward
        GameObject instance = Instantiate<GameObject>(prefab, scrollview.transform.parent.parent.GetChild(0));
        instance.GetComponent<D_PAGE_PASS_PASSITEM>().Init(data[count], points[count - 1], points[count - 1], passSystem);
        // background �� ����
        for (int i = 0; i < 3; i++) instance.transform.GetChild(i).GetComponent<Image>().color = new Color(1, 0.9f, 0);
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
        if (D_PassDataManager.Instance.curLevel - 1 < D_PassDataManager.Instance.CheckedLevel)
            return;
        D_PassDataManager.Instance.CheckedLevel = D_PassDataManager.Instance.curLevel;

        foreach (var i in levelList)
        {
            D_PAGE_PASS_PASSITEM sc = i.GetComponent<D_PAGE_PASS_PASSITEM>();
            sc.GetNormalReward();
            sc.GetPassRewards();
        }
        ShowGetItemPopup(D_PassDataManager.Instance.CheckedLevel);
    }

    
    public void GetReward(int level)
    {
        // 6 
        if (D_PassDataManager.Instance.curLevel - 1 < D_PassDataManager.Instance.CheckedLevel)
            return;
        foreach (var i in levelList)
        {
            D_PAGE_PASS_PASSITEM sc = i.GetComponent<D_PAGE_PASS_PASSITEM>();
            if (sc.data.passLevel <= level && sc.data.passLevel > D_PassDataManager.Instance.CheckedLevel)
            {
                D_PassDataManager.Instance.CheckedLevel = sc.data.passLevel;
                sc.GetNormalReward();
                // sc.GetPassRewards(); //
                sc.GetPassRewards(); 
            }
        }
        ShowGetItemPopup(level);
    }

    public void ResetPassSystem()
    {
        for (int i = levelList.Count - 1; i >= 0; i--)
        {
            GameObject obj = levelList[i];
            // ����Ʈ���� ����
            levelList.Remove(levelList[i]);
            // ���ӿ�����Ʈ ����
            Destroy(obj);
        }
        levelList.Clear();
        OpenPassSystem();
    }

    public void UpdateLevelUp()
    {
        // ���� ����Ʈ �ٽ� ����
        D_PassDataManager.Instance.curPoint = 
            levelList[D_PassDataManager.Instance.curLevel-1].GetComponent<D_PAGE_PASS_PASSITEM>().needAllPoint;

        
        // ���� ��
        D_PassDataManager.Instance.curLevel++;

        int curPassLevel = D_PassDataManager.Instance.curLevel-1;
       
        // �ƽ� ����Ʈ �ٽ� ����
        D_PassDataManager.Instance.maxPoint = 
            levelList[curPassLevel].GetComponent<D_PAGE_PASS_PASSITEM>().needAllPoint;
        
        // ������Ʈ�� ���� �ؽ�Ʈ �ٲٱ�
         levelList[curPassLevel].GetComponent<D_PAGE_PASS_PASSITEM>().UpdatePassLevel();
    }


    public bool DownBuyPassBtn()
    {
        int curlevel = D_PassDataManager.Instance.curLevel-1;
        // �н� ���Ű� �Ǿ����� �ʴٸ�
        if (!levelList[curlevel].GetComponent<D_PAGE_PASS_PASSITEM>().bActivePassReward)
        {
            Debug.Log("�н� ���� �Ϸ�");
            return true;
        }
        // �н� ���Ű� �Ǿ��ִٸ� �ʴٸ�
        Debug.Log("�н� ���� �Ұ�");
        return false;
    }

    public void BuyPass()
    {
        int curlevel = D_PassDataManager.Instance.curLevel - 1;
        levelList[curlevel].GetComponent<D_PAGE_PASS_PASSITEM>().BuyPass();
    }


    private void ShowGetItemPopup(int level)
    {
        GameObject prefab = Resources.Load<GameObject>("D_POPUP_GETITEM");
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_GETITEM>().UpdateList(level);
    }

}

