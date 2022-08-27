using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Singleton;

public class B_DataHolder : MonoSingleton<B_DataHolder>
{
    public Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>();

    public Dictionary<string, Dictionary<string, object>> ENHANCETABLE_COMPOSE_COST;
    public Dictionary<string, Dictionary<string, object>> ENHANCETABLE_COMPOSE_NEXT;
    public Dictionary<string, Dictionary<string, object>> ENHANCETABLE_ENHANCE_COST;
    public Dictionary<string, Dictionary<string, object>> ENHANCETABLE_GRADEUP_COST;
    public Dictionary<string, Dictionary<string, object>> GRADETABLE_GRADEINFO;
    public Dictionary<string, Dictionary<string, object>> ITEMTABLE_CLASSIFICATION;
    public Dictionary<string, Dictionary<string, object>> ITEMTABLE_EQUIPABILITY;
    public Dictionary<string, Dictionary<string, object>> ITEMTABLE_MAININFO;
    public Dictionary<string, Dictionary<string, object>> ITEMTABLE_PORTIONABILITY;
    public Dictionary<string, Dictionary<string, object>> STRINGTABLE;

    // Start is called before the first frame update
    void Awake()
    {
        var sprites = Resources.LoadAll<Sprite>("Sprite/INVENTORY");
        foreach (var sprite in sprites)
        {
            spriteDictionary.Add(sprite.name, sprite);
        }

        ENHANCETABLE_COMPOSE_COST = ExcelParser.Read("ENHANCETABLE_COMPOSE_COST");
        ENHANCETABLE_COMPOSE_NEXT = ExcelParser.Read("ENHANCETABLE_COMPOSE_NEXT");
        ENHANCETABLE_ENHANCE_COST = ExcelParser.Read("ENHANCETABLE_ENHANCE_COST");
        ENHANCETABLE_GRADEUP_COST = ExcelParser.Read("ENHANCETABLE_GRADEUP_COST");
        GRADETABLE_GRADEINFO = ExcelParser.Read("GRADETABLE_GRADEINFO");
        ITEMTABLE_CLASSIFICATION = ExcelParser.Read("ITEMTABLE_CLASSIFICATION");
        ITEMTABLE_EQUIPABILITY = ExcelParser.Read("ITEMTABLE_EQUIPABILITY");
        ITEMTABLE_MAININFO = ExcelParser.Read("ITEMTABLE_MAININFO");
        ITEMTABLE_PORTIONABILITY = ExcelParser.Read("ITEMTABLE_PORTIONABILITY");
        STRINGTABLE = ExcelParser.Read("STRINGTABLE");
    }

    public Dictionary<string, Dictionary<string, object>> GetTable(string tableName)
    {
        Dictionary<string, Dictionary<string, object>> table = new Dictionary<string, Dictionary<string, object>>();
        switch (tableName)
        {
            case "ENHANCETABLE_COMPOSE_COST":
                table = ENHANCETABLE_COMPOSE_COST;
                break;
            case "ENHANCETABLE_COMPOSE_NEXT":
                table = ENHANCETABLE_COMPOSE_NEXT;
                break;
            case "ENHANCETABLE_ENHANCE_COST":
                table = ENHANCETABLE_ENHANCE_COST;
                break;
            case "ENHANCETABLE_GRADEUP_COST":
                table = ENHANCETABLE_GRADEUP_COST;
                break;
            case "GRADETABLE_GRADEINFO":
                table = GRADETABLE_GRADEINFO;
                break;
            case "ITEMTABLE_CLASSIFICATION":
                table = ITEMTABLE_CLASSIFICATION;
                break;
            case "ITEMTABLE_EQUIPABILITY":
                table = ITEMTABLE_EQUIPABILITY;
                break;
            case "ITEMTABLE_MAININFO":
                table = ITEMTABLE_MAININFO;
                break;
            case "ITEMTABLE_PORTIONABILITY":
                table = ITEMTABLE_PORTIONABILITY;
                break;
        }
        return table;
    }

    public string GetValueFromTable(string tableName, string key, string column)
    {
        Dictionary<string, Dictionary<string, object>> table = new Dictionary<string, Dictionary<string, object>>();
        Dictionary<string, object> tmp = new Dictionary<string, object>();
        switch (tableName)
        {
            case "ENHANCETABLE_COMPOSE_COST":
                table = ENHANCETABLE_COMPOSE_COST;
                break;
            case "ENHANCETABLE_COMPOSE_NEXT":
                table = ENHANCETABLE_COMPOSE_NEXT;
                break;
            case "ENHANCETABLE_ENHANCE_COST":
                table = ENHANCETABLE_ENHANCE_COST;
                break;
            case "ENHANCETABLE_GRADEUP_COST":
                table = ENHANCETABLE_GRADEUP_COST;
                break;
            case "GRADETABLE_GRADEINFO":
                table = GRADETABLE_GRADEINFO;
                break;
            case "ITEMTABLE_CLASSIFICATION":
                table = ITEMTABLE_CLASSIFICATION;
                break;
            case "ITEMTABLE_EQUIPABILITY":
                table = ITEMTABLE_EQUIPABILITY;
                break;
            case "ITEMTABLE_MAININFO":
                table = ITEMTABLE_MAININFO;
                break;
            case "ITEMTABLE_PORTIONABILITY":
                table = ITEMTABLE_PORTIONABILITY;
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
    
    /*
    public Sprite GetSpriteByID(int value)
    {
        if (value == 0) return null;
        return spriteDictionary[REWARDMAIN[value.ToString()]["IMAGEPATH"].ToString()];
    }

    public string GetTextByID(int value)
    {
        if (value == 0) return null;
        return GetValueFromTable("STRINGTABLE", REWARDMAIN[value.ToString()]["STRINGKEY"].ToString(), "DESCRIPTION");
    }
    */
}
