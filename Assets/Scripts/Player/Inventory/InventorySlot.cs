using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Image _background;

    private Color _normalColor = Color.white;
    private Color _selectedColor = Color.yellow;

    private float _normalScale = 1f;
    private float _selectedScale = 1.2f;
    private float _tweenDuration = 0.15f;

    private InventoryUI _inventoryUI;
    private Item _item;
    private GameObject _itemPrefab;
    private int _slotIndex;
    private bool _isEmpty = true;
    public Item Item => _item;
    public bool IsEmpty => _isEmpty;
    private bool _isSelectable = true;
    private bool _isBackpackSlot = false;
    private bool _isSelected = false;

    private void Awake()
    {
        _item = GetComponentInChildren<Item>();
        if(_item != null ) _isEmpty = false; 
    }

    public void Initialization(GameObject itemPrefab, int index, InventoryUI parentUI, Color normalColor, Color selectedColor, float normalScale, float selectedScale, float tweenDuration, bool isSelectable = true)
    {
        _itemPrefab = itemPrefab;
        _slotIndex = index;
        _inventoryUI = parentUI;
        _isSelectable = isSelectable;
        _isBackpackSlot = !isSelectable;

        _normalColor = normalColor;
        _selectedColor = selectedColor;

        _normalScale = normalScale;
        _selectedScale = selectedScale;
        _tweenDuration = tweenDuration;

        _background.color = _normalColor;
        transform.localScale = Vector3.one * _normalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isSelectable) return;
        if (_isSelected) return;
        _inventoryUI.SelectSlot(_slotIndex);

        SoundManager.Instance.PlaySelectedSound();
    }

    public void Select()
    {
        _isSelected = true;
        _background.DOColor(_selectedColor, 0.2f);
        transform.DOScale(_selectedScale, 0.2f);

        if (!_isBackpackSlot && !_isEmpty)
            _item.SetSelected(true);

        SoundManager.Instance.PlaySelectedSound();
    }

    public void Deselect()
    {
        _isSelected = false;
        _background.DOColor(_normalColor, 0.2f);
        transform.DOScale(_normalScale, 0.2f);

        if (!_isBackpackSlot && !_isEmpty)
            _item.SetSelected(false);
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

            string keySlot = $"SlotFor_{_item.ItemData.name}";
            string keyCount = $"CountFor_{_item.ItemData.name}";
            PlayerPrefs.DeleteKey(keySlot);
            PlayerPrefs.DeleteKey(keyCount);
            PlayerPrefs.Save();

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

        string keySlot = _isSelectable
            ? $"SlotFor_{draggedItem.ItemData.name}"
            : $"BackpackSlotFor_{draggedItem.ItemData.name}";

        string keyCount = $"CountFor_{draggedItem.ItemData.name}";

        PlayerPrefs.SetInt(keySlot, _slotIndex);
        PlayerPrefs.SetInt(keyCount, draggedItem.ItemCount);
        PlayerPrefs.Save();

        draggedItem.transform.SetParent(transform);
        draggedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }


}
