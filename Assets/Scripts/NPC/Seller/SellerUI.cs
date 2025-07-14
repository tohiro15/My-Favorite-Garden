using System.Linq;
using UnityEngine;

public class SellerUI : MonoBehaviour
{
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private Transform _sellerContainer;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private ItemData[] _itemSellerData;

    private void Start()
    {
        FillUI();
    }

    public void FillUI()
    {
        foreach (Transform child in _sellerContainer)
            Destroy(child.gameObject);

        var sortedItems = _itemSellerData.OrderBy(item => item.ItemPrice).ToArray();

        for (int i = 0; i < sortedItems.Length; i++)
        {
            var go = Instantiate(_itemPrefab, _sellerContainer);
            var sellerItem = go.GetComponent<SellerItem>();
            sellerItem.Init(sortedItems[i], _inventoryUI);
        }
    }
}
