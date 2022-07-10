using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A_PAGE_PASS_PASSITEM : MonoBehaviour
{
    [SerializeField] Text _levelText;
    
    [Header("�Ϲ� ����")]
    [SerializeField] Button _normalRewardBtn;
    [SerializeField] Image _normalRewardImg;
    [SerializeField] GameObject _normalDimmed;
    [SerializeField] GameObject _normalGetDimmed;
    [SerializeField] GameObject _normalLockDimmed;


    [Header("�н� ����")]
    [SerializeField] Button _passRewardBtn1;
    [SerializeField] Image _passRewardImg1;
    [SerializeField] GameObject _passDimmed1;
    [SerializeField] GameObject _passGetDimmed1;
    [SerializeField] GameObject _passLockDimmed1;
    [SerializeField] Button _passRewardBtn2;
    [SerializeField] Image _passRewardImg2;
    [SerializeField] GameObject _passDimmed2;
    [SerializeField] GameObject _passGetDimmed2;
    [SerializeField] GameObject _passLockDimmed2;

    Action<int, bool> _OnClickGetItem;

    Dictionary<string, object> _itemData = new Dictionary<string, object>();
    int _beforeNeedPoint;

    bool NormalCanGet
    {
        get
        {
            return _normalDimmed.activeSelf == false
                && _normalGetDimmed.activeSelf == false;
        }
    }

    bool PassCanGet
    {
        // pass1�� pass2�� �ᱹ �������������� 1���� ��ħ
        get
        {
            return _passDimmed1.activeSelf == false
                && _passGetDimmed1.activeSelf == false
                && _passLockDimmed1.activeSelf == false;
        }
    }

    // �ش� �н��� ������
    public int BeforeNeedPoint
    {
        get => _beforeNeedPoint;
    }

    public int PassLevel
    {
        get
        {
            var passLevelID = _itemData["PASSLEVEL_ID"].ToString();
            var levelTable = ExcelParser.Read("PASS_TABLE-PASSLEVEL");
            var nowLevel = int.Parse(levelTable[passLevelID]["LEVEL"].ToString());

            return nowLevel;
        }
    }

    void OnEnable()
    {
        _normalRewardBtn.onClick.AddListener(OnClickNormalItem);
        _passRewardBtn1.onClick.AddListener(OnClickPassItem_1);
        _passRewardBtn2.onClick.AddListener(OnClickPassItem_2);
    }

    void OnDisable()
    {
        _normalRewardBtn.onClick.RemoveAllListeners();
    }

    public void SetData(Dictionary<string, object> data, int beforeNeedPoint, Action<int, bool> action = null)
    {
        _itemData = data;
        _beforeNeedPoint = beforeNeedPoint;
        _OnClickGetItem = action;

        // ��������
        _levelText.text = PassLevel.ToString();

        // �Ϲݾ����ۼ���
        var normalRewardID = _itemData["NORMAL_REWARD_ID"].ToString();
        var rewardTable = ExcelParser.Read("REWARD_TABLE-REWARDMAIN");
        var normalRewardPath = rewardTable[normalRewardID]["IMAGEPATH"].ToString();

        _normalRewardImg.sprite = Resources.Load<Sprite>(normalRewardPath);

        var normalDimmed = A_PassInfo.Instance.Point < _beforeNeedPoint;
        var getDimmed = A_PassInfo.Instance.NormalStep >= PassLevel;

        _normalDimmed.SetActive(normalDimmed);
        _normalGetDimmed.SetActive(getDimmed);

        // �н������� ����
        getDimmed = A_PassInfo.Instance.PassStep >= PassLevel;

        var passRewardID_1 = _itemData["PASS_REWARD_ID_1"].ToString();
        var passRewardPath_1 = rewardTable[passRewardID_1]["IMAGEPATH"].ToString();

        _passRewardImg1.sprite = Resources.Load<Sprite>(passRewardPath_1);

        var lockDimmed = !A_PassInfo.Instance.Premium; // �����ߴ��� ���θ� �����ϴ� �Լ� �߰��Ǿ���Ѵ�.

        _passDimmed1.SetActive(normalDimmed);
        _passGetDimmed1.SetActive(getDimmed);
        _passLockDimmed1.SetActive(lockDimmed);

        var passRewardID_2 = _itemData["PASS_REWARD_ID_2"].ToString();
        if (passRewardID_2.CompareTo("0") != 0)
        {
            // ID�� 0�� �ƴҶ��� �������ش�.
            var passRewardPath_2 = rewardTable[passRewardID_2]["IMAGEPATH"].ToString();

            _passRewardImg2.sprite = Resources.Load<Sprite>(passRewardPath_2);
            _passRewardImg2.gameObject.SetActive(true);

            _passDimmed2.SetActive(normalDimmed);
            _passGetDimmed2.SetActive(getDimmed);
            _passLockDimmed2.SetActive(lockDimmed);
        }
        else
        {
            _passRewardImg2.gameObject.SetActive(false);
        }
    }

    #region ��ư ������ó��
    void OnClickNormalItem()
    {
        var normalRewardID = _itemData["NORMAL_REWARD_ID"].ToString();

        if(NormalCanGet == true)
        {
            // ����ȹ�� �˾� �� ��Ŷ�߼۽��Ѿ��Ѵ�.
            _OnClickGetItem.Invoke(PassLevel, false);
            PacketManager.Instance.PassRewardRequest(PassLevel, isPremium : false);
        }
        else
        {
            // ������ ���� �˾��� �߻���Ų��.
            A_POPUP_ITEMINFO.Open(normalRewardID);
        }
    }

    void OnClickPassItem_1()
    {
        var passItemID = _itemData["PASS_REWARD_ID_1"].ToString();

        if (PassCanGet == true)
        {
            // ����ȹ�� �˾� �� ��Ŷ�߼۽��Ѿ��Ѵ�.
            _OnClickGetItem.Invoke(PassLevel, true);
            PacketManager.Instance.PassRewardRequest(PassLevel, isPremium : true);
        }
        else
        {
            // ������ ���� �˾��� �߻���Ų��.
            A_POPUP_ITEMINFO.Open(passItemID);
        }
    }

    void OnClickPassItem_2()
    {

        var passItemID = _itemData["PASS_REWARD_ID_2"].ToString();

        if (PassCanGet == true)
        {
            // ����ȹ�� �˾� �� ��Ŷ�߼۽��Ѿ��Ѵ�.
            _OnClickGetItem.Invoke(PassLevel, true);
            PacketManager.Instance.PassRewardRequest(PassLevel, isPremium : true);
        }
        else
        {
            // ������ ���� �˾��� �߻���Ų��.
            A_POPUP_ITEMINFO.Open(passItemID);
        }
    }
    #endregion
}
