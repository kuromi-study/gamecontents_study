using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class D_POPUP_PASSINFO : MonoBehaviour
{

    public void Init(string description)
    {
        this.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = description;
    }
    
    public void DownCloseBtn()
    {
        Destroy(this.gameObject);
    }
}
