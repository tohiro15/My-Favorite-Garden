using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;
    [Space]
    [SerializeField] private InventorySlot[] _inventorySlots;
    [SerializeField] private ItemData[] _allItemTypes;

    public InventorySlot[] InventorySlots => _inventorySlots;
    private void Awake()
    {
        for(int i = 0;  i < _inventorySlots.Length; i++)
        {
            _inventorySlots[i].Initialization(_itemPrefab,i);
        }
        foreach (var itemData in _allItemTypes)
        {
            string keySlot = $"SlotFor_{itemData.name}";
            string keyCount = $"CountFor_{itemData.name}";

            int savedSlot = PlayerPrefs.GetInt(keySlot, -1);
            int savedCount = PlayerPrefs.GetInt(keyCount, itemData.DefaultCount);

            if (savedSlot >= 0 && savedSlot < _inventorySlots.Length && savedCount > 0)
            {
                _inventorySlots[savedSlot].Add(itemData, savedCount);
            }
        }
    }


    public void AddItem(ItemData itemData, int count)
    {
        // Достакать в уже существующий слот
        //foreach (var slot in _inventorySlots)
        //{
        //    if (!slot.IsEmpty && slot.Item.ItemData == itemData)
        //    {
        //        slot.Item.Add(count);
        //        return;
        //    }
        //}

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
}
