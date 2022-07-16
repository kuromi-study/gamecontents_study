using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Singleton;

public class D_StringkeyManager : MonoSingleton<D_StringkeyManager>
{
    Dictionary<string, Dictionary<string, object>> stringtable;


    private void Awake()
    {
        stringtable = ExcelParser.Read("STRINGTABLE");
    }


    public string GetString(string key)
    {
        if (stringtable == null)
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

    
}
