using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem : MonoBehaviour
{
    private int id;
    private int passID;
    private int reqLevel;
    
    private Text levelText;
    
    private GameObject normalItemObj;
    private Image normalItemImage;
    private Button normalItemButton;
    private Image normalItemNormalImage;
    private Image normalItemGetImage;
    private Image normalItemLockImage;
    private int normalRewardID;
    private bool canObtainNormal;
    private bool isNormalObtained;

    private GameObject pass1ItemObj;
    private Image pass1ItemImage;
    private Button pass1ItemButton;
    private Image pass1ItemNormalImage;
    private Image pass1ItemGetImage;
    private Image pass1ItemLockImage;
    private int pass1RewardID;
    private bool canObtainPass1;
    private bool isPass1Obtained;
    
    private GameObject pass2ItemObj;
    private Image pass2ItemImage;
    private Button pass2ItemButton;
    private Image pass2ItemNormalImage;
    private Image pass2ItemGetImage;
    private Image pass2ItemLockImage;
    private int pass2RewardID;
    private bool canObtainPass2;
    private bool isPass2Obtained;

    

    public void InitItem()
    {
        List<Transform> children = transform.GetComponentsInChildren<Transform>(true).ToList();

        levelText=children.Find(x => x.name == "txt_lv").GetComponent<Text>();
        
        normalItemObj = children.Find(x => x.name == "reward_normal").gameObject;
        normalItemButton = normalItemObj.transform.GetChild(0).GetComponent<Button>();
        normalItemImage = normalItemButton.GetComponent<Image>();
        normalItemNormalImage = normalItemButton.transform.GetChild(0).GetComponent<Image>();
        normalItemGetImage = normalItemButton.transform.GetChild(1).GetComponent<Image>();
        normalItemLockImage = normalItemButton.transform.GetChild(2).GetComponent<Image>();
        
        pass1ItemButton = children.Find(x => x.name == "btn_pass_01").GetComponent<Button>();
        pass1ItemImage = pass1ItemButton.GetComponent<Image>();
        pass1ItemNormalImage = pass1ItemButton.transform.GetChild(0).GetComponent<Image>();
        pass1ItemGetImage = pass1ItemButton.transform.GetChild(1).GetComponent<Image>();
        pass1ItemLockImage = pass1ItemButton.transform.GetChild(2).GetComponent<Image>();
        
        pass2ItemButton = children.Find(x => x.name == "btn_pass_02").GetComponent<Button>();
        pass2ItemImage = pass2ItemButton.GetComponent<Image>();
        pass2ItemNormalImage = pass2ItemButton.transform.GetChild(0).GetComponent<Image>();
        pass2ItemGetImage = pass2ItemButton.transform.GetChild(1).GetComponent<Image>();
        pass2ItemLockImage = pass2ItemButton.transform.GetChild(2).GetComponent<Image>();


        normalItemButton.onClick.AddListener(delegate { ReceiveReward(0); });
        pass1ItemButton.onClick.AddListener(delegate { ReceiveReward(1); });
        pass2ItemButton.onClick.AddListener(delegate { ReceiveReward(2); });

        MagicBox.Instance.onLevelUp += UpdateProgress;
    }

    public void InitData(KeyValuePair<string, Dictionary<string, object>> mainData)
    {
        id = Int32.Parse(mainData.Value["ID"].ToString());
        
        
        
        passID = Int32.Parse(mainData.Value["PASSMAIN_ID"].ToString());
        //reqLevel = Int32.Parse(mainData.Value["PASSLEVEL_ID"].ToString());
        reqLevel = DataHolder.Instance.GetActualLevel(Int32.Parse(mainData.Value["PASSLEVEL_ID"].ToString()));
        
        normalRewardID = Int32.Parse(mainData.Value["NORMAL_REWARD_ID"].ToString());
        pass1RewardID = Int32.Parse(mainData.Value["PASS_REWARD_ID_1"].ToString());
        pass2RewardID = Int32.Parse(mainData.Value["PASS_REWARD_ID_2"].ToString());

        levelText.text = $"LV.{reqLevel}";
        normalItemImage.sprite = DataHolder.Instance.GetSpriteByMissionID(normalRewardID);
        pass1ItemImage.sprite = DataHolder.Instance.GetSpriteByMissionID(pass1RewardID);
        pass2ItemImage.sprite = DataHolder.Instance.GetSpriteByMissionID(pass2RewardID);

        MagicBox.Instance.onLevelUp += UpdateProgress;
        
        UpdateProgress();
    }
    

    void UpdateProgress()
    {
        if (MagicBox.Instance.playerLevel >= reqLevel)
        {
            canObtainNormal = canObtainPass1 = canObtainPass2 = true;
            normalItemNormalImage.gameObject.SetActive(true);
            pass1ItemNormalImage.gameObject.SetActive(true);
            pass2ItemNormalImage.gameObject.SetActive(true);
        }
    }


    void ReceiveReward(int num)
    {
        switch (num)
        {
            case 0:
                if (!canObtainNormal || isNormalObtained) return;
                normalItemGetImage.gameObject.SetActive(true);
                MagicBox.Instance.GainItem(normalRewardID);
                break;
            case 1:
                if (!canObtainPass1 || isPass1Obtained) return;
                pass1ItemGetImage.gameObject.SetActive(true);
                MagicBox.Instance.GainItem(pass1RewardID);
                break;
            case 2:
                if (!canObtainPass2 || isPass2Obtained) return;
                pass2ItemGetImage.gameObject.SetActive(true);
                MagicBox.Instance.GainItem(pass2RewardID);
                break;
        }
    }

    public void ReceiveAll()
    {
        if (!canObtainNormal || isNormalObtained) return;
        isNormalObtained = true;
        normalItemGetImage.gameObject.SetActive(true);
        MagicBox.Instance.GainItem(normalRewardID);
        
        if (!canObtainPass1 || isPass1Obtained) return;
        isPass1Obtained = true;
        pass1ItemGetImage.gameObject.SetActive(true);
        MagicBox.Instance.GainItem(pass1RewardID);
        
        if (!canObtainPass2 || isPass2Obtained) return;
        isPass2Obtained = true;
        pass2ItemGetImage.gameObject.SetActive(true);
        MagicBox.Instance.GainItem(pass2RewardID);
    }
}
