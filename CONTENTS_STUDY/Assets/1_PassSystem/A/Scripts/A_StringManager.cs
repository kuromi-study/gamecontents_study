using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Singleton;

public class A_StringManager : MonoSingleton<A_StringManager>
{
    Dictionary<string, Dictionary<string, object>> stringtable;

    public string? GetString(string key)
    {
        if(stringtable == null)
        {
            stringtable = ExcelParser.Read("STRINGTABLE");
        }

        if (stringtable.TryGetValue(key, out var fortext) == true)
        {
            return fortext["DESCRIPTION"].ToString();
        }
        else
        {
            Debug.LogError($"Please Check stringkey : {key}");
            return null;
        }
    }

    public DateTime ConvertStringTimeToDate(string timeStr)
    {
        var year = int.Parse(timeStr.Substring(0, 4));
        var month = int.Parse(timeStr.Substring(4, 2));
        var day = int.Parse(timeStr.Substring(6, 2));
        var hour = int.Parse(timeStr.Substring(8, 2));
        var minutes = int.Parse(timeStr.Substring(10, 2));
        var second = int.Parse(timeStr.Substring(12, 2));

        DateTime time = new DateTime(year, month, day, hour, minutes, second);
        return time;
    }
}
