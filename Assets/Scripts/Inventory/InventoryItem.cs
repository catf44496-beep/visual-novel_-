// InventoryItem.cs — 背包中物品的运行时实例（数据 + 数量）

using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int amount;

    public InventoryItem(ItemData data, int amount = 1)
    {
        this.data = data;
        this.amount = Mathf.Clamp(amount, 1, data.maxStack);
    }

    // 增加数量，返回溢出量（背包满时无法堆叠的部分）
    public int AddAmount(int count)
    {
        int overflow = 0;
        int newAmount = amount + count;
        if (newAmount > data.maxStack)
        {
            overflow = newAmount - data.maxStack;
            amount = data.maxStack;
        }
        else
        {
            amount = newAmount;
        }
        return overflow;
    }

    public bool IsMaxStack => amount >= data.maxStack;
    public bool IsEmpty    => amount <= 0;
}
