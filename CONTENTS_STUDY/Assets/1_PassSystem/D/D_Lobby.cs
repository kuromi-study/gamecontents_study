using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_Lobby : MonoBehaviour
{
    [SerializeField] GameObject pass_page;

    public void PassOpen()
    {
        pass_page.SetActive(true);
    }
}
