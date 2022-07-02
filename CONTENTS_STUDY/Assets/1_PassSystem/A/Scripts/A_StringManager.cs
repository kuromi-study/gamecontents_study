using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Singleton;

public class A_StringManager : MonoSingleton<A_StringManager>
{
    public string? GetString(string key)
    {
        var stringtable = ExcelParser.Read("STRINGTABLE");
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
}
