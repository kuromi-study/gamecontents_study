using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class C_OdinSampleWindwo : OdinMenuEditorWindow
{
    [MenuItem("Tools/InventoryStudy")]
    private static void OpenWindow()
    {
        GetWindow<C_OdinSampleWindwo>().Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;

        tree.Add("인벤토리툴", new TextureUtilityEditor());
        return tree;
    }
}

public class TextureUtilityEditor
{
    [TitleGroup("SelectItem", Order = 1, HorizontalLine = true)]
    [SerializeField]  SelectedItem selectedItem = new SelectedItem();

    [TitleGroup("ItemTab", Order = 2, HorizontalLine = true)]
    [HorizontalGroup("ItemTab/TabGroup")]
    [Button("장비")]
    public void SetListA()
    {
        SelectTab(1);
    }

    [HorizontalGroup("ItemTab/TabGroup")]
    [Button("악세사리")]
    public void SetListB()
    {
        SelectTab(1001);
    }

    [HorizontalGroup("ItemTab/TabGroup")]
    [Button("소모품")]
    public void SetListC()
    {
        SelectTab(2001);
    }

    [TitleGroup("ItemList", Order = 3, HorizontalLine = true)]
    [TableList(ShowIndexLabels =false,AlwaysExpanded =true,NumberOfItemsPerPage =3)]
    [SerializeField] List<NormalItem> _itemList = new List<NormalItem>();

    /// <summary>
    /// 단순 초기화용
    /// </summary>
    [Button("Init")]
    public void Init()
    {
        if (_itemMainInfo.Count == 0)
        {
            _itemMainInfo = ExcelParser.Read("DATA/ITEMTABLE_MAININFO");
        }

        foreach (var it in _itemMainInfo)
        {
            NormalItem temp = new NormalItem();
            temp.SetItemInfo(int.Parse(it.Key));
            temp._selectAction = SelectAction;
            _itemList.Add(temp);
        }
    }

    Dictionary<string, Dictionary<string, object>> _itemMainInfo = new Dictionary<string, Dictionary<string, object>>();


    /// <summary>
    /// 리스트에서 아이템을 선택했을 경우
    /// </summary>
    /// <param name="group"></param>
    public void SelectTab(int group)
    {
        if(_itemMainInfo.Count==0)
        {
            _itemMainInfo = ExcelParser.Read("DATA/ITEMTABLE_MAININFO");
        }

        _itemList.Clear();

        foreach (var it in _itemMainInfo)
        {
            if( int.Parse(it.Value["MAIN_CATEGORY"].ToString()) == group)
            {
                NormalItem temp = new NormalItem();
                temp.SetItemInfo(int.Parse(it.Key));
                temp._selectAction = SelectAction;
                _itemList.Add(temp);
            }
        }
    }

    /// <summary>
    /// 아이템 콜백용
    /// </summary>
    /// <param name="id"></param>
    public void SelectAction(int id)
    {
        selectedItem.SetItemInfo(id);
    }
}

[Serializable]
public class NormalItem
{
    [HorizontalGroup("Left")]
    [PreviewField(alignment: ObjectFieldAlignment.Left, height: 100)]
    [SerializeField, HideLabel()] Sprite selectItemImg;

    [HorizontalGroup("Left")]
    [VerticalGroup("Left/Top")]
    [SerializeField] string name;

    [HorizontalGroup("Left")]
    [VerticalGroup("Left/Top")]
    [SerializeField] string birth;

    int _itemid;

    public Action<int> _selectAction;

    [HorizontalGroup("Left")]
    [VerticalGroup("Left/Top")]
    [Button("선택하기")]
    public void SelectItem()
    {
        _selectAction?.Invoke(_itemid);
    }

    public void SetItemInfo(int id)
    {
        _itemid = id;

           var ItemInfo = C_ItemInfo.GetItemInfo(id) as ItemInfoEquip;

        selectItemImg = Resources.Load<Sprite>(ItemInfo.ImagePath);

        birth = A_StringManager.Instance.GetString(ItemInfo.GradeString);
        name = A_StringManager.Instance.GetString(ItemInfo.NameString);
    }
}

[Serializable]
public class SelectedItem
{
    [HorizontalGroup("Left")]
    [PreviewField(alignment:ObjectFieldAlignment.Left,height:100)]
    [SerializeField, HideLabel()] Sprite selectItemImg;

    [HorizontalGroup("Left")]
    [VerticalGroup("Left/Top")]
    [SerializeField] int enhance;
    [HorizontalGroup("Left")]
    [VerticalGroup("Left/Top")]
    [SerializeField] int gradeup;

    [HorizontalGroup("Left")]
    [VerticalGroup("Left/Bot")]
    [SerializeField] int num;

    [HorizontalGroup("Left")]
    [VerticalGroup("Left/Bot")]
    [Button("추가하기")]
    public void AddItem()
    {
        C_Item_FBS temp = new C_Item_FBS(_itemid);
        C_UserInfo.Instance.itemList.Add(temp);
    }

    int _itemid;

    public void SetItemInfo(int id)
    {
        _itemid = id;

        var ItemInfo = C_ItemInfo.GetItemInfo(id) as ItemInfoEquip;

        selectItemImg = Resources.Load<Sprite>(ItemInfo.ImagePath);
        enhance = 0;
        gradeup = 0;
        num = 0;
    }
}