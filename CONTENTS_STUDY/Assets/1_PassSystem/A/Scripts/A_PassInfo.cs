using System;
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


    #region �ӽ� ��������Ʈ��
    // ���� �и��ؾ���.
    private Dictionary<string, List<Action>> eventManager = new Dictionary<string, List<Action>>();

    public readonly string PASS_EVENT_NAME = "PASSEVENT";

    public void AddEvent(string key, Action action)
    {
        if(eventManager.ContainsKey(key) == true)
        {
            eventManager[key].Add(action);
        }
        else
        {
            eventManager.Add(key, new List<Action>());
            eventManager[key].Add(action);
        }
    }

    public void RemoveEvent(string key, Action action)
    {
        if (eventManager.ContainsKey(key) == true)
        {
            eventManager[key].Remove(action);
        }
        else
        {
            Debug.LogError("Please Check Event Name");
        }
    }

    public void RemoveEventAll(string key)
    {
        if (eventManager.ContainsKey(key) == true)
        {
            eventManager.Remove(key);
        }
        else
        {
            Debug.LogError("Please Check Event Name");
        }
    }

    public void BroadCastEvent(string key)
    {
        if (eventManager.ContainsKey(key) == true)
        {
            foreach (var it in eventManager[key])
            {
                it?.Invoke();
            }
        }
        else
        {
            Debug.LogError("Please Check Event Name");
        }
    }
    #endregion
}
