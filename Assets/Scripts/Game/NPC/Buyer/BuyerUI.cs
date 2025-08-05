using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System;
using UnityEngine.Localization;

public class BuyerUI : MonoBehaviour
{
    [Header("Item List Settings")]
    [SerializeField] private GameObject _buyerItemPrefab;
    [SerializeField] private InventoryUI _playerInventory;
    [SerializeField] private BuyerLine[] _lines;
    [SerializeField] private TextMeshProUGUI _emptyItemText;

    [Header("Buyer Settings")]
    [SerializeField] private LocalizedString _seasonLocalizedString;
    [SerializeField] private TextMeshProUGUI _currentSeasonText;
    [SerializeField] private TextMeshProUGUI _totalPriceText;
    [SerializeField] private TextMeshProUGUI _selectedItemPriceText;
    [Space]
    [SerializeField] private Button _sellButton;
    [SerializeField] private Button _sellAllButton;

    private int _totalPrice;
    private int _selectedItemPrice;
    public event Action<int> OnSelectedItemPriceChanged;

    private void Awake()
    {
        InventoryUI.OnInventoryChanged += InitializeItem;
    }

    private void OnDestroy()
    {
        InventoryUI.OnInventoryChanged -= InitializeItem;
    }

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
                if (slot.IsSeleted && slot.BuyerItem.ItemData.CanSell)
                    _selectedItemPrice += slot.BuyerItem.ItemData.ItemPrice * slot.BuyerItem.ItemCount;
    }

    private void RecalculateAllPrice()
    {
        _totalPrice = 0;
        foreach (var slot in _playerInventory?.InventorySlots)
            if (slot != null && !slot.IsEmpty && slot.Item.ItemData.CanSell)
                _totalPrice += slot.Item.ItemData.ItemPrice * slot.Item.ItemCount;

        foreach (var slot in _playerInventory?.BackpackSlots)
            if (slot != null && !slot.IsEmpty && slot.Item.ItemData.CanSell)
                _totalPrice += slot.Item.ItemData.ItemPrice * slot.Item.ItemCount;
    }

    public void InitializeItem()
    {
        var sellableSlots = _playerInventory?.InventorySlots
            .Concat(_playerInventory?.BackpackSlots)
            .Where(s => s != null && !s.IsEmpty && s.Item.ItemData.CanSell)
            .ToArray();

        int totalItems = sellableSlots.Length;
        int neededLines = Mathf.CeilToInt(totalItems / 3f);

        for (int i = 0; i < _lines.Length; i++)
        {
            if (i < neededLines)
            {
                _lines[i].gameObject.SetActive(true);

                int startIdx = i * 3;
                int countInThisLine = Mathf.Min(3, totalItems - startIdx);
                var slice = new InventorySlot[countInThisLine];
                Array.Copy(sellableSlots, startIdx, slice, 0, countInThisLine);

                _lines[i].Initialize(slice, _buyerItemPrefab);
            }
            else
            {
                _lines[i].gameObject.SetActive(false);
            }
        }

        bool noItems = totalItems == 0;
        _emptyItemText.gameObject.SetActive(noItems);
        _sellAllButton.gameObject.SetActive(!noItems);
        _sellButton.gameObject.SetActive(!noItems);

        RecalculateAllPrice();
        UpdateUI();
    }


    public void SellAll()
    {
        foreach (var invSlot in _playerInventory.InventorySlots)
            if (invSlot != null && !invSlot.IsEmpty && invSlot.Item.ItemData.CanSell)
                invSlot.Clear();

        PlayerStatistic.Instance.AddMoney(_totalPrice);
        _totalPrice = 0;

        InitializeItem();
        UpdateUI();
    }

    private void SellSelectedItem()
    {
        var selectedSlots = _lines
            .SelectMany(line => line.Slots)
            .Where(s => s.IsSeleted && s.BuyerItem.ItemData.CanSell)
            .ToList();

        foreach (var buyerSlot in selectedSlots)
        {
            int count = buyerSlot.BuyerItem.ItemCount;
            int price = buyerSlot.BuyerItem.ItemData.ItemPrice * count;
            PlayerStatistic.Instance.AddMoney(price);

            if (buyerSlot.SourceSlot != null)
                buyerSlot.SourceSlot.Clear();
        }

        _selectedItemPrice = 0;
        RecalculateAllPrice();

        InitializeItem();
        UpdateUI();
    }


    public void UpdateUI()
    {
        var nfi = new NumberFormatInfo
        {
            NumberGroupSeparator = ".",
            NumberDecimalDigits = 0
        };

        _seasonLocalizedString.TableEntryReference = SeasonManager.Instance.GetCurrentSeasonLocalizationKey();
        _seasonLocalizedString.StringChanged += UpdateSeasonText;

        _totalPriceText.text = _totalPrice.ToString("N0", nfi);
        _selectedItemPriceText.text = _selectedItemPrice.ToString("N0", nfi);

        bool hasSelected = _lines.SelectMany(line => line.Slots).Any(s => s.IsSeleted);
        _sellButton.gameObject.SetActive(hasSelected);
    }

    private void UpdateSeasonText(string localizedValue)
    {
        _currentSeasonText.text = localizedValue;
        _seasonLocalizedString.StringChanged -= UpdateSeasonText;
    }

}
