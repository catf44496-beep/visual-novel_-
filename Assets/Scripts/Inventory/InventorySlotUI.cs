using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI 组件")]
    public Image      iconBg;
    public Image      iconImage;
    public Text       iconText;
    public Text       countText;
    public Image      rarityPip;
    public GameObject selectedOverlay;

    [Header("当前物品数据")]
    public ItemData currentItem;
    public int      currentCount;
    public bool     isEmpty = true;

    private InventoryUI mainUI;

    private static readonly Color[] RarityColors =
    {
        new Color(0.70f, 0.70f, 0.70f),
        new Color(0.30f, 0.80f, 0.30f),
        new Color(0.30f, 0.50f, 1.00f),
        new Color(0.70f, 0.30f, 1.00f),
    };

    private void Awake()
    {
        iconBg          = transform.Find("IconBG")?.GetComponent<Image>();
        iconImage       = transform.Find("IconBG/IconImage")?.GetComponent<Image>();
        iconText        = transform.Find("IconBG/IconText")?.GetComponent<Text>();
        countText       = transform.Find("IconBG/CountText")?.GetComponent<Text>();
        if (countText == null)
            countText   = transform.Find("CountText")?.GetComponent<Text>();
        rarityPip       = transform.Find("RarityPip")?.GetComponent<Image>();
        selectedOverlay = transform.Find("SelectedOverlay")?.gameObject;

        if (selectedOverlay != null)
        {
            Image ov = selectedOverlay.GetComponent<Image>();
            if (ov != null) ov.raycastTarget = false;
        }
    }

    public void Init(InventoryUI uiManager)
    {
        mainUI = uiManager;
    }

    public void SetItem(ItemData item, int count)
    {
        currentItem  = item;
        currentCount = count;
        isEmpty      = false;

        if (iconBg != null) iconBg.gameObject.SetActive(true);

        if (countText != null)
        {
            countText.gameObject.SetActive(count > 1);
            countText.text = count.ToString();
        }

        if (rarityPip != null)
        {
            rarityPip.gameObject.SetActive(true);
            int ri = Mathf.Clamp((int)item.rarity, 0, RarityColors.Length - 1);
            rarityPip.color = RarityColors[ri];
        }

        if (iconImage != null && item.icon != null)
        {
            iconImage.gameObject.SetActive(true);
            iconImage.sprite = item.icon;
            if (iconText != null) iconText.gameObject.SetActive(false);
        }
        else if (iconText != null)
        {
            iconText.gameObject.SetActive(true);
            iconText.text = item.itemName.Length >= 2 ? item.itemName.Substring(0, 2) : item.itemName;
        }
    }

    public void SetEmpty()
    {
        currentItem  = null;
        currentCount = 0;
        isEmpty      = true;

        if (iconBg    != null) iconBg.gameObject.SetActive(false);
        if (countText != null) countText.gameObject.SetActive(false);
        if (rarityPip != null) rarityPip.gameObject.SetActive(false);
        if (iconImage != null) iconImage.gameObject.SetActive(false);
        if (iconText  != null) iconText.gameObject.SetActive(false);

        Image bg = GetComponent<Image>();
        if (bg != null) bg.color = new Color(0.14f, 0.13f, 0.09f, 0.5f);
        SetSelected(false);
    }

    public void SetSelected(bool isSelected)
    {
        if (selectedOverlay != null) selectedOverlay.SetActive(isSelected);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isEmpty || currentItem == null) return;
        if (mainUI != null) mainUI.OnSlotSelected(this);
    }
}