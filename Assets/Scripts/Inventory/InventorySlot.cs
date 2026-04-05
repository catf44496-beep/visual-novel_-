// InventorySlot.cs — 背包格子 UI 组件
// 挂载在每个 Slot 预制体根物体上

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("UI 引用")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image rarityBorder;        // 外框颜色显示稀有度
    [SerializeField] private GameObject emptyOverlay;   // 空格子遮罩

    private InventoryItem boundItem;

    // ──────────────────────────────────────────
    // 数据绑定
    // ──────────────────────────────────────────

    public void Bind(InventoryItem item)
    {
        boundItem = item;

        if (item == null || item.data == null)
        {
            SetEmpty();
            return;
        }

        emptyOverlay.SetActive(false);
        iconImage.sprite  = item.data.icon;
        iconImage.enabled = true;

        // 数量：不可叠加或只有1个时隐藏
        bool showCount = item.data.isStackable && item.amount > 1;
        countText.gameObject.SetActive(showCount);
        if (showCount) countText.text = item.amount.ToString();

        // 稀有度外框
        if (rarityBorder != null)
            rarityBorder.color = item.data.GetRarityColor();
    }

    public void SetEmpty()
    {
        boundItem = null;
        iconImage.sprite  = null;
        iconImage.enabled = false;
        countText.gameObject.SetActive(false);
        if (rarityBorder != null) rarityBorder.color = new Color(0.3f, 0.3f, 0.3f);
        emptyOverlay.SetActive(true);
    }

    // ──────────────────────────────────────────
    // 交互事件
    // ──────────────────────────────────────────

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (boundItem != null)
            ItemTooltip.Instance?.Show(boundItem.data, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltip.Instance?.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (boundItem == null) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 左键：使用（仅生存物资）
            if (boundItem.data.itemType == ItemType.Survival)
                InventoryManager.Instance.UseItem(boundItem.data);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 右键：查看详情 / 图鉴故事
            ItemTooltip.Instance?.ShowFull(boundItem.data, transform.position);
        }
    }
}
