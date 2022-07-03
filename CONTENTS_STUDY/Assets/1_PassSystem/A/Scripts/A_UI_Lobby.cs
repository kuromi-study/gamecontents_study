using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A_UI_Lobby : MonoBehaviour
{
    [SerializeField] private Button _passBtn;

    private void Awake()
    {
        PacketManager.Instance.LoginRequest();
    }

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
        A_PAGE_PASS.Open();
    }
}
