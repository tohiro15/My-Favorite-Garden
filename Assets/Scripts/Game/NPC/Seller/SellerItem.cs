using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellerItem : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _buyText;
    [SerializeField] private TextMeshProUGUI _itemNameText;

    private InventoryUI _inventoryUI;
    private BuyButton _buyButtonScript;

    public void Init(ItemData itemData, InventoryUI inventoryUI)
    {
        _itemData = itemData;
        _inventoryUI = inventoryUI;

        _buyButton?.onClick.RemoveAllListeners();
        _buyButton?.onClick.AddListener(BuyItem);
        _buyButtonScript = _buyButton.GetComponent<BuyButton>();
        _buyButtonScript.Init(itemData, _buyText, _priceText);

        UpdateUI();
    }
    private void UpdateUI()
    {
        var nfi = new NumberFormatInfo()
        {
            NumberGroupSeparator = ".",
            NumberDecimalDigits = 0
        };

        _iconImage.sprite = _itemData.Icon;
        _itemData.ItemName.StringChanged += s => _itemNameText.text = s;
        _priceText.text = $"{_itemData.ItemPrice.ToString("N0", nfi)} $";
    }

    public void BuyItem()
    {
        if (PlayerStatistic.Instance.Money >= _itemData.ItemPrice)
        {
            _inventoryUI.AddItem(_itemData, 1);
            PlayerStatistic.Instance.RemoveMoney(_itemData.ItemPrice);
        }
    }
}
