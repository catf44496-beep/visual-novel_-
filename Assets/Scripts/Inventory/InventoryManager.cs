using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [Header("Config")]
    public ItemDatabase database;
    public int maxSlots = 30;

    [Header("Economy")]
    public int money = 0;
    public int codexCount = 0;

    public List<InventoryEntry> entries = new List<InventoryEntry>();

    public event System.Action OnInventoryChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (database != null) database.Init();
    }

    public bool AddItem(int id, int count = 1)
    {
        var item = database.Get(id);
        if (item == null) { Debug.LogWarning($"Item {id} not found"); return false; }
        return AddItem(item, count);
    }

    public bool AddItem(ItemData item, int count = 1)
    {
        var existing = entries.FirstOrDefault(e => e.item == item);
        if (existing != null)
        {
            existing.count += count;
        }
        else
        {
            if (entries.Count >= maxSlots) return false;
            entries.Add(new InventoryEntry(item, count));
        }
        if (item.category == ItemCategory.Collectible) codexCount++;
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool RemoveItem(ItemData item, int count = 1)
    {
        var existing = entries.FirstOrDefault(e => e.item == item);
        if (existing == null || existing.count < count) return false;
        existing.count -= count;
        if (existing.count <= 0) entries.Remove(existing);
        OnInventoryChanged?.Invoke();
        return true;
    }

    public List<InventoryEntry> GetByCategory(ItemCategory category)
        => entries.Where(e => e.item.category == category).ToList();

    public int UsedSlots => entries.Count;
}
