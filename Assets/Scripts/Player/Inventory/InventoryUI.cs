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
            string invKey = $"SlotFor_{itemData.name}";
            string bpKey = $"BackpackSlotFor_{itemData.name}";
            string cntKey = $"CountFor_{itemData.name}";

            int savedCount = PlayerPrefs.GetInt(cntKey, itemData.DefaultCount);

            int invSlot = PlayerPrefs.GetInt(invKey, -1);
            if (invSlot >= 0 && invSlot < _inventorySlots.Length && savedCount > 0)
            {
                _inventorySlots[invSlot].Add(itemData, savedCount);
                continue;
            }

            int bpSlot = PlayerPrefs.GetInt(bpKey, -1);
            if (bpSlot >= 0 && bpSlot < _backpackSlots.Length && savedCount > 0)
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

    public void AddItem(ItemData itemData, int count)
    {
        // Достакать в уже существующий слот
        foreach (var slot in _inventorySlots)
        {
            if (!slot.IsEmpty && slot.Item.ItemData == itemData)
            {
                slot.Item.Add(count);
                return;
            }
        }

        // Переместить в незанятый слот
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
        var slot = _inventorySlots[_selectedSlot];

        slot.Select();

        if (!slot.IsEmpty)
        {
            var prefab = slot.Item.ItemData.ItemPrefab;
            HandController.Instance.Hold(prefab);
        }
        else
        {
            HandController.Instance.Clear();
        }
    }


}
