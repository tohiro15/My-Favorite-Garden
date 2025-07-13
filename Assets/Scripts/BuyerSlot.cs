using UnityEngine;

public class BuyerSlot : MonoBehaviour
{
    private GameObject _itemPrefab;
    private ItemData _itemData;
    private int _count;
    private Inventory _inventory;

    public void Initialization(GameObject itemPrefab, ItemData itemData, Inventory inventory, int count)
    {
        _itemPrefab = itemPrefab;
        _itemData = itemData;
        _count = count;
        _inventory = inventory;
    }
}
