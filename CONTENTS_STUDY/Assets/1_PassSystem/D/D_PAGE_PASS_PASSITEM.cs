using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_PAGE_PASS_PASSITEM : MonoBehaviour
{
    [SerializeField] Text passLevelTXT;
    [SerializeField] Image normalPassImg;
    [SerializeField] Image rewardPass1Img;
    [SerializeField] Image rewardPass2Img;

    public enum ItemType { none, ckecked, locked };

    public ItemType normal_type = ItemType.none;
    ItemType pass1_type = ItemType.none;
    ItemType pass2_type = ItemType.none;
    public bool normal_dimmed { get; private set; }
    public bool pass1_dimmed { get; private set; }
    public bool pass2_dimmed { get; private set; }

    int passID;
    int passLevel;
    
    public int needPoint;
    public D_PASSREWARD data { get; private set; }
    public D_PASSSYSTEM passSystem;

    public void Init(D_PASSREWARD data_,int needPoint_,D_PASSSYSTEM passSystem_)
    {
        data = data_;
        passID = data.passID;
        passLevel = data.passLevel;
        passSystem = passSystem_;
        passLevelTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_005"), data.passLevel.ToString());
        if (D_PassDataManager.Instance.curLevel >= passLevel)
            passLevelTXT.color = new Color(1, 1, 1);
        else
            passLevelTXT.color = new Color(0, 0, 0);

        // 이미지
        if (data.normal_reward_ID != 0)
            normalPassImg.sprite = Resources.Load<Sprite>(D_PassDataManager.Instance.GetRewardMainData(data.normal_reward_ID).IMAGEPATH);
        else
            normalPassImg.gameObject.SetActive(false);

        rewardPass1Img.sprite = Resources.Load<Sprite>(D_PassDataManager.Instance.GetRewardMainData(data.pass_reward_ID1).IMAGEPATH);

        if (data.pass_reward_ID2 != 0)
            rewardPass2Img.sprite = Resources.Load<Sprite>(D_PassDataManager.Instance.GetRewardMainData(data.pass_reward_ID2).IMAGEPATH);
        else
            rewardPass2Img.gameObject.SetActive(false);
        needPoint = needPoint_;

        SetRewardState();
        // 현재 진행중인 보상이 자동 스크롤 되도록
    }

    public void SetRewardState()
    {
        // 획득한 레벨
        int checkedLevel = D_PassDataManager.Instance.CheckedLevel;

        // 현재 이 아이템의 레벨이 획득한 레벨에 포함되는 경우
        if (passLevel <= checkedLevel)
        {
            // 체크 표시
            normal_dimmed = true;
            normal_type = ItemType.ckecked;
        }

        // 현재 보유 포인트
        int curPoint = D_PassDataManager.Instance.curPoint;
        int curLevel = D_PassDataManager.Instance.curLevel;

        // 현재 필요한 포인트보다 보유한 포인트가 낮은 경우
        if (needPoint > curPoint+100)
        {
            normal_dimmed = true;
            normal_type = ItemType.locked;
        }

        // 
        pass1_dimmed = true;
        pass2_dimmed = true;
        pass1_type = ItemType.locked;
        pass2_type = ItemType.locked;
        //

        UpdateItems();
        SetRewardImg();
    }

    public void SetRewardImg()
    {
        if (normal_dimmed)
            normalPassImg.transform.GetChild(0).gameObject.SetActive(true);

        switch (normal_type)
        {
            case ItemType.ckecked: { normalPassImg.transform.GetChild(1).gameObject.SetActive(true); }break;
        }

        if(pass1_dimmed)
            rewardPass1Img.transform.GetChild(0).gameObject.SetActive(true);

        if (pass2_dimmed)
            rewardPass2Img.transform.GetChild(0).gameObject.SetActive(true);

        switch (pass1_type)
        {
            case ItemType.ckecked: { rewardPass1Img.transform.GetChild(1).gameObject.SetActive(true); } break;
            case ItemType.locked: { rewardPass1Img.transform.GetChild(2).gameObject.SetActive(true); } break;
        }

        switch (pass2_type)
        {
            case ItemType.ckecked: { rewardPass2Img.transform.GetChild(1).gameObject.SetActive(true); } break;
            case ItemType.locked: { rewardPass2Img.transform.GetChild(2).gameObject.SetActive(true); } break;
        }
    }

    public void UpdateItems()
    {
        if(normal_type == ItemType.ckecked)
            D_PassDataManager.Instance.AddList(D_PassDataManager.Instance.GetRewardMainData(data.normal_reward_ID));
    }

    public void GetNormalReward()
    {
        Debug.Log("일반보상아이템획득");
        normal_type = ItemType.ckecked;
        normal_dimmed = true;
        SetRewardState();
    }

    public void DownNormalReward()
    {
        Debug.Log("일반보상아이템");

        //획득가능한경우
        if (!normal_dimmed)
        {
            passSystem.GetReward(passLevel);
        }
        // 획득 불가능한 경우
        else
        {
            // 아이템 설명 팝업이 뜨도록
            GameObject prefab = Resources.Load<GameObject>("D_POPUP_ITEMINFO");
            GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
            popup.GetComponent<D_POPUP_ITEMINFO>().Init(data.normal_reward_ID);
        }
    }

    public void DownPassReward1()
    {
        Debug.Log("패스보상아이템1");

        //획득가능한경우
        if (!pass1_dimmed)
        {
            Debug.Log("패스보상아이템1획득");
            GameObject prefab = Resources.Load<GameObject>("D_POPUP_GETITEM");
            GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        }
        // 획득 불가능한 경우
        else
        {
            GameObject prefab = Resources.Load<GameObject>("D_POPUP_ITEMINFO");
            GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
            popup.GetComponent<D_POPUP_ITEMINFO>().Init(data.pass_reward_ID1);
        }
    }
    public void DownPassReward2()
    {
        Debug.Log("패스보상아이템2");
        //획득가능한경우
        if (!pass2_dimmed)
        {
            Debug.Log("패스보상아이템1획득");
            GameObject prefab = Resources.Load<GameObject>("D_POPUP_GETITEM");
            GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        }
        // 획득 불가능한 경우
        else
        {
            GameObject prefab = Resources.Load<GameObject>("D_POPUP_ITEMINFO");
            GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
            popup.GetComponent<D_POPUP_ITEMINFO>().Init(data.pass_reward_ID2);
        }
    }
}
