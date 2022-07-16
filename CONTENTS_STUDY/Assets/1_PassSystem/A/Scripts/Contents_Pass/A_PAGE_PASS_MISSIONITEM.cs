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

    int _missionCount;

    Dictionary<string, object> _missionData = new Dictionary<string, object>();

    public bool isClear
    {
        get => _missionDimmed.activeSelf;
    }

    private void OnEnable()
    {
        _completeBtn.onClick.AddListener(OnClickComplete);
    }

    private void OnDisable()
    {
        _completeBtn.onClick.RemoveAllListeners();
    }

    public void SetData(Dictionary<string, object> data)
    {
        if (isClear == true)
        {
            // 재갱신 못하려고 return
            // 사실 실무에서 이러면 안됨..
            return;
        }

        _missionData = data;

        // 일반아이템세팅
        var rewardID = _missionData["REWARD_ID"].ToString();
        var rewardTable = ExcelParser.Read("REWARD_TABLE-REWARDMAIN");
        var rewardPath = rewardTable[rewardID]["IMAGEPATH"].ToString();

        _rewardImg.sprite = Resources.Load<Sprite>(rewardPath);

        // 임무설명 세팅
        var missionTypeID = _missionData["MISSIONTYPE_ID"].ToString();
        var missionTypeTable = ExcelParser.Read("MISSION_TABLE-MISSIONTYPE");
        _missionCount = int.Parse(missionTypeTable[missionTypeID]["COUNT"].ToString());
        var missionStringKey = missionTypeTable[missionTypeID]["STRINGKEY"].ToString();

        SetMissionRemainCount();
        _descriptionText.SetTextWithStringKey(missionStringKey);
    }

    private void SetMissionRemainCount()
    {
        if(_missionCount < 0)
        {
            _missionCount = 0;
            _missionDimmed.SetActive(true);
        }
        else
        {
            _missionDimmed.SetActive(false);
        }

        var progressStr = A_StringManager.Instance.GetString("ui_pass_008");
        progressStr = string.Format(progressStr, _missionCount.ToString());
        _progressText.SetTextWithString(progressStr);
    }

    //보상버튼 클릭시
    private void OnClickComplete()
    {
        if(_missionCount >= 0)
        {
            _missionCount -= 1;
            SetMissionRemainCount();
        }
        else
        {
            // 미션 클리어처리를 하려니까 너무 해야하는게많아서..
            // 그냥 해당미션 포인트 획득처리를하고
            // 현재미션을 클리어처리를하려고함.
            // REWARD재참조가 귀찮지만 그냥 정석대로가자...
            var rewardID = _missionData["REWARD_ID"].ToString();

            var rewardTable = ExcelParser.Read("REWARD_TABLE-REWARDMAIN");
            var rewardNum = int.Parse(rewardTable[rewardID]["NUM"].ToString());

            PacketManager.Instance.PassPointRequest(rewardNum);
            SetMissionRemainCount();
        }
    }
}
