using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A_PAGE_PASS_PASSITEM : MonoBehaviour
{
    [SerializeField] Text _levelText;
    
    [Header("일반 보상")]
    [SerializeField] Button _normalRewardBtn;
    [SerializeField] Image _normalRewardImg;
    [SerializeField] GameObject _normalDimmed;
    [SerializeField] GameObject _normalGetDimmed;
    [SerializeField] GameObject _normalLockDimmed;


    [Header("패스 보상")]
    [SerializeField] Image _passRewardImg1;
    [SerializeField] GameObject _passDimmed1;
    [SerializeField] GameObject _passGetDimmed1;
    [SerializeField] GameObject _passLockDimmed1;
    [SerializeField] Image _passRewardImg2;
    [SerializeField] GameObject _passDimmed2;
    [SerializeField] GameObject _passGetDimmed2;
    [SerializeField] GameObject _passLockDimmed2;

    bool CanGet
    {
        get
        {
            return _normalDimmed.activeSelf == false
                && _normalGetDimmed == false
                && _normalLockDimmed == false;
        }
    }

    Dictionary<string, object> _itemData = new Dictionary<string, object>();
    int _beforeNeedPoint;

    // 해당 패스의 시작점
    public int BeforeNeedPoint
    {
        get => _beforeNeedPoint;
    }

    public void OnEnable()
    {
        _normalRewardBtn.onClick.AddListener(OnClickNormalItem);
    }

    public void SetData(Dictionary<string, object> data, int beforeNeedPoint)
    {
        _itemData = data;
        _beforeNeedPoint = beforeNeedPoint;

        // 레벨세팅
        var passLevelID = _itemData["PASSLEVEL_ID"].ToString();
        var levelTable = ExcelParser.Read("PASS_TABLE-PASSLEVEL");
        var nowLevel = levelTable[passLevelID]["LEVEL"].ToString();
        _levelText.text = nowLevel;

        // 일반아이템세팅
        var normalRewardID = _itemData["NORMAL_REWARD_ID"].ToString();
        var rewardTable = ExcelParser.Read("REWARD_TABLE-REWARDMAIN");
        var normalRewardPath = rewardTable[normalRewardID]["IMAGEPATH"].ToString();

        _normalRewardImg.sprite = Resources.Load<Sprite>(normalRewardPath);

        var normalDimmed = A_PassInfo.Instance.Point < _beforeNeedPoint;
        var getDimmed = A_PassInfo.Instance.Step >= int.Parse(nowLevel);

        _normalDimmed.SetActive(normalDimmed);
        _normalGetDimmed.SetActive(getDimmed);

        // 패스아이템 세팅
        var passRewardID_1 = _itemData["PASS_REWARD_ID_1"].ToString();
        var passRewardPath_1 = rewardTable[passRewardID_1]["IMAGEPATH"].ToString();

        _passRewardImg1.sprite = Resources.Load<Sprite>(passRewardPath_1);

        var lockDimmed = true; // 구매했는지 여부를 갱신하는 함수 추가되어야한다.

        _passDimmed1.SetActive(normalDimmed);
        _passGetDimmed1.SetActive(getDimmed);
        _passLockDimmed1.SetActive(lockDimmed);

        var passRewardID_2 = _itemData["PASS_REWARD_ID_2"].ToString();
        if (passRewardID_2.CompareTo("0") != 0)
        {
            // ID가 0이 아닐때만 갱신해준다.
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

    #region 버튼 리스너처리
    void OnClickNormalItem()
    {
        var normalRewardID = _itemData["NORMAL_REWARD_ID"].ToString();

        if(CanGet == true)
        {
            // 보상획득 팝업 및 패킷발송시켜야한다.

        }
        else
        {
            // 아이템 설명 팝업을 발생시킨다.
            A_POPUP_ITEMINFO.Open(normalRewardID);
        }

    }

    void OnClickPassItem_1()
    {

    }

    void OnClickPassItem_2()
    {

    }
    #endregion
}
