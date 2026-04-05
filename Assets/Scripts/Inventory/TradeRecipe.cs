// TradeRecipe.cs — 黑市交易配方数据（序列化结构体，嵌入 ItemData）

using System;
using UnityEngine;

[Serializable]
public class TradeRecipe
{
    [Header("交易条件")]
    public ItemData requiredItem;       // 需要上交的收集品
    public int requiredAmount = 1;      // 需要数量

    [Header("交易奖励")]
    public ItemData rewardItem;         // 换得的生存物资
    public int rewardAmount = 1;        // 奖励数量

    [Header("NPC 台词")]
    [TextArea(1, 3)]
    public string npcDialogue = "好东西，换给你。";
}
