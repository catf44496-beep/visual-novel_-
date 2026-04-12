using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("物品設定")]
    [Tooltip("請將對應的 ItemData (ScriptableObject) 拖曳到這裡")]
    public ItemData itemData;

    [Tooltip("拾取時獲得的數量")]
    public int amount = 1;

    // 當滑鼠點擊這個物件的碰撞體 (Collider) 時會自動觸發此方法
    private void OnMouseDown()
    {
        // 1. 防呆檢查
        if (itemData == null)
        {
            Debug.LogWarning("這個場景物件沒有綁定 ItemData！無法拾取。", gameObject);
            return;
        }

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("場景中找不到 InventoryManager！請確認它已經被建立。");
            return;
        }

        // 2. 嘗試將物品加入背包
        bool isAdded = InventoryManager.Instance.AddItem(itemData, amount);

        // 3. 如果加入成功（背包未滿），則銷毀場景上的物件
        if (isAdded)
        {
            Debug.Log($"成功拾取: {itemData.name} x {amount}");

            // 可以加點特效或音效
            // AudioManager.PlaySound("Pickup");

            Destroy(gameObject);
        }
        else
        {
            Debug.LogWarning("背包已滿，無法拾取！");
            // 這裡可以呼叫 UI 顯示 "背包已滿" 的提示文字
        }
    }
}