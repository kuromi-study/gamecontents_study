using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class A_PAGE_PASS : MonoBehaviour
{
    [Header("��� ������Ʈ")]
    [SerializeField] Button _backBtn;
    [SerializeField] Text _backBtnText;
    [SerializeField] Button _infoBtn;
    [SerializeField] Text _remainTimeText;
    [SerializeField] Button _purchaseBtn;
    [SerializeField] Text _purchaseBtnText;

    [Header("����Ʈ ������Ʈ")]
    [SerializeField] GameObject _rewardPrefab;
    [SerializeField] GameObject _missionPrefab;
    [SerializeField] GameObject _lastReward;
    [SerializeField] GameObject _scrollView;

    static GameObject _thispage;

    public static void Open()
    {
        // �н���ư�� Ŭ���������
        // �н��������� ����ؾ��Ѵ�.
        if (_thispage == null)
        {
            var uiroot = GameObject.Find("UIRoot");

            var passui = Resources.Load<GameObject>("A_PAGE_PASS");
            _thispage = Instantiate<GameObject>(passui);
            _thispage.transform.SetParent(uiroot.transform);

            var recttransform = _thispage.GetComponent<RectTransform>();
            recttransform.sizeDelta = Vector2.zero;
            recttransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            _thispage.SetActive(true);
        }

        _thispage.GetComponent<A_PAGE_PASS>().InitPage();
    }

    void OnDisable()
    {
        _backBtn?.onClick.RemoveAllListeners();
        _infoBtn?.onClick.RemoveAllListeners();
    }

    void InitPage()
    {
        AddBtnListner();
        RefreshTopLayer();
    }

    void AddBtnListner()
    {
        _backBtn?.onClick.AddListener(OnClickClose);
        _infoBtn?.onClick.AddListener(OnClickInfo);
    }

    void RefreshTopLayer()
    {
        _backBtnText.SetTextWithStringKey("ui_pass_001");

        var remainStr = A_StringManager.Instance.GetString("ui_pass_002");
        if(remainStr != null)
        {
            remainStr = string.Format(remainStr, "1", "�ð�");
            _remainTimeText.SetTextWithString(remainStr);
        }

        _purchaseBtnText.SetTextWithStringKey("ui_pass_003");
    }

    #region ��ư ������ ó��
    void OnClickClose()
    {
        _thispage.SetActive(false);
    }

    void OnClickInfo()
    {

    }
    #endregion
}
