using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Singleton;

public class GameManager : MonoSingleton<GameManager>
{
    // Start is called before the first frame update
    void Start()
    {
        PacketManager.Instance.LoginRequest();
    }
}
