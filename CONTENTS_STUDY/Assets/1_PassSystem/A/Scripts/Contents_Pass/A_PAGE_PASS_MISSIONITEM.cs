using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class A_PAGE_PASS_MISSIONITEM : MonoBehaviour
{
    [SerializeField] GameObject _missionDimmed;

    [Header("����")]
    [SerializeField] Image _rewardImg;

    [Header("�ӹ� ����")]
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
            // �簻�� ���Ϸ��� return
            // ��� �ǹ����� �̷��� �ȵ�..
            return;
        }

        _missionData = data;

        // �Ϲݾ����ۼ���
        var rewardID = _missionData["REWARD_ID"].ToString();
        var rewardTable = ExcelParser.Read("REWARD_TABLE-REWARDMAIN");
        var rewardPath = rewardTable[rewardID]["IMAGEPATH"].ToString();

        _rewardImg.sprite = Resources.Load<Sprite>(rewardPath);

        // �ӹ����� ����
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

    //�����ư Ŭ����
    private void OnClickComplete()
    {
        if(_missionCount >= 0)
        {
            _missionCount -= 1;
            SetMissionRemainCount();
        }
        else
        {
            // �̼� Ŭ����ó���� �Ϸ��ϱ� �ʹ� �ؾ��ϴ°Ը��Ƽ�..
            // �׳� �ش�̼� ����Ʈ ȹ��ó�����ϰ�
            // ����̼��� Ŭ����ó�����Ϸ�����.
            // REWARD�������� �������� �׳� ������ΰ���...
            var rewardID = _missionData["REWARD_ID"].ToString();

            var rewardTable = ExcelParser.Read("REWARD_TABLE-REWARDMAIN");
            var rewardNum = int.Parse(rewardTable[rewardID]["NUM"].ToString());

            PacketManager.Instance.PassPointRequest(rewardNum);
            SetMissionRemainCount();
        }
    }
}
