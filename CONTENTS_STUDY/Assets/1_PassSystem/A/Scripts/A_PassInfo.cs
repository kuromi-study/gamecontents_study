using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Singleton;

public class A_PassInfo : MonoSingleton<A_PassInfo>
{
    // point -- ��������Ʈ
    // step -- ���罺��

    private PassPoint _passInfo;

    public int Step
    {
        get
        {
            return _passInfo.Step;
        }
    }

    public int Point
    {
        get
        {
            return _passInfo.Point;
        }
    }

    public void SetPassInfo(PassPoint passpoint)
    {
        _passInfo = passpoint;
    }
}
