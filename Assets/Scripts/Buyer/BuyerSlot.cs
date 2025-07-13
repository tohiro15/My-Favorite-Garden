using UnityEngine;
using UnityEngine.EventSystems;

public class BuyerSlot : MonoBehaviour
{
    private Inventory _inventory;
    private Item _item;
    private ItemData _itemData;
    private GameObject _itemPrefab;

    private int _count;
    private bool _isEmpty = true;
    public bool IsEmpty => _isEmpty;

    public void Initialization(ItemData itemData, Inventory inventory, GameObject itemPrefab, int count)
    {
        _inventory = inventory;
        _itemData = itemData;
        _itemPrefab = itemPrefab;
        _count = count;

        _item = _inventory.InventorySlots[count].Item;

        if (_item != null) _isEmpty = false;
    }

    public void Add(ItemData itemData, int count)
    {
        if (_item == null)
        {
            //_item = go.GetComponent<Item>();
        }
        _item.Change(itemData, count);
        _item.gameObject.SetActive(true);
        _isEmpty = false;
    }


    public void Clear()
    {
        if (_item != null)
        {
            Destroy(_item.gameObject);
            _item = null;
        }

        _isEmpty = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggedItem = eventData.pointerDrag?.GetComponent<Item>();
        if (draggedItem == null)
            return;

        var sourceSlot = draggedItem.OriginSlot;

        if (sourceSlot == this)
        {
            draggedItem.transform.SetParent(transform);
            draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            return;
        }

        if (_isEmpty)
        {
            Add(draggedItem.ItemData, draggedItem.ItemCount);
            sourceSlot.Clear();
        }
        else
        {
            var tmpData = _item.ItemData;
            var tmpCount = _item.ItemCount;

            Add(draggedItem.ItemData, draggedItem.ItemCount);
            sourceSlot.Add(tmpData, tmpCount);
        }

        draggedItem.transform.SetParent(transform);
        draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
