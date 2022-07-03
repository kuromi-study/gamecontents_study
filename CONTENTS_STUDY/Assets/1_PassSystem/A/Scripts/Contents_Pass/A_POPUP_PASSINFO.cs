using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class A_POPUP_PASSINFO : MonoBehaviour
{
    [SerializeField] Text _description;
    [SerializeField] Button _closeBtn;

    static GameObject _thisPopup;

    public static void Open(string key)
    {
        // �н���ư�� Ŭ���������
        // �н��������� ����ؾ��Ѵ�.
        if (_thisPopup == null)
        {
            var uiroot = GameObject.Find("UIRoot");

            var passui = Resources.Load<GameObject>("A_POPUP_PASSINFO");
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

        _thisPopup.GetComponent<A_POPUP_PASSINFO>().InitPopup(key);
    }

    void OnDisable()
    {
        _closeBtn?.onClick.RemoveAllListeners();
    }

    void InitPopup(string key)
    {
        AddBtnListner();

        // description ���ſ�
        _description.SetTextWithStringKey(key);
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
