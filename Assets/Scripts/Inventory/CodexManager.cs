// CodexManager.cs — 图鉴系统：记录并展示已收集的时代老物件
// 挂载在同一个 GameManager 物体上

using System;
using System.Collections.Generic;
using UnityEngine;

public class CodexManager : MonoBehaviour
{
    public static CodexManager Instance { get; private set; }

    // 所有收集品定义（在 Inspector 中填入）
    [Header("全部收集品列表")]
    [SerializeField] private List<ItemData> allCollectibles = new List<ItemData>();

    // 已发现的收集品 ID 集合（存档时序列化此列表）
    private HashSet<int> discoveredIDs = new HashSet<int>();

    public event Action OnCodexUpdated;

    // ──────────────────────────────────────────

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    // ──────────────────────────────────────────
    // 公开 API
    // ──────────────────────────────────────────

    /// <summary>InventoryManager 添加收集品时调用。</summary>
    public void RegisterCollectible(ItemData data)
    {
        if (data.itemType != ItemType.Collectible) return;
        if (discoveredIDs.Add(data.itemID))
        {
            Debug.Log($"[Codex] 新收集品解锁：{data.itemName}");
            OnCodexUpdated?.Invoke();
        }
    }

    public bool IsDiscovered(ItemData data) => discoveredIDs.Contains(data.itemID);

    public IReadOnlyList<ItemData> AllCollectibles => allCollectibles;

    public int DiscoveredCount => discoveredIDs.Count;
    public int TotalCount      => allCollectibles.Count;

    // ──────────────────────────────────────────
    // 存档支持（PlayerPrefs 简易实现）
    // ──────────────────────────────────────────

    private const string SaveKey = "CodexDiscovered";

    public void Save()
    {
        PlayerPrefs.SetString(SaveKey, string.Join(",", discoveredIDs));
        PlayerPrefs.Save();
    }

    public void Load()
    {
        discoveredIDs.Clear();
        string raw = PlayerPrefs.GetString(SaveKey, "");
        if (string.IsNullOrEmpty(raw)) return;
        foreach (var s in raw.Split(','))
            if (int.TryParse(s, out int id)) discoveredIDs.Add(id);
    }
}
