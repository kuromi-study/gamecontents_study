using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_POPUP_BUYBASE : MonoBehaviour
{
    public enum POPUPType { buyPass,buyLevel};
    public POPUPType popupType = POPUPType.buyPass;

    [SerializeField] Text descriptionTXT;
    [SerializeField] Text cancelTXT;
    [SerializeField] Text OKTXT;
    D_PAGE_PASS pagePass ;

    private void Awake()
    { 
        cancelTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_011"));
        OKTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_012"));
    }

    public void Init(POPUPType type, D_PAGE_PASS pagePass)
    {
        popupType = type;
        this.pagePass = pagePass;
        // ���̾� ����ġ : 1000
        // ���̾� ����ġ : 50
        switch (popupType)
        {
            case POPUPType.buyPass:
                { descriptionTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_013"), 1000); }
                break;
            case POPUPType.buyLevel:
                { descriptionTXT.text = string.Format(D_StringkeyManager.Instance.GetString("ui_pass_010"), 50, D_PassDataManager.Instance.curLevel); }
                break;
        }
    }
    public void BuyPass() { pagePass.BuyPass(); Debug.Log("���̾Ƹ� ����Ͽ� �н� ����"); }
    public void BuyLevel() { pagePass.BuyLevel(); Debug.Log("���̾Ƹ� ����Ͽ� ���� ����"); }

    public void DownBuyPassBtn()
    {
        switch (popupType)
        {
            case POPUPType.buyPass: { BuyPass(); } break;
            case POPUPType.buyLevel: { BuyLevel(); } break;
        }
            
        Destroy(this.gameObject);
    }

    public void DowncancelBtn()
    {
        Destroy(this.gameObject);
    }
}
