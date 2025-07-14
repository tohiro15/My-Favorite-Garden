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

    private void Start()
    {
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
        _itemNameText.text = _itemData.ItemName.ToString();
        _priceText.text = $"{_itemData.ItemPrice.ToString("N0", nfi)} $";
    }
}
