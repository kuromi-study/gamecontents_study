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
    public List<KeyValuePair<string, Dictionary<string, object>>> REWARDMAIN_LIST;
    public List<KeyValuePair<string, Dictionary<string, object>>> STRINGTABLE_LIST;
    
    // Start is called before the first frame update
    void Start()
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
        REWARDMAIN_LIST = REWARDMAIN.ToList();
        STRINGTABLE_LIST = STRINGTABLE.ToList();
    }

    public Sprite GetSpriteByMissionID(int value)
    {
        return spriteDictionary[REWARDMAIN[value.ToString()]["IMAGEPATH"].ToString()];
    }
}
