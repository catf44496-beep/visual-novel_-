using UnityEngine;

public enum ItemCategory { Survival, Collectible, BlackMarket }
public enum ItemRarity { Common, Uncommon, Rare, Epic }

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public int id;
    public string itemName;
    public ItemCategory category;
    public ItemRarity rarity;
    [TextArea] public string description;
    public string effect;
    public float weight;
    public Sprite icon;
}
