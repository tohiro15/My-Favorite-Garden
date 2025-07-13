using UnityEngine;

public class BuyerSlot : MonoBehaviour
{
    private BuyerItem _buyerItem;
    private bool _isEmpty = true;
    public bool IsEmpty => _isEmpty;

    private void Awake()
    {
        _buyerItem = GetComponentInChildren<BuyerItem>();
        if (_buyerItem != null) _isEmpty = false;
    }
    public void Add(ItemData data, GameObject buyerItemPrefab, int count)
    {
        if (_buyerItem == null)
        {
            var go = Instantiate(buyerItemPrefab, transform);
            _buyerItem = go.GetComponent<BuyerItem>();
        }
        _buyerItem.Change(data, count);
        _buyerItem.gameObject.SetActive(true);
        _isEmpty = false;
    }


    public void Clear()
    {
        if (_buyerItem != null)
            Destroy(_buyerItem.gameObject);

        _buyerItem = null;
        _isEmpty = true;
    }

}
