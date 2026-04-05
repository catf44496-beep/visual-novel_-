// InventoryUI.cs — 背包面板控制器（网格布局）
// 挂载在 InventoryPanel 根物体上
// 层级：Canvas > InventoryPanel(此脚本) > Viewport > SlotGrid

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("面板引用")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform  slotGrid;           // GridLayoutGroup 所在物体
    [SerializeField] private GameObject slotPrefab;         // InventorySlot 预制体
    [SerializeField] private TextMeshProUGUI slotCountText; // "12 / 24"
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;

    [Header("标签页按钮")]
    [SerializeField] private Button tabSurvival;
    [SerializeField] private Button tabCollectible;
    [SerializeField] private Button tabAll;

    private List<InventorySlot> slotViews = new List<InventorySlot>();
    private ItemType? currentFilter = null; // null = 全部

    // ──────────────────────────────────────────

    private void Start()
    {
        // 预生成格子
        for (int i = 0; i < InventoryManager.Instance.SlotCount; i++)
        {
            var go   = Instantiate(slotPrefab, slotGrid);
            var slot = go.GetComponent<InventorySlot>();
            slotViews.Add(slot);
        }

        // 订阅数据变化
        InventoryManager.Instance.OnInventoryChanged += RefreshUI;

        // 标签页
        tabSurvival?.onClick.AddListener(() => SetFilter(ItemType.Survival));
        tabCollectible?.onClick.AddListener(() => SetFilter(ItemType.Collectible));
        tabAll?.onClick.AddListener(() => SetFilter(null));

        inventoryPanel.SetActive(false);
        RefreshUI();
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= RefreshUI;
    }

    // ──────────────────────────────────────────
    // 开关 & 刷新
    // ──────────────────────────────────────────

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            TogglePanel();
    }

    public void TogglePanel()
    {
        bool nowOpen = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(nowOpen);
        if (nowOpen) RefreshUI();
    }

    private void SetFilter(ItemType? filter)
    {
        currentFilter = filter;
        RefreshUI();
    }

    // ──────────────────────────────────────────
    // 核心刷新逻辑
    // ──────────────────────────────────────────

    public void RefreshUI()
    {
        var allItems = InventoryManager.Instance.Items;

        // 筛选
        var filtered = new List<InventoryItem>();
        foreach (var item in allItems)
        {
            if (currentFilter == null || item.data.itemType == currentFilter)
                filtered.Add(item);
        }

        // 填充格子
        for (int i = 0; i < slotViews.Count; i++)
        {
            if (i < filtered.Count)
                slotViews[i].Bind(filtered[i]);
            else
                slotViews[i].SetEmpty();
        }

        // 更新数量文字
        if (slotCountText != null)
            slotCountText.text = $"{InventoryManager.Instance.UsedSlots} / {InventoryManager.Instance.SlotCount}";
    }
}
