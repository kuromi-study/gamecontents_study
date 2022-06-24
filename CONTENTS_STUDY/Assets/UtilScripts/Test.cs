using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour
{

    public int _exp = 0;

    void Start()
    {
        List<Dictionary<string, object>> data = ExcelParser.Read("REWARD_TABLE-REWARDMAIN");

        for (var i = 0; i < data.Count; i++)
        {
            Debug.Log($"index {i.ToString()} : {data[i]["ID"]} {data[i]["NUM"]} {data[i]["IMAGEPATH"]}");
        }
    }
}