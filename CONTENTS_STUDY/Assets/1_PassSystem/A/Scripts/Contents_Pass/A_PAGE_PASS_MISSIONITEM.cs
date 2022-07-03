using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class A_PAGE_PASS_MISSIONITEM : MonoBehaviour
{
    [SerializeField] GameObject _missionDimmed;

    [Header("보상")]
    [SerializeField] Image _rewardImg;

    [Header("임무 설명")]
    [SerializeField] Text _remainTimeText;
    [SerializeField] Text _progressText;
    [SerializeField] Text _descriptionText;

    [SerializeField] Button _completeBtn;


    Dictionary<string, object> _missionData = new Dictionary<string, object>();

    public void SetData(Dictionary<string, object> data)
    {
        _missionData = data;

        // 일반아이템세팅
        var rewardID = _missionData["REWARD_ID"].ToString();
        var rewardTable = ExcelParser.Read("REWARD_TABLE-REWARDMAIN");
        var rewardPath = rewardTable[rewardID]["IMAGEPATH"].ToString();

        _rewardImg.sprite = Resources.Load<Sprite>(rewardPath);

        // 임무설명 세팅
        var missionTypeID = _missionData["MISSIONTYPE_ID"].ToString();
        var missionTypeTable = ExcelParser.Read("MISSION_TABLE-MISSIONTYPE");
        var missionCount = missionTypeTable[missionTypeID]["COUNT"].ToString();
        var missionStringKey = missionTypeTable[missionTypeID]["STRINGKEY"].ToString();

        var progressStr = A_StringManager.Instance.GetString("ui_pass_008");
        progressStr = string.Format(progressStr, missionCount);
        _progressText.SetTextWithString(progressStr);
        _descriptionText.SetTextWithStringKey(missionStringKey);
    }
}
