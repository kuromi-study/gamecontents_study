using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Singleton;

public class MagicBox : MonoSingleton<MagicBox>
{
    public int playerLevel=1, curExp, reqExp;

    public delegate void NoParamDelegate();

    public NoParamDelegate onLogin,
        onGamePlayed,
        onGacha,
        onUpgradeSucceeded,
        onUpgradeFailed,
        onItemBought,
        onItemSold,
        onMissionCompleted,
        onLevelUp;

    public delegate void SingleParamDelegate(int value);
    public SingleParamDelegate onGoldSpent, onDiaSpent;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            CompleteMission();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Login();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            PlayGame();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            Gacha();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            SucceedUpgrade();
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            FailUpgrade();
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            BuyItem();
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            SellItem();
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            SpendGold(1000);
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            SpendDiamond(1000);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LevelUp();
        }
    }

    public void Login()
    {
        Debug.Log($"Player Logged in");
        onLogin();
    }

    public void PlayGame()
    {
        Debug.Log($"Player played a game");
        onGamePlayed();
    }

    public void Gacha()
    {
        Debug.Log($"Player gacha'ed");
        onGacha();
    }

    public void SucceedUpgrade()
    {
        Debug.Log($"Player succeeded to upgrade an item");
        onUpgradeSucceeded();
    }

    public void FailUpgrade()
    {
        Debug.Log($"Player failed to upgrade an item");
        onUpgradeFailed();
    }
    
    public void BuyItem()
    {
        Debug.Log($"Player bought an item");
        onItemBought();
    }
    
    public void SellItem()
    {
        Debug.Log($"Player sold an item!");
        onItemSold();
    }

    public void SpendGold(int value)
    {
        Debug.Log($"Player spent {value} gold");
        onGoldSpent(value);
    }
    
    public void SpendDiamond(int value)
    {
        Debug.Log($"Player spent {value} diamond");
        onDiaSpent(value);
    }

    public void CompleteMission()
    {
        Debug.Log($"Player completed a mission");
        onMissionCompleted();
    }
    
    public void GainItem(int itemKey)
    {
        if (itemKey == 0) return;
        var descriptionKey=DataHolder.Instance.GetValueFromTable("REWARDMAIN",itemKey.ToString(),"STRINGKEY");
        var itemStr = DataHolder.Instance.GetValueFromTable("STRINGTABLE", descriptionKey, "DESCRIPTION");
        
        //var descriptionKey = DataHolder.Instance.REWARDMAIN[itemKey.ToString()]["STRINGKEY"].ToString();
        //var itemStr = DataHolder.Instance.STRINGTABLE[descriptionKey]["DESCRIPTION"].ToString();
        if(itemStr==null) Debug.Log("such item does not exist");
        else Debug.Log($"Player gained item {itemStr}");
        if (itemKey == 10002)
        {
            GainEXP(100);
        }
    }

    public void GainEXP(int amount)
    {
        
    }

    public void LevelUp()
    {
        Debug.Log("Player level Up!");
        playerLevel++;
        onLevelUp();
    }
}
