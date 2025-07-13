using UnityEngine;

public class BuyerLine : MonoBehaviour
{
    [SerializeField] private BuyerSlot[] _slots;

    public void Initialize(Inventory playerInventory, GameObject itemPrefab)
    {
        for (int i = 0; i > _slots.Length; i++)
        {
            _slots[i].Initialization(playerInventory.InventorySlots[i].Item.ItemData,playerInventory, itemPrefab, playerInventory.InventorySlots[i].Item.ItemCount);
            if(_slots[i].IsEmpty == false)
            {
                _slots[i].gameObject.SetActive(false);
            }
        }
    }
}
