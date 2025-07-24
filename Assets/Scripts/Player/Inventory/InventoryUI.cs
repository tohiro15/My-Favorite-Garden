using System;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [SerializeField] private GameObject _itemPrefab;

    [Space]
    [Header("Slot Settings")]
    [Space]

    [SerializeField] private InventorySlot[] _inventorySlots;
    [SerializeField] private InventorySlot[] _backpackSlots;
    [SerializeField] private ItemData[] _allItemTypes;
    [Header("Color Settigs")]
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _selectedColor = Color.yellow;
    [SerializeField] private Color _backpackColor;
    [Header("Other Settings")]
    [SerializeField] private float _normalSlotScale = 1f;
    [SerializeField] private float _selectedSlotScale = 1.2f;
    [SerializeField] private float _tweenSlotDuration = 0.15f;
    public InventorySlot[] InventorySlots => _inventorySlots;
    public InventorySlot[] BackpackSlots => _backpackSlots;

    private int _selectedSlot = -1;

    public int SelectedSlotIndex => _selectedSlot;

    public static event Action OnInventoryChanged;

    private void Awake()
    {

        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        for (int i = 0; i < _inventorySlots.Length; i++)
            _inventorySlots[i].Initialization(
                _itemPrefab, i, this, _normalColor, _selectedColor,
                _normalSlotScale, _selectedSlotScale, _tweenSlotDuration, true);

        for (int i = 0; i < _backpackSlots.Length; i++)
            _backpackSlots[i].Initialization(
                _itemPrefab, i, this, _backpackColor, _backpackColor,
                _normalSlotScale, _selectedSlotScale, _tweenSlotDuration, false);

        foreach (var itemData in _allItemTypes)
        {
            string invKey = $"SlotFor_{itemData.name}";
            string bpKey = $"BackpackSlotFor_{itemData.name}";
            string cntKey = $"CountFor_{itemData.name}";

            int savedCount = PlayerPrefs.GetInt(cntKey, 0);
            if (savedCount <= 0)
                continue;

            int invSlot = PlayerPrefs.GetInt(invKey, -1);
            if (invSlot >= 0 && invSlot < _inventorySlots.Length)
            {
                _inventorySlots[invSlot].Add(itemData, savedCount);
                continue;
            }

            int bpSlot = PlayerPrefs.GetInt(bpKey, -1);
            if (bpSlot >= 0 && bpSlot < _backpackSlots.Length)
            {
                _backpackSlots[bpSlot].Add(itemData, savedCount);
            }
        }
    }


    private void Update()
    {
        for (int i = 0; i < _inventorySlots.Length; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectSlot(i);
    }

    public void AddItem(ItemData itemData, int count, bool preferBackpack = false)
    {
        InventorySlot[] firstSearch = preferBackpack ? _backpackSlots : _inventorySlots;
        InventorySlot[] secondSearch = preferBackpack ? _inventorySlots : _backpackSlots;

        // Достак уже в существующий слот
        foreach (var slot in firstSearch.Concat(secondSearch))
        {
            if (!slot.IsEmpty && slot.Item.ItemData == itemData && slot.Item.ItemCount < slot.Item.ItemData.MaxStackSize)
            {
                slot.Item.Add(count);
                SaveSlot(slot);

                OnInventoryChanged?.Invoke();
                return;
            }
        }

        // Ищем первый пустой слот
        foreach (var slot in firstSearch)
        {
            if (slot.IsEmpty)
            {
                slot.Add(itemData, count);
                SaveSlot(slot);

                if (slot.IsSelectable && SelectedSlotIndex == slot.SlotIndex && itemData.PlantPrefab != null)
                {
                    HandController.Instance.Hold(itemData.ItemPrefab, itemData);
                }

                OnInventoryChanged?.Invoke();

                return;
            }

        }
        // Если не нашли в приоритетной зоне — ищем в другой
        foreach (var slot in secondSearch)
        {
            if (slot.IsEmpty)
            {
                slot.Add(itemData, count);
                SaveSlot(slot);
                OnInventoryChanged?.Invoke();
                return;
            }
        }

        // Если вообще некуда добавить — показать сообщение игроку
        Debug.LogWarning($"Нет свободного места ни в инвентаре, ни в рюкзаке для {itemData.name}");
    }

    private void SaveSlot(InventorySlot slot)
    {
        var item = slot.Item;
        string keySlot = slot.IsSelectable
            ? $"SlotFor_{item.ItemData.name}"
            : $"BackpackSlotFor_{item.ItemData.name}";
        string keyCount = $"CountFor_{item.ItemData.name}";

        PlayerPrefs.SetInt(keySlot, slot.SlotIndex);
        PlayerPrefs.SetInt(keyCount, item.ItemCount);
        PlayerPrefs.Save();
    }

    public void SelectSlot(int index)
    {
        if (index == _selectedSlot) return;

        if (_selectedSlot >= 0)
            _inventorySlots[_selectedSlot].Deselect();

        _selectedSlot = index;
        var slot = _inventorySlots[_selectedSlot];

        slot.Select();

        if (!slot.IsEmpty && slot.Item.ItemData.PlantPrefab != null)
        {
            HandController.Instance.Hold(slot.Item.ItemData.ItemPrefab, slot.Item.ItemData);
        }
        else
        {
            HandController.Instance.Clear();
        }
    }


}
