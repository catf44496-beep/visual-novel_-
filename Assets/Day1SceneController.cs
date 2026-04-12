using UnityEngine;

public class Day1SceneController : MonoBehaviour
{
    [Header("UI")]
    public InventoryUI inventoryUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
            ToggleInventory();
    }

    public void ToggleInventory()
    {
        if (inventoryUI != null)
            inventoryUI.Toggle();
    }
}
