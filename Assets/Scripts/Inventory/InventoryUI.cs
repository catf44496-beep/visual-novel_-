using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("格子 Prefab")]
    public InventorySlotUI slotPrefab;

    [Header("右侧详情面板")]
    public Image  detailIcon;
    public Text   detailName;
    public Text   detailRarity;
    public Text   detailDesc;
    public Text   detailEffect;
    public Text   detailWeight;

    [Header("槽位容器")]
    public Transform gridContent;

    [Header("Tab 按钮")]
    public Button tabSurvival;
    public Button tabCodex;
    public Button tabMarket;

    private List<InventorySlotUI> spawnedSlots = new List<InventorySlotUI>();
    private InventorySlotUI currentSelectedSlot;

    private static readonly string[] RarityLabels = { "普通", "非常见", "稀有", "史诗" };

    private void Start()
    {
        Transform dp = transform.Find("Body/DetailPanel");
        if (dp != null)
        {
            Transform iconTr = dp.Find("DetailIcon");
            if (iconTr != null) detailIcon = iconTr.GetComponent<Image>();
            detailName   = FindText(dp, "NameText");
            detailRarity = FindText(dp, "RarityText");
            detailDesc   = FindText(dp, "DescText");
            detailEffect = FindText(dp, "EffectBox/EffectText");
            detailWeight = FindText(dp, "WeightText");
        }

        if (gridContent == null)
        {
            Transform gc = transform.Find("Body/GridContainer/Content");
            if (gc != null) gridContent = gc;
        }

        if (tabSurvival != null) tabSurvival.onClick.AddListener(() => RefreshGrid(ItemCategory.Survival));
        if (tabCodex    != null) tabCodex.onClick.AddListener(()    => RefreshGrid(ItemCategory.Collectible));

        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged += OnInventoryChanged;

        ClearDetail();
        RefreshGrid(ItemCategory.Survival);
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= OnInventoryChanged;
    }

    private void OnInventoryChanged()
    {
        RefreshGrid(ItemCategory.Survival);
    }

    public void Toggle() { gameObject.SetActive(!gameObject.activeSelf); }
    public void Open()   { gameObject.SetActive(true); RefreshGrid(ItemCategory.Survival); }
    public void Close()  { gameObject.SetActive(false); }

    public void RefreshGrid(ItemCategory category)
    {
        if (gridContent == null || slotPrefab == null) return;

        foreach (InventorySlotUI s in spawnedSlots)
            if (s != null) Destroy(s.gameObject);
        spawnedSlots.Clear();
        currentSelectedSlot = null;

        if (InventoryManager.Instance == null) return;

        List<InventoryEntry> allEntries = InventoryManager.Instance.entries;
        foreach (InventoryEntry entry in allEntries)
        {
            if (entry.item != null && entry.item.category != category) continue;

            GameObject go = (GameObject)Object.Instantiate((Object)slotPrefab, gridContent, false);
            InventorySlotUI sui = suiInst;
            sui.Init(this);

            if (entry.item != null)
                sui.SetItem(entry.item, entry.count);
            else
                sui.SetEmpty();

            spawnedSlots.Add(sui);
        }
    }

    public void OnSlotSelected(InventorySlotUI clicked)
    {
        if (currentSelectedSlot != null) currentSelectedSlot.SetSelected(false);
        currentSelectedSlot = clicked;
        currentSelectedSlot.SetSelected(true);
        UpdateDetailPanel(clicked.currentItem, clicked.currentCount);
    }

    private void UpdateDetailPanel(ItemData item, int count)
    {
        if (item == null) { ClearDetail(); return; }

        if (detailIcon != null)
        {
            detailIcon.gameObject.SetActive(item.icon != null);
            if (item.icon != null) detailIcon.sprite = item.icon;
        }
        if (detailName    != null) detailName.text    = item.itemName;
        if (detailRarity  != null)
        {
            int ri = Mathf.Clamp((int)item.rarity, 0, RarityLabels.Length - 1);
            detailRarity.text  = RarityLabels[ri];
            detailRarity.color = item.GetRarityColor();
        }
        if (detailDesc   != null) detailDesc.text    = item.description;
        if (detailEffect != null) detailEffect.text  = item.effect;
        if (detailWeight != null) detailWeight.text  = "重量 " + item.weight + " kg  |  数量 " + count;

        if (item.category == ItemCategory.Collectible && item.isUnlocked)
            if (detailDesc != null && !string.IsNullOrEmpty(item.backgroundStory))
                detailDesc.text = item.description + "\n\n" + item.backgroundStory;
    }

    private void ClearDetail()
    {
        if (detailIcon   != null) detailIcon.gameObject.SetActive(false);
        if (detailName   != null) detailName.text   = "";
        if (detailRarity != null) detailRarity.text = "";
        if (detailDesc   != null) detailDesc.text   = "← 点击物品查看详情";
        if (detailEffect != null) detailEffect.text = "";
        if (detailWeight != null) detailWeight.text = "";
    }

    private static Text FindText(Transform root, string path)
    {
        Transform t = root.Find(path);
        return t != null ? t.GetComponent<Text>() : null;
    }
}
