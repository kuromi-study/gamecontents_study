using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby : MonoBehaviour
{
    [SerializeField] private Button _passBtn;

    private void OnEnable()
    {
        _passBtn?.onClick.AddListener(OnClickPass);
    }

    private void OnDisable()
    {
        _passBtn?.onClick.RemoveAllListeners();
    }

    public void OnClickPass()
    {
        // 패스버튼을 클릭했을경우
        // 패스페이지를 출력해야한다.

        var uiroot = GameObject.Find("UIRoot");

        var passui = Resources.Load<GameObject>("PAGE_PASS");
        var passui_object = Instantiate<GameObject>(passui);
        passui_object.transform.SetParent(uiroot.transform);

        passui_object.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        passui_object.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
