using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[HideInInspector]
public enum ItemType { none, ckecked, locked };

public class D_PAGE_PASS_PASSITEM : MonoBehaviour
{
    // ========== 오브젝트 ==========
    [SerializeField] Text passLevelTXT;
    [SerializeField] Image normalPassImg;
    [SerializeField] Image[] rewardPassImg;
    // [SerializeField] Image rewardPass2Img;


    // ========== 패스 관련 변수들 ==========
    int passID;
    int passLevel;
    public int needAllPoint { get; private set; }
    public int prevNeedPoint { get; private set; }
    public D_PASSREWARD data { get; private set; }

    // ========== normal reward Item ==========
    ItemType normal_type = ItemType.none;
    public D_PASSSYSTEM passSystem;

    // ========== pass reward Item ==========
    ItemType pass1_type = ItemType.none;
    ItemType pass2_type = ItemType.none;
    public bool bActivePassReward { get; private set; }


    private void Awake()
    {
        // 패스 구매 여부 초기화
        bActivePassReward = false;
    }

    public void Init(D_PASSREWARD data_,int needPoint_,int prevNeedPoint_, D_PASSSYSTEM passSystem_)
    {
        data = data_;
        passID = data.passID;
        passLevel = data.passLevel;
        passSystem = passSystem_;
        needAllPoint = needPoint_;
        prevNeedPoint = prevNeedPoint_;
        passLevelTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_005"), data.passLevel.ToString());


        // 이미지
        if (data.normal_reward_ID != 0)
            normalPassImg.sprite = Resources.Load<Sprite>(D_PassDataManager.Instance.GetRewardMainData(data.normal_reward_ID).IMAGEPATH);
        else
            normalPassImg.gameObject.SetActive(false);

        rewardPassImg[0].sprite = Resources.Load<Sprite>(D_PassDataManager.Instance.GetRewardMainData(data.pass_reward_ID1).IMAGEPATH);

        if (data.pass_reward_ID2 != 0)
            rewardPassImg[1].sprite = Resources.Load<Sprite>(D_PassDataManager.Instance.GetRewardMainData(data.pass_reward_ID2).IMAGEPATH);
        else
            rewardPassImg[1].gameObject.SetActive(false);

        // text
        if (D_PassDataManager.Instance.curLevel >= passLevel)
            passLevelTXT.color = new Color(1, 1, 1);
        else
            passLevelTXT.color = new Color(0, 0, 0);

        // 저장해야되는 변수
        D_PassDataManager.Instance.ActiveRewardPassState[passLevel] = bActivePassReward;

        UpdatePassLevel();

        if (normal_type == ItemType.ckecked)
        {
            D_PassDataManager.Instance.AddList(D_PassDataManager.Instance.GetRewardMainData(data.normal_reward_ID));
        }
    }

    public void UpdatePassLevel()
    {
        if (D_PassDataManager.Instance.curLevel >= passLevel)
            passLevelTXT.color = new Color(1, 1, 1);
        else
            passLevelTXT.color = new Color(0, 0, 0);

        UpdatePassReward(0);
        UpdatePassReward(1);
        UpdateNormalReward();

    }

    #region RewardPass

    public void BuyPass()
    {
        bActivePassReward = true;
        UpdatePassReward(0);
        UpdatePassReward(1);
        UpdateNormalReward();
        D_PassDataManager.Instance.ActiveRewardPassState[passLevel] = bActivePassReward;
    }

    private void UpdatePassReward(int index)
    {
        if (data.pass_reward_ID1 == 0 && index == 0) return; 
        if (data.pass_reward_ID2 == 0 && index == 1) return;

        int curlevel = D_PassDataManager.Instance.curLevel;

        // 현재 레벨이 아직 패스레벨보다 낮을 때
        if (passLevel > curlevel)
            rewardPassImg[index].transform.GetChild(0).gameObject.SetActive(true);
        else
        rewardPassImg[index].transform.GetChild(0).gameObject.SetActive(false);
            

        // 경우 생각 하기
        // 레벨 구매 + 획득한 상태
        if (bActivePassReward && pass1_type == ItemType.ckecked)
        {
            rewardPassImg[index].transform.GetChild(1).gameObject.SetActive(true);
            rewardPassImg[index].transform.GetChild(2).gameObject.SetActive(false);
            return;
        }

        // 레벨 구매 + 획득하지 않은 상태
        if (bActivePassReward && pass1_type == ItemType.none)
        {
            rewardPassImg[index].transform.GetChild(1).gameObject.SetActive(false);
            rewardPassImg[index].transform.GetChild(2).gameObject.SetActive(false);
            return;
        }

        // 획득 불가능한 상태 + 구매가능한 레벨이지만 패스 사지 않음
        rewardPassImg[index].transform.GetChild(1).gameObject.SetActive(false);
        rewardPassImg[index].transform.GetChild(2).gameObject.SetActive(true);
    }
    #endregion

