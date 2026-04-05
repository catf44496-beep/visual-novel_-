// BlackMarketUI.cs — 黑市 NPC 交易面板
// 挂载在 BlackMarketPanel 根物体上
// 玩家携带收集品时，NPC 自动列出可用交易

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlackMarketUI : MonoBehaviour
{
    [Header("面板引用")]
    [SerializeField] private GameObject blackMarketPanel;
    [SerializeField] private Transform  tradeListParent;   // 交易条目的父物体
    [SerializeField] private GameObject tradeRowPrefab;    // TradeRow 预制体（见下方说明）
    [SerializeField] private TextMeshProUGUI npcDialogueText;
    [SerializeField] private Button closeButton;

    private List<GameObject> spawnedRows = new List<GameObject>();

    // ──────────────────────────────────────────

    private void Start()
    {
        blackMarketPanel.SetActive(false);
        closeButton?.onClick.AddListener(Close);
    }

    // ──────────────────────────────────────────
    // 打开（由 NPC 触发调用）
    // ──────────────────────────────────────────

    /// <summary>
    /// NPC 触发交互时调用此方法，传入 NPC 的开场台词。
    /// 面板自动扫描背包中所有收集品并列出可用交易。
    /// </summary>
    public void Open(string openingLine = "你有什么好东西？")
    {
        npcDialogueText.text = openingLine;
        BuildTradeList();
        blackMarketPanel.SetActive(true);
    }

    public void Close()
    {
        blackMarketPanel.SetActive(false);
        ClearRows();
    }

    // ──────────────────────────────────────────
    // 构建交易列表
    // ──────────────────────────────────────────

    private void BuildTradeList()
    {
        ClearRows();

        foreach (var item in InventoryManager.Instance.Items)
        {
            if (item.data.itemType != ItemType.Collectible) continue;
            if (item.data.tradeOptions == null || item.data.tradeOptions.Count == 0) continue;

            foreach (var recipe in item.data.tradeOptions)
            {
                if (!InventoryManager.Instance.HasItem(recipe.requiredItem, recipe.requiredAmount)) continue;

                var row = Instantiate(tradeRowPrefab, tradeListParent);
                SetupTradeRow(row, recipe);
                spawnedRows.Add(row);
            }
        }

        if (spawnedRows.Count == 0)
            npcDialogueText.text = "你现在没有我想要的东西。";
    }

    // ──────────────────────────────────────────
    // 配置单行交易 UI
    // TradeRow 预制体需有以下组件（名字须完全匹配）：
    //   Image "SourceIcon"、TextMeshPro "SourceLabel"
    //   Image "RewardIcon"、TextMeshPro "RewardLabel"
    //   TextMeshPro "DialogueText"、Button "TradeButton"
    // ──────────────────────────────────────────

    private void SetupTradeRow(GameObject row, TradeRecipe recipe)
    {
        // 上交物
        var srcIcon  = row.transform.Find("SourceIcon")?.GetComponent<Image>();
        var srcLabel = row.transform.Find("SourceLabel")?.GetComponent<TextMeshProUGUI>();
        if (srcIcon  != null) srcIcon.sprite = recipe.requiredItem.icon;
        if (srcLabel != null)
            srcLabel.text = $"{recipe.requiredItem.itemName} x{recipe.requiredAmount}";

        // 获得物
        var rewIcon  = row.transform.Find("RewardIcon")?.GetComponent<Image>();
        var rewLabel = row.transform.Find("RewardLabel")?.GetComponent<TextMeshProUGUI>();
        if (rewIcon  != null) rewIcon.sprite = recipe.rewardItem.icon;
        if (rewLabel != null)
            rewLabel.text = $"{recipe.rewardItem.itemName} x{recipe.rewardAmount}";

        // NPC 台词
        var dialogue = row.transform.Find("DialogueText")?.GetComponent<TextMeshProUGUI>();
        if (dialogue != null) dialogue.text = recipe.npcDialogue;

        // 交易按钮
        var btn = row.transform.Find("TradeButton")?.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => ExecuteTrade(recipe));
        }
    }

    // ──────────────────────────────────────────
    // 执行交易
    // ──────────────────────────────────────────

    private void ExecuteTrade(TradeRecipe recipe)
    {
        if (!InventoryManager.Instance.HasItem(recipe.requiredItem, recipe.requiredAmount))
        {
            npcDialogueText.text = "你没有足够的物品。";
            return;
        }

        InventoryManager.Instance.RemoveItem(recipe.requiredItem, recipe.requiredAmount);
        bool success = InventoryManager.Instance.AddItem(recipe.rewardItem, recipe.rewardAmount);

        if (success)
        {
            npcDialogueText.text = recipe.npcDialogue;
            BuildTradeList(); // 刷新列表
        }
        else
        {
            // 背包满，回滚
            InventoryManager.Instance.AddItem(recipe.requiredItem, recipe.requiredAmount);
            npcDialogueText.text = "你的背包满了，先腾出点地方。";
        }
    }

    private void ClearRows()
    {
        foreach (var row in spawnedRows) Destroy(row);
        spawnedRows.Clear();
    }
}
