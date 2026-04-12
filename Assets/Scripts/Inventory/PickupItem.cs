using UnityEngine;
using UnityEngine.EventSystems;

public class PickupItem : MonoBehaviour, IPointerClickHandler
{
    [Header("物品设置")]
    [Tooltip("请在 Inspector 里把对应的 ItemData (ScriptableObject) 拖放到这里")]
    public ItemData itemData;

    [Tooltip("拾取时加入的数量")]
    public int amount = 1;

    public void OnPointerClick(PointerEventData eventData)
    {
        TryPickup();
    }

    public void TryPickup()
    {
        // 1. 前置校验
        if (itemData == null)
        {
            Debug.LogWarning("这个可拾取物没有绑定 ItemData，无法拾取。", gameObject);
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("场景中找不到 InventoryManager，请检查场景配置。");
            return;
        }

        // 2. 尝试将物品加入背包
        bool isAdded = InventoryManager.Instance.AddItem(itemData, amount);

        // 3. 加入成功则刷新 UI 并销毁自身
        if (isAdded)
        {
            Debug.Log($"成功拾取: {itemData.itemName} x {amount}");

            InventoryUI ui = Object.FindObjectOfType<InventoryUI>();
            if (ui != null && ui.gameObject.activeSelf)
                ui.RefreshGrid(ItemCategory.Survival);

            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("背包已满，无法拾取。");
        }
    }
}
