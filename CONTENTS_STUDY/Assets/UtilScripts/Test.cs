using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Test : MonoBehaviour
{

    public int _exp = 0;

    void Start()
    {
       var data = ExcelParser.Read("MISSION_TABLE-MISSIONTYPE");

        //for (var i = 0; i < data.Count; i++)
        //{
        //    Debug.Log($"index {i.ToString()} : {data[i]["ID"]} {data[i]["NUM"]} {data[i]["IMAGEPATH"]}");
        //}

        foreach(var it in data)
        {
            Debug.Log($"ID : {it.Value["ID"]}  COUNT :: { it.Value["COUNT"]} STRINGKEY :: { it.Value["STRINGKEY"]}");
        }

        var stringtable = ExcelParser.Read("STRINGTABLE");
        stringtable.TryGetValue("ui_pass_001", out var needstring);
        Debug.Log($"{needstring["DESCRIPTION"]}");

        // 1 :: id값
        var key = data["1"]["STRINGKEY"].ToString();
        stringtable.TryGetValue(key, out var needstring2);
        Debug.Log($"{needstring2["DESCRIPTION"]}");
    }
}