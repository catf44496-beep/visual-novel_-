using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items = new List<ItemData>();

    private Dictionary<int, ItemData> _lookup;

    public void Init()
    {
        _lookup = new Dictionary<int, ItemData>();
        foreach (var item in items)
            if (item != null) _lookup[item.id] = item;
    }

    public ItemData Get(int id)
    {
        if (_lookup == null) Init();
        _lookup.TryGetValue(id, out var result);
        return result;
    }
}
