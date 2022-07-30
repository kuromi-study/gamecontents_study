using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_POPUP_GETITEM : MonoBehaviour
{
    [SerializeField] GameObject scrolllview;
    [SerializeField] Text desriptionTXT;
    [SerializeField] GameObject itemObj;
    
    private void Awake()
    {
        desriptionTXT.text = D_StringkeyManager.Instance.GetString("ui_pass_009");
    }

    // �н� �����϶�
    public void UpdateList()
    {
        var list = D_PassDataManager.Instance.GetItemList();

        for (int i = 0; i < list.Count; i++)
        {
            GameObject prefab = Resources.Load<GameObject>("D_ITEM_IMAGE");
            GameObject instance = Instantiate<GameObject>(prefab, scrolllview.transform);
            instance.GetComponent<Image>().sprite = Resources.Load<Sprite>(list[i].IMAGEPATH);
        }
        Debug.Log("UpdateList");
    }

    // �븻 �����϶�
    public void UpdateList(int level)
    {
      var list = D_PassDataManager.Instance.GetItemList();
       
       for(int i = 0; i < list.Count; i++)
       {
            GameObject prefab =Resources.Load<GameObject>("D_ITEM_IMAGE");
            GameObject instance = Instantiate<GameObject>(prefab, scrolllview.transform);
            instance.GetComponent<Image>().sprite = Resources.Load<Sprite>(list[i].IMAGEPATH);
        }
        Debug.Log("UpdateList");
        D_PassDataManager.Instance.CheckedLevel = level;
    }

    public void DownCloseBtn()
    {
        Destroy(this.gameObject);
    }

}
