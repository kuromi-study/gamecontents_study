using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utility.Singleton;

public class B_PlayerInfo : MonoSingleton<B_PlayerInfo>
{
    private Transform CharacterInfoTr;

    private B_ItemDisplay helmetDisplay;
    private B_ItemDisplay armorDisplay;
    private B_ItemDisplay shoesDisplay;
    private B_ItemDisplay necklaceDisplay;
    private B_ItemDisplay ringDisplay;
    private B_ItemDisplay beltDisplay;
    
    private B_InventoryItem equippedHelmet;
    private B_InventoryItem equippedArmor;
    private B_InventoryItem equippedShoes;
    private B_InventoryItem equippedNecklace;
    private B_InventoryItem equippedRing;
    private B_InventoryItem equippedBelt;

    private Text battlePowerText;
    private Text attackText;
    private Text defenceText;
    private Text add_hpText;
    private Text add_mpText;

    public int battlePower;
    public int attack;
    public int defence;
    public int add_hp;
    public int add_mp;
    public int gold;
    
    void Awake()
    {
        BindObjects();
    }
    
    public void EquipItem(B_InventoryItem item)
    {
        switch (item.itemData.subCategoryKey)
        {
            case "2":
                if (equippedHelmet != null)
                {
                    equippedHelmet.itemData.isEquipped = false;
                    equippedHelmet.onItemChanged -= UpdatePlayerStat;
                }
                equippedHelmet = item;
                item.itemData.isEquipped = true;
                item.onItemChanged += UpdatePlayerStat;
                helmetDisplay.SetInventoryItem(item);
                break;
            case "3":
                if (equippedArmor != null)
                {
                    equippedArmor.itemData.isEquipped = false;
                    equippedArmor.onItemChanged -= UpdatePlayerStat;
                }
                equippedArmor = item;
                item.itemData.isEquipped = true;
                armorDisplay.SetInventoryItem(item);
                break;
            case "4":
                if (equippedShoes != null)
                {
                    equippedShoes.itemData.isEquipped = false;
                    equippedShoes.onItemChanged -= UpdatePlayerStat;
                }
                equippedShoes = item;
                item.itemData.isEquipped = true;
                shoesDisplay.SetInventoryItem(item);
                break;
            case "1002":
                if (equippedNecklace != null)
                {
                    equippedNecklace.itemData.isEquipped = false;
                    equippedNecklace.onItemChanged -= UpdatePlayerStat;
                }
                equippedNecklace = item;
                item.itemData.isEquipped = true;
                necklaceDisplay.SetInventoryItem(item);
                break;
            case "1003":
                if (equippedRing != null)
                {
                    equippedRing.itemData.isEquipped = false;
                    equippedRing.onItemChanged -= UpdatePlayerStat;
                }
                equippedRing = item;
                item.itemData.isEquipped = true;
                ringDisplay.SetInventoryItem(item);
                break;
            case "1004":
                if (equippedBelt != null)
                {
                    equippedBelt.itemData.isEquipped = false;
                    equippedBelt.onItemChanged -= UpdatePlayerStat;
                }
                equippedBelt = item;
                item.itemData.isEquipped = true;
                beltDisplay.SetInventoryItem(item);
                break;
            case "2002":
                break;
            case "2003":
                break;
        }
        UpdatePlayerStat();
        //Replace in Right Panel
    }

    public void UpdatePlayerStat()
    {
        attack = defence = add_hp = add_mp = 0;
        if (equippedHelmet != null)
        {
            attack += equippedHelmet.itemData.itemAbility1;
            defence += equippedHelmet.itemData.itemAbility2;
            add_hp += equippedHelmet.itemData.itemAbility3;
            add_mp += equippedHelmet.itemData.itemAbility4;
        }
        if (equippedArmor != null)
        {
            attack += equippedArmor.itemData.itemAbility1;
            defence += equippedArmor.itemData.itemAbility2;
            add_hp += equippedArmor.itemData.itemAbility3;
            add_mp += equippedArmor.itemData.itemAbility4;
        }
        if (equippedShoes != null)
        {
            attack += equippedShoes.itemData.itemAbility1;
            defence += equippedShoes.itemData.itemAbility2;
            add_hp += equippedShoes.itemData.itemAbility3;
            add_mp += equippedShoes.itemData.itemAbility4;
        }
        if (equippedNecklace != null)
        {
            attack += equippedNecklace.itemData.itemAbility1;
            defence += equippedNecklace.itemData.itemAbility2;
            add_hp += equippedNecklace.itemData.itemAbility3;
            add_mp += equippedNecklace.itemData.itemAbility4;
        }
        if (equippedRing != null)
        {
            attack += equippedRing.itemData.itemAbility1;
            defence += equippedRing.itemData.itemAbility2;
            add_hp += equippedRing.itemData.itemAbility3;
            add_mp += equippedRing.itemData.itemAbility4;
        }
        if (equippedBelt != null)
        {
            attack += equippedBelt.itemData.itemAbility1;
            defence += equippedBelt.itemData.itemAbility2;
            add_hp += equippedBelt.itemData.itemAbility3;
            add_mp += equippedBelt.itemData.itemAbility4;
        }

        battlePower = attack * 10 + defence * 5 + add_hp + add_mp;
        UpdateStatUI();
    }

    public void UpdateStatUI()
    {
        battlePowerText.text = $"전투력 : {battlePower}";
        attackText.text = $"공격력 : {attack}";
        defenceText.text = $"방어력 : {defence}";
        add_hpText.text = $"HP : {add_hp}";
        add_mpText.text = $"MP : {add_mp}";
    }

    void BindObjects()
    {
        CharacterInfoTr = GameObject.Find("CharacterInfo").transform;
        List<Transform> children = CharacterInfoTr.GetComponentsInChildren<Transform>(true).ToList();

        helmetDisplay = children.Find(x => x.name == "btn_equip_head").GetComponent<B_ItemDisplay>();
        armorDisplay = children.Find(x => x.name == "btn_equip_body").GetComponent<B_ItemDisplay>();
        shoesDisplay = children.Find(x => x.name == "btn_equip_leg").GetComponent<B_ItemDisplay>();
        necklaceDisplay = children.Find(x => x.name == "btn_acc_necklace").GetComponent<B_ItemDisplay>();
        ringDisplay = children.Find(x => x.name == "btn_acc_ring").GetComponent<B_ItemDisplay>();
        beltDisplay = children.Find(x => x.name == "btn_acc_belt").GetComponent<B_ItemDisplay>();

        battlePowerText = children.Find(x => x.name == "txt_all").GetComponent<Text>();
        attackText = children.Find(x => x.name == "txt_attack").GetComponent<Text>();
        defenceText = children.Find(x => x.name == "txt_defence").GetComponent<Text>();
        add_hpText = children.Find(x => x.name == "txt_HP").GetComponent<Text>();
        add_mpText = children.Find(x => x.name == "txt_MP").GetComponent<Text>();
    }
}
