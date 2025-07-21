// BuyerSlot.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class BuyerSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private InventorySlot _sourceSlot;
    public InventorySlot SourceSlot => _sourceSlot;

    private BuyerItem _buyerItem;
    private bool _isEmpty = true;
    private bool _isSelected = false;
    private Color _defaultColor;
    private BuyerUI _buyerUI;

    public BuyerItem BuyerItem => _buyerItem;
    public bool IsSeleted => _isSelected;
    public event Action<BuyerSlot> OnSlotClicked;

    private void Awake()
    {
        _buyerItem = GetComponentInChildren<BuyerItem>();
        _defaultColor = GetComponent<Image>().color;
    }

    public void Add(BuyerUI buyerUI, InventorySlot sourceSlot, ItemData data, GameObject buyerItemPrefab, int count)
    {
        _buyerUI = buyerUI;
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

        // Больше не очищаем _sourceSlot здесь!
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isSelected = !_isSelected;
        GetComponent<Image>().color = _isSelected ? Color.green : _defaultColor;
        OnSlotClicked?.Invoke(this);
    }
}
