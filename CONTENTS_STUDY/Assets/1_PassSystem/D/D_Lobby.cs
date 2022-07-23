using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Lobby : MonoBehaviour
{
    [SerializeField] GameObject pass_page;
    bool bOpend = false;
    GameObject page_pass;
    public void PassOpen()
    {
        /*
        pass_page.SetActive(true);
        return;*/
        if (!bOpend)
        {
            GameObject prefab = Resources.Load<GameObject>("D_PAGE_PASS");
            page_pass = Instantiate(prefab, GameObject.Find("Canvas").transform);
            bOpend = true;
        }
        else
        {
            page_pass.SetActive(true);
        }
        
    }
}
