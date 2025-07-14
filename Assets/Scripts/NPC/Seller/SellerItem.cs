using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellerItem : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _buyText;
    [SerializeField] private TextMeshProUGUI _itemNameText;

    public void Init(ItemData itemData)
    {
        _itemData = itemData;

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
}
