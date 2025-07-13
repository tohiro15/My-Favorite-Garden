using UnityEngine;

public class BuyerLine : MonoBehaviour
{
    [SerializeField] private BuyerSlot[] _slots;

    public void Initialize(InventorySlot[] filledSlots, GameObject buyerItemPrefab)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < filledSlots.Length)
            {
                var invSlot = filledSlots[i];
                _slots[i].Add(invSlot.Item.ItemData, buyerItemPrefab, invSlot.Item.ItemCount);
                _slots[i].gameObject.SetActive(true);
            }
            else
            {
                _slots[i].Clear();
                _slots[i].gameObject.SetActive(false);
            }
        }
    }
}
