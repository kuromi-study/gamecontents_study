using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Singleton;

public class DataHolder : MonoSingleton<DataHolder>
{
    public Dictionary<string, Sprite> spriteDictionary=new Dictionary<string, Sprite>();

    public Dictionary<string, Dictionary<string, object>> MISSIONMAIN;
    public Dictionary<string, Dictionary<string, object>> MISSIONTYPE;
    public Dictionary<string, Dictionary<string, object>> PASSLEVEL;
    public Dictionary<string, Dictionary<string, object>> PASSMAIN;
    public Dictionary<string, Dictionary<string, object>> PASSREWARD;
    public Dictionary<string, Dictionary<string, object>> REWARDMAIN;
    public Dictionary<string, Dictionary<string, object>> STRINGTABLE;

    public List<KeyValuePair<string, Dictionary<string, object>>> MISSIONTYPE_LIST;
    public List<KeyValuePair<string, Dictionary<string, object>>> PASSLEVEL_LIST;
    public List<KeyValuePair<string, Dictionary<string, object>>> REWARDMAIN_LIST;
    public List<KeyValuePair<string, Dictionary<string, object>>> STRINGTABLE_LIST;
    
    // Start is called before the first frame update
    void Awake()
    {
        var sprites = Resources.LoadAll<Sprite>("sprites");
        foreach (var sprite in sprites)
        {
            spriteDictionary.Add(sprite.name, sprite);
        }

        MISSIONMAIN = ExcelParser.Read("MISSION_TABLE-MISSIONMAIN");
        MISSIONTYPE = ExcelParser.Read("MISSION_TABLE-MISSIONTYPE");
        PASSLEVEL = ExcelParser.Read("PASS_TABLE-PASSLEVEL");
        PASSMAIN = ExcelParser.Read("PASS_TABLE-PASSMAIN");
        PASSREWARD = ExcelParser.Read("PASS_TABLE-PASSREWARD");
        REWARDMAIN = ExcelParser.Read("REWARD_TABLE-REWARDMAIN");
        STRINGTABLE = ExcelParser.Read("STRINGTABLE");

        MISSIONTYPE_LIST = MISSIONTYPE.ToList();
        PASSLEVEL_LIST = PASSLEVEL.ToList();
        REWARDMAIN_LIST = REWARDMAIN.ToList();
        STRINGTABLE_LIST = STRINGTABLE.ToList();
    }

    public string GetValueFromTable(string tableName, string key, string column)
    {
        Debug.Log(key);
        Dictionary<string, Dictionary<string, object>> table = new Dictionary<string, Dictionary<string, object>>();
        Dictionary<string, object> tmp = new Dictionary<string, object>();
        switch (tableName)
        {
            case "REWARDMAIN":
                table = REWARDMAIN;
                break;
            case "STRINGTABLE":
                table = STRINGTABLE;
                break;
        }
        if (table == null || tmp==null) return null;
        var ts1 = table.TryGetValue(key.ToString(), out tmp);
        if (ts1)
        {
            object ret;
            var ts2 = tmp.TryGetValue(column, out ret);
            if (ts1 && ts2) return ret.ToString();
        }
        return null;
    }

    public int GetActualLevel(int value)
    {
        return Int32.Parse(PASSLEVEL_LIST[value-1].Value["LEVEL"].ToString());
    }
    
    public Sprite GetSpriteByMissionID(int value)
    {
        if (value == 0) return null;
        return spriteDictionary[REWARDMAIN[value.ToString()]["IMAGEPATH"].ToString()];
    }

    public string GetTextByMissionID(int value)
    {
        if (value == 0) return null;
        return GetValueFromTable("STRINGTABLE", REWARDMAIN[value.ToString()]["STRINGKEY"].ToString(), "DESCRIPTION");
    }
}