    // dimmed : 0 , get : 1

    public void UpdateNormalReward()
    {
        int checkedLevel = D_PassDataManager.Instance.CheckedLevel;
        int curlevel = D_PassDataManager.Instance.curLevel;
        int curPoint = D_PassDataManager.Instance.curPoint;

        // 획득 불가능한 경우 : 포인트가 도달하지 못한 경우
        if (prevNeedPoint > curPoint)
        {
            normal_type = ItemType.locked;
            normalPassImg.transform.GetChild(0).gameObject.SetActive(true);
            normalPassImg.transform.GetChild(1).gameObject.SetActive(false);
            return;
        }

        // 획득 불가능한 경우 : 이미 획득한 경우
        if (passLevel <= checkedLevel || normal_type == ItemType.ckecked)
        {
            normal_type =  ItemType.ckecked;
            normalPassImg.transform.GetChild(0).gameObject.SetActive(false);
            normalPassImg.transform.GetChild(1).gameObject.SetActive(true);

            return;
        }

        // 획득 가능한 경우
        if (normal_type != ItemType.ckecked)
        {
            normal_type = ItemType.none;
            normalPassImg.transform.GetChild(0).gameObject.SetActive(false);
            normalPassImg.transform.GetChild(1).gameObject.SetActive(false);

            return;
        }
    }

    public void GetNormalReward()
    {
        int curlevel = D_PassDataManager.Instance.curLevel;

        if (normal_type == ItemType.ckecked || curlevel < passLevel) return;

        normal_type = ItemType.ckecked;
        D_PassDataManager.Instance.AddList(D_PassDataManager.Instance.GetRewardMainData(data.normal_reward_ID));
       
        UpdateNormalReward();
    }


    public void DownNormalReward()
    {
        int curPoint = D_PassDataManager.Instance.curPoint;

        // 획득 불가능한 경우 : 이미 획득 했거나, 레벨이 부족한 경우
        if (normal_type == ItemType.ckecked || normal_type == ItemType.locked)
        {
            ShowItemInfo(data.normal_reward_ID);
            return;
        }

        passSystem.GetReward(passLevel);
    }

    public void DownPassReward1()
    {
        if (bActivePassReward && pass1_type == ItemType.none)
        {
            pass1_type = ItemType.ckecked;
            UpdatePassReward(0);

            D_PassDataManager.Instance.AddList(D_PassDataManager.Instance.GetRewardMainData(data.pass_reward_ID1));

            ShowGetItempopup();
        }
        else if(!bActivePassReward  || pass1_type == ItemType.ckecked)
        {
            ShowItemInfo(data.pass_reward_ID1);
        }
    }

    public void DownPassReward2()
    {
        if (bActivePassReward && pass2_type == ItemType.none)
        {
            pass2_type = ItemType.ckecked;
            UpdatePassReward(1);

            D_PassDataManager.Instance.AddList(D_PassDataManager.Instance.GetRewardMainData(data.pass_reward_ID2));

            ShowGetItempopup();
        }
        else if (!bActivePassReward || pass2_type == ItemType.ckecked)
        {
            ShowItemInfo(data.pass_reward_ID2);
        }
    }

    public void ShowGetItempopup()
    {
        GameObject prefab = Resources.Load<GameObject>("D_POPUP_GETITEM");
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_GETITEM>().UpdateList();
    }

    // 아이템 정보 보여주는  함수 

    public void ShowItemInfo(int itemLevel)
    {
        GameObject prefab = Resources.Load<GameObject>("D_POPUP_ITEMINFO");
        GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        popup.GetComponent<D_POPUP_ITEMINFO>().Init(itemLevel);
    }
}
