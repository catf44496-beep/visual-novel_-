using System;

[Serializable]
public class InventoryEntry
{
    public ItemData item;
    public int count;

    public InventoryEntry(ItemData item, int count = 1)
    {
        this.item = item;
        this.count = count;
    }
}
