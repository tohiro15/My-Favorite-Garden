using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySlot[] _inventorySlots;
    [SerializeField] private ItemData[] _allItemTypes;
    private void Awake()
    {
        for(int i = 0;  i < _inventorySlots.Length; i++)
        {
            _inventorySlots[i].Initialization(i);
        }
    }
    private void Start()
    {
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
