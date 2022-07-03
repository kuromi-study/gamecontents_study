using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class A_POPUP_ITEMINFO : MonoBehaviour
{
    [SerializeField] Image _itemImg;
    [SerializeField] Text _description;
    [SerializeField] Button _closeBtn;

    static GameObject _thisPopup;

    public static void Open(string id)
    {
        // 패스버튼을 클릭했을경우
        // 패스페이지를 출력해야한다.
        if (_thisPopup == null)
        {
            var uiroot = GameObject.Find("UIRoot");

            var passui = Resources.Load<GameObject>("A_POPUP_ITEMINFO");
            _thisPopup = Instantiate<GameObject>(passui);
            _thisPopup.transform.SetParent(uiroot.transform);

            var recttransform = _thisPopup.GetComponent<RectTransform>();
            recttransform.sizeDelta = Vector2.zero;
            recttransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            _thisPopup.SetActive(true);
        }

        _thisPopup.GetComponent<A_POPUP_ITEMINFO>().InitPopup(id);
    }

    void OnDisable()
    {
        _closeBtn?.onClick.RemoveAllListeners();
    }

    void InitPopup(string id)
    {
        AddBtnListner();

        // 아이템 이미지 갱신용
        var rewardTable = ExcelParser.Read("REWARD_TABLE-REWARDMAIN");
        var normalRewardPath = rewardTable[id]["IMAGEPATH"].ToString();

        _itemImg.sprite = Resources.Load<Sprite>(normalRewardPath);

        // 설명 추가용
        var descriptionKey = rewardTable[id]["STRINGKEY"].ToString();
        _description.SetTextWithStringKey(descriptionKey);
    }

    void AddBtnListner()
    {
        _closeBtn?.onClick.AddListener(OnClickClose);
    }

    void OnClickClose()
    {
        _thisPopup.SetActive(false);
    }
}
