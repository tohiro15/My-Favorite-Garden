using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyerItem : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private TextMeshProUGUI _itemNameText;

    private int _itemCount;
    public ItemData ItemData => _itemData;
    public int ItemCount => _itemCount;
    public void Change(ItemData itemData, int count)
    {
        _itemData = itemData;
        _itemCount = count;
        UpdateUI();
    }

    private void UpdateUI()
    {
        bool hasAny = _itemCount > 0;

        _iconImage.sprite = _itemData.Icon;

        if (hasAny)
        {
            _countText.text = $"x{_itemCount}";
            _itemData.ItemName.StringChanged += s => _itemNameText.text = s;
        }
    }
}
