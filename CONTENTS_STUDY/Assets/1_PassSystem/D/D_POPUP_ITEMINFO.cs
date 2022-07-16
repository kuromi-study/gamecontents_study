using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_POPUP_ITEMINFO : MonoBehaviour
{
    int itemID;
    [SerializeField] Image itemImg;
    [SerializeField] Text dexriptionTXT;

    public void Init(int itemID_)
    {
        itemID = itemID_;
        string path = D_PassDataManager.Instance.GetRewardMainData(itemID).IMAGEPATH;
        itemImg.sprite = Resources.Load<Sprite>(path);
        string strkey = D_PassDataManager.Instance.GetRewardMainData(itemID).STRINGKEY;
        dexriptionTXT.text = D_StringkeyManager.Instance.GetString(strkey);
    }



    public void DownCloseBtn()
    {
        Destroy(this.gameObject);
    }
}
