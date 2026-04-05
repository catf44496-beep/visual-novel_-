// ItemData.cs — 所有物品的 ScriptableObject 数据定义
// 右键 Assets > Create > Inventory > Item Data 创建物品

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("基本信息")]
    public int itemID;
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public ItemRarity rarity;

    [Header("描述文本")]
    [TextArea(2, 4)]
    public string description;

    [Header("背景故事（收集品解锁）")]
    [TextArea(3, 8)]
    public string backstory;

    [Header("堆叠设置")]
    public bool isStackable = true;
    public int maxStack = 99;

    [Header("生存效果（仅生存物资）")]
    public SurvivalEffectType effectType = SurvivalEffectType.None;
    public float effectValue = 0f;

    [Header("黑市交易配方（仅收集品）")]
    public List<TradeRecipe> tradeOptions = new List<TradeRecipe>();

    // 稀有度对应颜色
    public Color GetRarityColor()
    {
        return rarity switch
        {
            ItemRarity.Common   => new Color(0.8f, 0.8f, 0.8f),
            ItemRarity.Uncommon => new Color(0.3f, 0.9f, 0.3f),
            ItemRarity.Rare     => new Color(0.3f, 0.5f, 1.0f),
            ItemRarity.Epic     => new Color(0.7f, 0.3f, 1.0f),
            _                   => Color.white
        };
    }

    public string GetRarityName()
    {
        return rarity switch
        {
            ItemRarity.Common   => "普通",
            ItemRarity.Uncommon => "罕见",
            ItemRarity.Rare     => "稀有",
            ItemRarity.Epic     => "史诗",
            _                   => ""
        };
    }
}
