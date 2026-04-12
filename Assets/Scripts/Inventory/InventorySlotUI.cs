using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// 背包格子 Prefab 脚本
/// Prefab 层级：
///   SlotRoot (Image — 背景)
///     Icon (Image — 物品图标)
///     CountBadge (TMP_Text — 右下角数量)
///     RarityPip (Image — 左上角稀有度小圆点)
///     SelectedOverlay (Image — 高亮遮罩，默认 disabled)
/// </summary>
[RequireComponent(typeof(Button))]
public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image      slotBackground;
    [SerializeField] private Image      itemIcon;
    [SerializeField] private TMP_Text   countText;
    [SerializeField] private Image      rarityPip;
    [SerializeField] private GameObject selectedOverlay;

    [Header("颜色：空槽 / 有物品 / 选中")]
    [SerializeField] private Color emptyBg    = new Color(0.145f, 0.137f, 0.094f);
    [SerializeField] private Color filledBg   = new Color(0.145f, 0.157f, 0.094f);
    [SerializeField] private Color selectedBg = new Color(0.176f, 0.165f, 0.094f);

    // ── 运行时数据 ────────────────────────────────────────────────
    public InventorySlot Slot { get; private set; }
    private Action<InventorySlotUI> onClick;

    // ── 初始化 ────────────────────────────────────────────────────
    public void Setup(InventorySlot slot, Action<InventorySlotUI> clickCallback)
    {
        Slot    = slot;
        onClick = clickCallback;

        GetComponent<Button>().onClick.AddListener(() => onClick?.Invoke(this));
        Refresh();
    }

    // ── 刷新显示 ──────────────────────────────────────────────────
    public void Refresh()
    {
        bool empty = Slot == null || Slot.IsEmpty;

        if (slotBackground)
            slotBackground.color = empty ? emptyBg : filledBg;

        if (itemIcon)
        {
            itemIcon.enabled = !empty;
            if (!empty) itemIcon.sprite = Slot.itemData.icon;
        }

        if (countText)
        {
            countText.gameObject.SetActive(!empty && Slot.amount > 1);
            if (!empty) countText.text = Slot.amount.ToString();
        }

        if (rarityPip)
        {
            rarityPip.gameObject.SetActive(!empty);
            if (!empty) rarityPip.color = Slot.itemData.GetRarityColor();
        }

        SetHighlight(false);
    }

    // ── 高亮控制 ──────────────────────────────────────────────────
    public void SetHighlight(bool on)
    {
        if (selectedOverlay) selectedOverlay.SetActive(on);
        if (slotBackground && Slot != null && !Slot.IsEmpty)
            slotBackground.color = on ? selectedBg : filledBg;
    }
}
