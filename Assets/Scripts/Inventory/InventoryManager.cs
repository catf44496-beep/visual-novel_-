// InventoryManager.cs — 背包核心逻辑，单例模式
// 挂载在场景中的空物体 "GameManager" 上

using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("背包设置")]
    [SerializeField] private int maxSlots = 24;

    // 运行时背包数据
    private List<InventoryItem> items = new List<InventoryItem>();

    // 事件：UI 订阅刷新显示
    public event Action OnInventoryChanged;

    // ──────────────────────────────────────────
    // 生命周期
    // ──────────────────────────────────────────

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ──────────────────────────────────────────
    // 公开 API
    // ──────────────────────────────────────────

    public IReadOnlyList<InventoryItem> Items => items;
    public int SlotCount => maxSlots;
    public int UsedSlots => items.Count;
    public bool IsFull   => items.Count >= maxSlots;

    /// <summary>添加物品。返回 false 表示背包已满无法添加。</summary>
    public bool AddItem(ItemData data, int amount = 1)
    {
        if (data == null) return false;

        int remaining = amount;

        // 优先堆叠到现有格子
        if (data.isStackable)
        {
            foreach (var item in items)
            {
                if (item.data == data && !item.IsMaxStack)
                {
                    remaining = item.AddAmount(remaining);
                    if (remaining <= 0) break;
                }
            }
        }

        // 剩余量放入新格子
        while (remaining > 0)
        {
            if (IsFull)
            {
                Debug.LogWarning($"[Inventory] 背包已满，无法添加 {data.itemName} x{remaining}");
                OnInventoryChanged?.Invoke();
                return false;
            }

            int toAdd = Mathf.Min(remaining, data.maxStack);
            items.Add(new InventoryItem(data, toAdd));
            remaining -= toAdd;
        }

        // 收集品 → 通知图鉴
        if (data.itemType == ItemType.Collectible)
            CodexManager.Instance?.RegisterCollectible(data);

        OnInventoryChanged?.Invoke();
        return true;
    }

    /// <summary>移除物品。返回实际移除数量。</summary>
    public int RemoveItem(ItemData data, int amount = 1)
    {
        int removed = 0;
        for (int i = items.Count - 1; i >= 0 && removed < amount; i--)
        {
            if (items[i].data != data) continue;

            int canRemove = Mathf.Min(items[i].amount, amount - removed);
            items[i].amount -= canRemove;
            removed += canRemove;

            if (items[i].IsEmpty)
                items.RemoveAt(i);
        }

        if (removed > 0) OnInventoryChanged?.Invoke();
        return removed;
    }

    /// <summary>使用（消耗型）物品，触发效果后从背包移除。</summary>
    public bool UseItem(ItemData data)
    {
        if (!HasItem(data)) return false;

        ApplySurvivalEffect(data);
        RemoveItem(data, 1);
        return true;
    }

    /// <summary>查询是否拥有指定数量的物品。</summary>
    public bool HasItem(ItemData data, int amount = 1)
    {
        int total = 0;
        foreach (var item in items)
            if (item.data == data) total += item.amount;
        return total >= amount;
    }

    /// <summary>查询物品总数量。</summary>
    public int GetItemCount(ItemData data)
    {
        int total = 0;
        foreach (var item in items)
            if (item.data == data) total += item.amount;
        return total;
    }

    // ──────────────────────────────────────────
    // 内部逻辑
    // ──────────────────────────────────────────

    private void ApplySurvivalEffect(ItemData data)
    {
        // 此处对接玩家状态系统；示例用 Debug.Log 代替
        switch (data.effectType)
        {
            case SurvivalEffectType.RestoreHealth:
                Debug.Log($"[Effect] 恢复生命 {data.effectValue}");
                // PlayerStats.Instance.RestoreHealth(data.effectValue);
                break;
            case SurvivalEffectType.RestoreHunger:
                Debug.Log($"[Effect] 恢复饱腹 {data.effectValue}");
                break;
            case SurvivalEffectType.CureInfection:
                Debug.Log("[Effect] 感染已治愈");
                break;
            case SurvivalEffectType.RepairEquipment:
                Debug.Log($"[Effect] 修复耐久 {data.effectValue}");
                break;
        }
    }
}
