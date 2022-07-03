using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A_POPUP_GETITEM_ITEM : MonoBehaviour
{
    [SerializeField] Image _itemImage;

    public void SetData(string itemID)
    {
        var rewardTable = ExcelParser.Read("REWARD_TABLE-REWARDMAIN");
        var normalRewardPath = rewardTable[itemID]["IMAGEPATH"].ToString();

        _itemImage.sprite = Resources.Load<Sprite>(normalRewardPath);
    }
}
