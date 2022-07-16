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

        // �̹���
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
        // ���� �������� ������ �ڵ� ��ũ�� �ǵ���
    }

    public void SetRewardState()
    {
        // ȹ���� ����
        int checkedLevel = D_PassDataManager.Instance.CheckedLevel;

        // ���� �� �������� ������ ȹ���� ������ ���ԵǴ� ���
        if (passLevel <= checkedLevel)
        {
            // üũ ǥ��
            normal_dimmed = true;
            normal_type = ItemType.ckecked;
        }

        // ���� ���� ����Ʈ
        int curPoint = D_PassDataManager.Instance.curPoint;
        int curLevel = D_PassDataManager.Instance.curLevel;

        // ���� �ʿ��� ����Ʈ���� ������ ����Ʈ�� ���� ���
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
        Debug.Log("�Ϲݺ��������ȹ��");
        normal_type = ItemType.ckecked;
        normal_dimmed = true;
        SetRewardState();
    }

    public void DownNormalReward()
    {
        Debug.Log("�Ϲݺ��������");

        //ȹ�氡���Ѱ��
        if (!normal_dimmed)
        {
            passSystem.GetReward(passLevel);
        }
        // ȹ�� �Ұ����� ���
        else
        {
            // ������ ���� �˾��� �ߵ���
            GameObject prefab = Resources.Load<GameObject>("D_POPUP_ITEMINFO");
            GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
            popup.GetComponent<D_POPUP_ITEMINFO>().Init(data.normal_reward_ID);
        }
    }

    public void DownPassReward1()
    {
        Debug.Log("�н����������1");

        //ȹ�氡���Ѱ��
        if (!pass1_dimmed)
        {
            Debug.Log("�н����������1ȹ��");
            GameObject prefab = Resources.Load<GameObject>("D_POPUP_GETITEM");
            GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        }
        // ȹ�� �Ұ����� ���
        else
        {
            GameObject prefab = Resources.Load<GameObject>("D_POPUP_ITEMINFO");
            GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
            popup.GetComponent<D_POPUP_ITEMINFO>().Init(data.pass_reward_ID1);
        }
    }
    public void DownPassReward2()
    {
        Debug.Log("�н����������2");
        //ȹ�氡���Ѱ��
        if (!pass2_dimmed)
        {
            Debug.Log("�н����������1ȹ��");
            GameObject prefab = Resources.Load<GameObject>("D_POPUP_GETITEM");
            GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
        }
        // ȹ�� �Ұ����� ���
        else
        {
            GameObject prefab = Resources.Load<GameObject>("D_POPUP_ITEMINFO");
            GameObject popup = Instantiate<GameObject>(prefab, GameObject.Find("Canvas").transform);
            popup.GetComponent<D_POPUP_ITEMINFO>().Init(data.pass_reward_ID2);
        }
    }
}
