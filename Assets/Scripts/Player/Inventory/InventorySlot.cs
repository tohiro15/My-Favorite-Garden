using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private int _slotIndex;
    private Item _item;
    public Item Item => _item;

    private bool _isEmpty = true;
    public bool IsEmpty => _isEmpty;

    private void Awake()
    {
        _item = GetComponentInChildren<Item>();
        if(_item != null ) _isEmpty = false; 
    }
    public void Initialization(int index)
    {
        _slotIndex = index;
    }
    public void Add(ItemData itemData, int count)
    {
        if (_item == null)
        {
            var go = Instantiate(_itemPrefab, transform);
            _item = go.GetComponent<Item>();
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

        if (IsEmpty)
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

        string keySlot = $"SlotFor_{draggedItem.ItemData.name}";
        string keyCount = $"CountFor_{draggedItem.ItemData.name}";

        PlayerPrefs.SetInt(keySlot, _slotIndex);
        PlayerPrefs.SetInt(keyCount, draggedItem.ItemCount);
        PlayerPrefs.Save();

        draggedItem.transform.SetParent(transform);
        draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }


}
