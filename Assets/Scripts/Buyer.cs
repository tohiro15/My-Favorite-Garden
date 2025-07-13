using UnityEngine;

public class Buyer : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Inventory _playerInventory;
    [SerializeField] private BuyerSlot[] _slots;

    private void Awake()
    {
        for(int i = 0; i > _slots.Length; i++)
        {
            _slots[i].Initialization(_itemPrefab, _playerInventory.InventorySlots[i].Item.ItemData, _playerInventory, _playerInventory.InventorySlots[i].Item.ItemCount);
        }
    }
}
