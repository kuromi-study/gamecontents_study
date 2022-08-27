using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ItemDataSO", menuName = "New ItemDataSO/itemDataSO")]
[Serializable]
public class ItemDataSO : ScriptableObject
{
    public int ID;
    public string mainCategory;
    public string subCategory;
    public string grade;
    public int enhance;
    public int star;
    public int quantity;
    public bool isEquipped;
    public bool isLocked;
}
