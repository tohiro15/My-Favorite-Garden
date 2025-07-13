using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class BuyerUI : MonoBehaviour
{
    [Header("Item List Settings")]
    [Space]
    [SerializeField] private GameObject _buyerItemPrefab;
    [Space]
    [SerializeField] private InventoryUI _playerInventory;
    [SerializeField] private BuyerLine[] _lines;
    [Space]
    [SerializeField] private TextMeshProUGUI _emptyItemText;

    [Header("Buyer Settings")]
    [Space]

    [SerializeField] private TextMeshProUGUI _totalPriceText;
    [SerializeField] private TextMeshProUGUI _selectedItemPriceText;
    [SerializeField] private Button _sellButton;
    [SerializeField] private Button _sellAllButton;

    private int _totalPrice;
    private int _selectedItemPrice;
    private void Start()
    {
        InitializeItem();

        _emptyItemText?.gameObject.SetActive(false);

        _sellAllButton?.onClick.RemoveAllListeners();
        _sellAllButton?.onClick.AddListener(SellAll);
        _sellButton?.onClick.RemoveAllListeners();

        AddPrice();
    }

    public void InitializeItem()
    {
        var filledSlots = _playerInventory.InventorySlots.Where(s => !s.IsEmpty).ToArray();

        int totalItems = filledSlots.Length;
        int neededLines = Mathf.CeilToInt(totalItems / 3f);

        for (int lineIndex = 0; lineIndex < _lines.Length; lineIndex++)
        {
            if (lineIndex < neededLines)
            {
                _lines[lineIndex].gameObject.SetActive(true);

                int startIdx = lineIndex * 3;
                int countInThisLine = Mathf.Min(3, totalItems - startIdx);

                var slice = new InventorySlot[countInThisLine];
                System.Array.Copy(filledSlots, startIdx, slice, 0, countInThisLine);

                _lines[lineIndex].Initialize(slice, _buyerItemPrefab);
            }
            else
            {
                _lines[lineIndex].gameObject.SetActive(false);
            }
        }

        if (filledSlots.Length <= 0 && _emptyItemText != null) _emptyItemText?.gameObject.SetActive(true);
        else _emptyItemText?.gameObject.SetActive(false);
    }
    public void AddPrice()
    {
        _totalPrice = 0;

        for (int i = 0; i < _playerInventory.InventorySlots.Length; i++)
        {
            var slot = _playerInventory.InventorySlots[i];
            if (slot != null && !slot.IsEmpty && slot.Item != null && slot.Item.ItemData != null)
            {
                _totalPrice += slot.Item.ItemData.ItemPrice;
            }
        }

        UpdateUI();
    }

    public void SellAll()
    {
        for (int i = 0; i < _playerInventory.InventorySlots.Length; i++)
        {
            var slot = _playerInventory.InventorySlots[i];
            if (slot != null && !slot.IsEmpty && slot.Item != null && slot.Item.ItemData != null)
            {
                slot.Clear();
            }
        }


        PlayerStatistic.Instance.AddMoney(_totalPrice);
        _totalPrice = 0;
        InitializeItem();
        UpdateUI();
    }

    public void UpdateUI()
    {
        var nfi = new NumberFormatInfo()
        {
            NumberGroupSeparator = ".",
            NumberDecimalDigits = 0
        };

        _totalPriceText.text = _totalPrice.ToString("N0", nfi);
        _selectedItemPriceText.text = _selectedItemPrice.ToString("N0", nfi);
    }
}
