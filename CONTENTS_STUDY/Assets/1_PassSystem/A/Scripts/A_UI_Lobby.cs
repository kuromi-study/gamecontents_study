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
        // �н���ư�� Ŭ���������
        // �н��������� ����ؾ��Ѵ�.
        A_PAGE_PASS.Open();
    }
}
