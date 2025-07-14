using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class BuyerSlot : MonoBehaviour, IPointerClickHandler
{
    private InventorySlot _sourceSlot;
    private BuyerItem _buyerItem;
    private bool _isEmpty = true;
    private bool _isSelected = false;
    private Color _defaultColor;

    public BuyerItem BuyerItem => _buyerItem;
    public bool IsSeleted => _isSelected;

    public event Action<BuyerSlot> OnSlotClicked;

    private void Awake()
    {
        _buyerItem = GetComponentInChildren<BuyerItem>();
        _defaultColor = GetComponent<Image>().color;
    }

    public void Add(InventorySlot sourceSlot, ItemData data, GameObject buyerItemPrefab, int count)
    {
        _sourceSlot = sourceSlot;
        if (_buyerItem == null && data.CanSell)
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

        GetComponent<Image>().color = _defaultColor;
        _buyerItem = null;
        _isEmpty = true;

        _isSelected = false;

        if (_sourceSlot != null)
        {
            _sourceSlot.Clear();
            _sourceSlot = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isSelected = !_isSelected;
        GetComponent<Image>().color = _isSelected ? Color.green : _defaultColor;
        OnSlotClicked?.Invoke(this);
    }
}
