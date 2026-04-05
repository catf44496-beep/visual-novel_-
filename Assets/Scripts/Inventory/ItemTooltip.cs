// ItemTooltip.cs — 物品悬停提示 & 详情弹窗
// 挂载在 Canvas 下的 TooltipPanel 上，设为 DontDestroyOnLoad 或常驻 Canvas

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip Instance { get; private set; }

    [Header("简要提示（悬停）")]
    [SerializeField] private GameObject hoverPanel;
    [SerializeField] private TextMeshProUGUI hoverItemName;
    [SerializeField] private TextMeshProUGUI hoverDescription;
    [SerializeField] private Image hoverRarityBar;

    [Header("详情面板（右键/收集品）")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private Image detailIcon;
    [SerializeField] private TextMeshProUGUI detailName;
    [SerializeField] private TextMeshProUGUI detailRarity;
    [SerializeField] private TextMeshProUGUI detailDescription;
    [SerializeField] private TextMeshProUGUI detailBackstory;
    [SerializeField] private GameObject backstorySection;   // 仅收集品显示
    [SerializeField] private Button closeButton;

    [Header("偏移量")]
    [SerializeField] private Vector2 hoverOffset = new Vector2(15f, -15f);

    private RectTransform rectTransform;

    // ──────────────────────────────────────────

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        rectTransform = GetComponent<RectTransform>();
        hoverPanel.SetActive(false);
        detailPanel.SetActive(false);

        closeButton?.onClick.AddListener(Hide);
    }

    // ──────────────────────────────────────────
    // 悬停提示
    // ──────────────────────────────────────────

    public void Show(ItemData data, Vector3 worldPos)
    {
        if (data == null) return;

        hoverItemName.text   = data.itemName;
        hoverItemName.color  = data.GetRarityColor();
        hoverDescription.text = data.description;
        if (hoverRarityBar) hoverRarityBar.color = data.GetRarityColor();

        hoverPanel.SetActive(true);
        FollowMouse();
    }

    public void Hide()
    {
        hoverPanel.SetActive(false);
        detailPanel.SetActive(false);
    }

    // ──────────────────────────────────────────
    // 详情面板（右键打开）
    // ──────────────────────────────────────────

    public void ShowFull(ItemData data, Vector3 anchorPos)
    {
        if (data == null) return;

        hoverPanel.SetActive(false);

        detailIcon.sprite    = data.icon;
        detailName.text      = data.itemName;
        detailName.color     = data.GetRarityColor();
        detailRarity.text    = $"【{data.GetRarityName()}】";
        detailRarity.color   = data.GetRarityColor();
        detailDescription.text = data.description;

        bool isCollectible = data.itemType == ItemType.Collectible
                          && CodexManager.Instance != null
                          && CodexManager.Instance.IsDiscovered(data);

        backstorySection.SetActive(isCollectible);
        if (isCollectible) detailBackstory.text = data.backstory;

        detailPanel.SetActive(true);
    }

    // ──────────────────────────────────────────

    private void Update()
    {
        if (hoverPanel.activeSelf) FollowMouse();
    }

    private void FollowMouse()
    {
        Vector2 mousePos = Input.mousePosition;
        hoverPanel.transform.position = new Vector3(
            mousePos.x + hoverOffset.x,
            mousePos.y + hoverOffset.y,
            0f);
    }
}
