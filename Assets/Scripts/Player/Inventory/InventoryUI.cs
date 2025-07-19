using UnityEngine;

public class InventoryUI : MonoBehaviour
{
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

    private void Awake()
    {
        for (int i = 0; i < _inventorySlots.Length; i++)
            _inventorySlots[i].Initialization(_itemPrefab, i, this, _normalColor, _selectedColor, _normalSlotScale, _selectedSlotScale, _tweenSlotDuration, true);

        for (int i = 0; i < _backpackSlots.Length; i++)
            _backpackSlots[i].Initialization(_itemPrefab, i, this, _backpackColor, _backpackColor, _normalSlotScale, _selectedSlotScale, _tweenSlotDuration, false);

        foreach (var itemData in _allItemTypes)
        {
            string keySlot = $"SlotFor_{itemData.name}";
            string keyCount = $"CountFor_{itemData.name}";
            int savedSlot = PlayerPrefs.GetInt(keySlot, -1);
            int savedCount = PlayerPrefs.GetInt(keyCount, itemData.DefaultCount);

            if (savedSlot >= 0 && savedSlot < _inventorySlots.Length && savedCount > 0)
                _inventorySlots[savedSlot].Add(itemData, savedCount);
            else if (savedSlot >= 0 && savedSlot < _backpackSlots.Length && savedCount > 0)
                _backpackSlots[savedSlot].Add(itemData, savedCount);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _inventorySlots.Length; i++)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                SelectSlot(i);
    }

    public void AddItem(ItemData itemData, int count)
    {
        // ƒостакать в уже существующий слот
        foreach (var slot in _inventorySlots)
        {
            if (!slot.IsEmpty && slot.Item.ItemData == itemData)
            {
                slot.Item.Add(count);
                return;
            }
        }

        // ѕереместить в незан€тый слот
        foreach (var slot in _inventorySlots)
        {
            if (slot.IsEmpty)
            {
                slot.Add(itemData, count);
                return;
            }
        }
    }

    public void SelectSlot(int index)
    {
        if (index == _selectedSlot) return;

        if (_selectedSlot >= 0)
            _inventorySlots[_selectedSlot].Deselect();
        _selectedSlot = index;
        _inventorySlots[_selectedSlot].Select();

        // TODO: здесь можно инициировать по€вление предмета в руке
        // var selectedItem = _inventorySlots[_selectedSlot].Item;
        // HandController.Instance.Hold(selectedItem);
    }

}
