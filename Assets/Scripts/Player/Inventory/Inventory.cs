using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private InventorySlot[] _inventorySlots;

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
