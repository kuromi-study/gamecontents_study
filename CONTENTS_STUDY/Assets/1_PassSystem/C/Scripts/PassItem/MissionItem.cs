using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class MissionItem : MonoBehaviour
{
    private Image itemImage;
    private Button itemButton;
    private Text timerText;
    private Text progressText;
    private Text descriptionText;
    private Button receiveRewardButton;
    private Image dimmedFilterImage;
    private Image checkImage;

    private int id;
    private int type;
    private long datetype;
    private int missiontype_id;
    private int reward_id;

    private bool isCleared;
    private bool isRewardObtained;
    private DateTime endTime;
    private int curProgress, destProgress;
    

    public void InitItem()
    {
        List<Transform> children = transform.GetComponentsInChildren<Transform>(true).ToList();
        itemImage = children.Find(x => x.name == "btn_normal").GetComponent<Image>();
        itemButton = children.Find(x => x.name == "btn_normal").GetComponent<Button>();
        timerText = children.Find(x => x.name == "txt_time").GetComponent<Text>();
        progressText = children.Find(x => x.name == "txt_count").GetComponent<Text>();
        descriptionText = children.Find(x => x.name == "txt_description").GetComponent<Text>();
        receiveRewardButton = children.Find(x => x.name == "btn_get").GetComponent<Button>();
        dimmedFilterImage = children.Find(x => x.name == "dimmed").GetComponent<Image>();
        checkImage = children.Find(x => x.name == "img_check").GetComponent<Image>();
        
        itemButton.onClick.AddListener(ReceiveReward);
    }

    public void InitData(KeyValuePair<string, Dictionary<string, object>> mainData)
    {
        id = Int32.Parse(mainData.Value["ID"].ToString());
        type = Int32.Parse(mainData.Value["TYPE"].ToString());
        datetype = Int64.Parse(mainData.Value["DATETYPE"].ToString());
        missiontype_id = Int32.Parse(mainData.Value["MISSIONTYPE_ID"].ToString());
        reward_id=Int32.Parse(mainData.Value["REWARD_ID"].ToString());

        itemImage.sprite = DataHolder.Instance.GetSpriteByMissionID(reward_id);
        
        int days = 0;
        var today = DateTime.Today;
        switch (datetype)
        {
            case 0:
                endTime = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);
                break;
            case 1:
                days = (DayOfWeek.Sunday - today.DayOfWeek + 7) % 7;
                endTime = today.AddDays(days).AddHours(23).AddMinutes(59).AddSeconds(59);
                //Debug.Log(endTime);
                break;
            case 2:
                days = DateTime.DaysInMonth(today.Year, today.Month) - today.Day;
                endTime = today.AddDays(days).AddHours(23).AddMinutes(59).AddSeconds(59);
                //Debug.Log(endTime);
                break;
            case 3:
                timerText.text = "";
                break;
        }

        destProgress = Int32.Parse(DataHolder.Instance.MISSIONTYPE_LIST[missiontype_id - 1].Value["COUNT"].ToString());
        progressText.text = $"{curProgress}/{destProgress} 남음";
        switch (missiontype_id)
        {
            case 1:
                MagicBox.Instance.onLogin += UpdateProgress;
                break;
            case 2:
                MagicBox.Instance.onGamePlayed += UpdateProgress;
                break;
            case 3:
                MagicBox.Instance.onGacha += UpdateProgress;
                break;
            case 4:
                MagicBox.Instance.onUpgradeSucceeded += UpdateProgress;
                break;
            case 5:
                MagicBox.Instance.onUpgradeFailed += UpdateProgress;
                break;
            case 6:
                MagicBox.Instance.onItemBought += UpdateProgress;
                break;
            case 7:
                MagicBox.Instance.onItemSold += UpdateProgress;
                break;
            case 8:
                MagicBox.Instance.onGoldSpent += UpdateProgress;
                break;
            case 9:
                MagicBox.Instance.onDiaSpent += UpdateProgress;
                break;
            case 10:
                MagicBox.Instance.onMissionCompleted += UpdateProgress;
                break;
        }
        
        UpdateData();
    }

    public void UpdateData()
    {
        if (datetype == 3) return;
        var earlier = DateTime.Compare(this.endTime, PassManager.Instance.endTime) < 0
            ? this.endTime
            : PassManager.Instance.endTime;
        var remainTimeSpan = earlier.Subtract(DateTime.Now);
        if (remainTimeSpan.Days > 0)
        {
            timerText.text = $"{remainTimeSpan.Days}D 남음";
        }
        else if (remainTimeSpan.Hours > 0)
        {
            timerText.text = $"{remainTimeSpan.Hours}H 남음";
        }
        else if (remainTimeSpan.Minutes > 0)
        {
            timerText.text = $"{remainTimeSpan.Minutes}M 남음";
        }
        else if (remainTimeSpan.Seconds > 0)
        {
            timerText.text = $"{remainTimeSpan.Seconds}S 남음";
        }
    }

    void UpdateProgress()
    {
        curProgress++;
        progressText.text = $"{curProgress}/{destProgress} 남음";
        if (curProgress >= destProgress)
        {
            isCleared = true;
            dimmedFilterImage.gameObject.SetActive(true);
            checkImage.gameObject.SetActive(false);
        }
    }

    void UpdateProgress(int val)
    {
        curProgress += val;
        progressText.text = $"{curProgress}/{destProgress} 남음";
        if (curProgress >= destProgress)
        {
            isCleared = true;
            dimmedFilterImage.gameObject.SetActive(true);
            checkImage.gameObject.SetActive(false);
        }
    }

    void ReceiveReward()
    {
        if (!isCleared || isRewardObtained) return;
        isRewardObtained = true;
        checkImage.gameObject.SetActive(true);
        MagicBox.Instance.GainItem(reward_id);
    }
}
