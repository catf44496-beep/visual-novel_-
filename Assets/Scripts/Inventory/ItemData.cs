using UnityEngine;

public enum ItemCategory { Survival, Collectible, BlackMarket }
public enum ItemRarity { Common, Uncommon, Rare, Epic }

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public int          id;
    public string       itemName;
    public ItemCategory category;
    public ItemRarity   rarity;
    [TextArea] public string description;
    public string       effect;
    public float        weight;
    public Sprite       icon;
    public bool         isUnlocked;
    [TextArea] public string backgroundStory;
    public bool         canExchange;
    public ItemData[]   exchangeRewards;
    public int[]        exchangeAmounts;

    public Color GetRarityColor()
    {
        switch (rarity)
        {
            case ItemRarity.Common:   return new Color(0.70f, 0.70f, 0.70f);
            case ItemRarity.Uncommon: return new Color(0.30f, 0.80f, 0.30f);
            case ItemRarity.Rare:     return new Color(0.30f, 0.50f, 1.00f);
            case ItemRarity.Epic:     return new Color(0.70f, 0.30f, 1.00f);
            default:                  return Color.white;
        }
    }

    public string GetRarityLabel()
    {
        switch (rarity)
        {
            case ItemRarity.Common:   return "普通";
            case ItemRarity.Uncommon: return "非常见";
            case ItemRarity.Rare:     return "稀有";
            case ItemRarity.Epic:     return "史诗";
            default:                  return "";
        }
    }
}