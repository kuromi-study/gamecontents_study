using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A_PAGE_PASS_PASSITEM : MonoBehaviour
{
    [SerializeField] Text _levelText;
    
    [Header("일반 보상")]
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

    Dictionary<string, object> _itemData = new Dictionary<string, object>();

    public void SetData(Dictionary<string, object> data)
    {
        _itemData = data;

        // 레벨세팅
        var passLevelID = _itemData["PASSLEVEL_ID"].ToString();
        var levelTable = ExcelParser.Read("PASS_TABLE-PASSLEVEL");
        var nowLevel = levelTable[passLevelID]["LEVEL"];
        _levelText.text = nowLevel.ToString();

        // 일반아이템세팅
        var normalRewardID = _itemData["NORMAL_REWARD_ID"].ToString();
        var rewardTable = ExcelParser.Read("REWARD_TABLE-REWARDMAIN");
        var normalRewardPath = rewardTable[normalRewardID]["IMAGEPATH"].ToString();

        _normalRewardImg.sprite = Resources.Load<Sprite>(normalRewardPath);

        // 패스아이템 세팅
        var passRewardID_1 = _itemData["PASS_REWARD_ID_1"].ToString();
        var passRewardPath_1 = rewardTable[passRewardID_1]["IMAGEPATH"].ToString();

        _passRewardImg1.sprite = Resources.Load<Sprite>(passRewardPath_1);

        var passRewardID_2 = _itemData["PASS_REWARD_ID_2"].ToString();
        if (passRewardID_2.CompareTo("0") != 0)
        {
            // ID가 0이 아닐때만 갱신해준다.
            var passRewardPath_2 = rewardTable[passRewardID_2]["IMAGEPATH"].ToString();

            _passRewardImg2.sprite = Resources.Load<Sprite>(passRewardPath_2);
            _passRewardImg2.gameObject.SetActive(true);
        }
        else
        {
            _passRewardImg2.gameObject.SetActive(false);
        }
    }
}
