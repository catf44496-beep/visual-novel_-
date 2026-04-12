using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 背包 UI 控制�?/// 层级结构参考：
///   InventoryPanel
///     TabBar
///       BtnSurvival / BtnCollectible / BtnMarket
///     GridContainer   �?ScrollView 内放 GridLayoutGroup
///     DetailPanel
///       DetailIcon / DetailName / DetailRarity / DetailDesc / DetailEffect / DetailWeight
///     StatusBar
///       TxtWeight / TxtCodex / TxtReputation
/// </summary>
public class InventoryUI : MonoBehaviour
{
    // ── Inspector 引用 ────────────────────────────────────────────
    [Header("面板根节�?)]
    [SerializeField] private GameObject inventoryPanel;

    [Header("标签页按�?)]
    [SerializeField] private Button btnSurvival;
    [SerializeField] private Button btnCollectible;
    [SerializeField] private Button btnMarket;

    [Header("Grid 容器（带 GridLayoutGroup�?)]
    [SerializeField] private Transform gridContainer;
    [SerializeField] private GameObject slotPrefab;      // 每个格子�?Prefab

    [Header("详情面板")]
    [SerializeField] private Image  detailIcon;
    [SerializeField] private TMP_Text detailName;
    [SerializeField] private TMP_Text detailRarity;
    [SerializeField] private TMP_Text detailDesc;
    [SerializeField] private TMP_Text detailEffect;
    [SerializeField] private TMP_Text detailWeight;
    [SerializeField] private TMP_Text detailStory;       // 仅收集品使用
    [SerializeField] private GameObject storyPanel;

    [Header("状态栏")]
    [SerializeField] private TMP_Text txtWeight;
    [SerializeField] private TMP_Text txtSlots;
    [SerializeField] private TMP_Text txtCodex;

    [Header("当前 Tab 选中�?/ 默认�?)]
    [SerializeField] private Color tabActiveColor   = new Color(0.85f, 0.78f, 0.44f);
    [SerializeField] private Color tabDefaultColor  = new Color(0.35f, 0.33f, 0.27f);

    // ── 运行�?────────────────────────────────────────────────────
    private enum TabMode { Survival, Collectible, Market }
    private TabMode currentTab = TabMode.Survival;
    private List<InventoryEntryUI> spawnedSlots = new();
    private InventoryEntryUI selectedSlot;

    // ── 生命周期 ─────────────────────────────────────────────────
    private void Start()
    {
        InventoryManager.Instance.OnInventoryChanged += RefreshGrid;

        btnSurvival  .onClick.AddListener(() => SwitchTab(TabMode.Survival));
        btnCollectible.onClick.AddListener(() => SwitchTab(TabMode.Collectible));
        btnMarket    .onClick.AddListener(() => SwitchTab(TabMode.Market));

        SwitchTab(TabMode.Survival);
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= RefreshGrid;
    }

    // ── 公共接口 ─────────────────────────────────────────────────
    public void TogglePanel()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (inventoryPanel.activeSelf) RefreshGrid();
    }

    // ── Tab 切换 ─────────────────────────────────────────────────
    private void SwitchTab(TabMode tab)
    {
        currentTab   = tab;
        selectedSlot = null;
        ClearDetail();
        UpdateTabVisuals();
        RefreshGrid();
    }

    private void UpdateTabVisuals()
    {
        SetTabColor(btnSurvival,    currentTab == TabMode.Survival);
        SetTabColor(btnCollectible, currentTab == TabMode.Collectible);
        SetTabColor(btnMarket,      currentTab == TabMode.Market);
    }

    private void SetTabColor(Button btn, bool active)
    {
        var txt = btn.GetComponentInChildren<TMP_Text>();
        if (txt) txt.color = active ? tabActiveColor : tabDefaultColor;
    }

    // ── Grid 刷新 ─────────────────────────────────────────────────
    private void RefreshGrid()
    {
        // 清除旧格�?        foreach (var s in spawnedSlots) Destroy(s.gameObject);
        spawnedSlots.Clear();

        var slots = InventoryManager.Instance.GetAllSlots();

        foreach (var slot in slots)
        {
            // 根据当前 Tab 决定是否跳过
            if (!ShouldShowSlot(slot)) continue;

            var go  = Instantiate(slotPrefab, gridContainer);
            var sui = go.GetComponent<InventoryEntryUI>();
            sui.Setup(slot, OnSlotClicked);
            spawnedSlots.Add(sui);
        }

        RefreshStatusBar();
    }

    private bool ShouldShowSlot(InventoryEntry slot)
    {
        if (slot.item == null) return currentTab == TabMode.Survival; // 空槽只在生存页显�?        return currentTab switch
        {
            TabMode.Survival    => slot.item.category == ItemCategory.Survival,
            TabMode.Collectible => slot.item.category == ItemCategory.Collectible,
            TabMode.Market      => slot.item.category == ItemCategory.Collectible
                                   && slot.item.canExchange,
            _ => false
        };
    }

    // ── 点击格子 ─────────────────────────────────────────────────
    private void OnSlotClicked(InventoryEntryUI sui)
    {
        if (selectedSlot != null) selectedSlot.SetHighlight(false);
        selectedSlot = sui;
        selectedSlot.SetHighlight(true);
        ShowDetail(sui.Slot);
    }

    // ── 详情面板 ─────────────────────────────────────────────────
    private void ShowDetail(InventoryEntry slot)
    {
        if (slot.item == null) { ClearDetail(); return; }

        var item = slot.item;
        if (detailIcon)   detailIcon.sprite = item.icon;
        if (detailName)   detailName.text   = item.itemName;
        if (detailRarity)
        {
            detailRarity.text  = item.GetRarityLabel();
            detailRarity.color = item.GetRarityColor();
        }
        if (detailDesc)   detailDesc.text   = item.description;
        if (detailEffect) detailEffect.text  = item.effectDescription;
        if (detailWeight) detailWeight.text  = $"重量 {item.weight} kg  |  数量 {slot.count}";

        bool isCollectible = item.category == ItemCategory.Collectible;
        if (storyPanel) storyPanel.SetActive(isCollectible && item.isUnlocked);
        if (detailStory && isCollectible && item.isUnlocked)
            detailStory.text = item.backgroundStory;
    }

    private void ClearDetail()
    {
        if (detailIcon)   detailIcon.sprite = null;
        if (detailName)   detailName.text   = "";
        if (detailRarity) detailRarity.text = "";
        if (detailDesc)   detailDesc.text   = "�?选择物品查看详情";
        if (detailEffect) detailEffect.text = "";
        if (detailWeight) detailWeight.text = "";
        if (storyPanel)   storyPanel.SetActive(false);
    }

    // ── 状态栏 ────────────────────────────────────────────────────
    private void RefreshStatusBar()
    {
        var mgr = InventoryManager.Instance;
        if (txtWeight) txtWeight.text = $"总重�?{mgr.GetTotalWeight():F1} kg";
        if (txtSlots)  txtSlots.text  = $"{mgr.UsedSlotCount} / {mgr.MaxSlots} �?;

        // 图鉴统计
        var slots    = mgr.GetAllSlots();
        int total    = slots.Count(s => !s.item == null && s.item.category == ItemCategory.Collectible);
        int unlocked = slots.Count(s => !s.item == null && s.item.category == ItemCategory.Collectible
                                        && s.item.isUnlocked);
        if (txtCodex) txtCodex.text = $"图鉴 {unlocked}/{total}";
    }
}
