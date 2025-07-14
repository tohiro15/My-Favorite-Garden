using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using UnityEngine.EventSystems;
using System;

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
    [Space]
    [SerializeField] private Button _sellButton;
    [SerializeField] private Button _sellAllButton;

    private int _totalPrice;
    private int _selectedItemPrice;

    public event Action<int> OnSelectedItemPriceChanged;
    private void Start()
    {
        InitializeItem();
        SubscribeSlots();

        _sellAllButton?.onClick.RemoveAllListeners();
        _sellAllButton?.onClick.AddListener(SellAll);
        _sellButton?.onClick.RemoveAllListeners();
        _sellButton?.onClick.AddListener(SellSelectedItem);

        RecalculateAllPrice();
        UpdateUI();
    }

    private void SubscribeSlots()
    {
        foreach (var line in _lines)
            foreach (var slot in line.Slots)
                slot.OnSlotClicked += HandleSlotClicked;
    }
    private void HandleSlotClicked(BuyerSlot slot)
    {
        RecalculateSelectedPrice();
        OnSelectedItemPriceChanged?.Invoke(_selectedItemPrice);
        UpdateUI();
    }
    private void RecalculateSelectedPrice()
    {
        _selectedItemPrice = 0;
        foreach (var line in _lines)
            foreach (var slot in line.Slots)
                if (slot.IsSeleted && slot.BuyerItem.ItemData.CanSell && slot.BuyerItem != null)
                    _selectedItemPrice += slot.BuyerItem.ItemData.ItemPrice * slot.BuyerItem.ItemCount;
    }

    private void RecalculateAllPrice()
    {
        _totalPrice = 0;

        for (int i = 0; i < _playerInventory.InventorySlots.Length; i++)
        {
            var slot = _playerInventory.InventorySlots[i];
            if (slot != null && !slot.IsEmpty && slot.Item != null && slot.Item.ItemData.CanSell && slot.Item.ItemData != null)
            {
                _totalPrice += slot.Item.ItemData.ItemPrice * slot.Item.ItemCount;
            }
        }
    }
    public void InitializeItem()
    {
        var sellableSlots = _playerInventory.InventorySlots
            .Where(s => !s.IsEmpty && s.Item.ItemData.CanSell)
            .ToArray();

        int totalItems = sellableSlots.Length;
        int neededLines = Mathf.CeilToInt(totalItems / 3f);

        for (int lineIndex = 0; lineIndex < _lines.Length; lineIndex++)
        {
            if (lineIndex < neededLines)
            {
                _lines[lineIndex].gameObject.SetActive(true);

                int startIdx = lineIndex * 3;
                int countInThisLine = Mathf.Min(3, totalItems - startIdx);
                var slice = new InventorySlot[countInThisLine];
                Array.Copy(sellableSlots, startIdx, slice, 0, countInThisLine);

                _lines[lineIndex].Initialize(slice, _buyerItemPrefab);
            }
            else
            {
                _lines[lineIndex].gameObject.SetActive(false);
            }
        }

        if (totalItems <= 0 && _emptyItemText != null)
        {
            _emptyItemText?.gameObject.SetActive(true);

            if (_sellAllButton != null && _sellButton != null)
            {
                _sellAllButton?.gameObject.SetActive(false);
                _sellButton?.gameObject.SetActive(false);
            }
        }
        else if (_emptyItemText != null)
        {
            _emptyItemText?.gameObject.SetActive(false);

            if (_sellAllButton != null && _sellButton != null)
            {
                _sellAllButton?.gameObject.SetActive(true);
                _sellButton?.gameObject.SetActive(true) ;
            }
        }
    }
    public void SellAll()
    {
        for (int i = 0; i < _playerInventory.InventorySlots.Length; i++)
        {
            var slot = _playerInventory.InventorySlots[i];
            if (slot != null && !slot.IsEmpty && slot.Item.ItemData.CanSell && slot.Item != null && slot.Item.ItemData != null)
            {
                slot.Clear();
            }
        }


        PlayerStatistic.Instance.AddMoney(_totalPrice);
        _totalPrice = 0;
        InitializeItem();
        UpdateUI();
    }

    private void SellSelectedItem()
    {
        var selectedSlots = _lines.SelectMany(line => line.Slots).Where(s => s.IsSeleted && s.BuyerItem.ItemData.CanSell && s.BuyerItem != null).ToList();

        foreach (var slot in selectedSlots)
        {
            int price = slot.BuyerItem.ItemData.ItemPrice * slot.BuyerItem.ItemCount;

            PlayerStatistic.Instance.AddMoney(price);
            slot.Clear();
            slot.gameObject.SetActive(false);
        }

        _selectedItemPrice = 0;
        RecalculateAllPrice();
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
