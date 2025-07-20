using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private TextMeshProUGUI _itemNameText;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Transform _originalParent;
    private InventorySlot _originSlot;

    private bool _isSelected = false;
    private bool _isPointerOver = false;
    private int _itemCount;
    private string PrefKey => $"ItemCount_{_itemData.name}";
    public InventorySlot OriginSlot => _originSlot;
    public ItemData ItemData => _itemData;
    public int ItemCount => _itemCount;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();

        _itemCount = PlayerPrefs.GetInt(PrefKey, _itemData.DefaultCount);
        UpdateUI();
    }
    private void Start()
    {
        _originSlot = GetComponentInParent<InventorySlot>();
        _itemNameText.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)) Add(1);
        if (Input.GetKeyDown(KeyCode.C)) Remove(1);
    }
    private void OnDisable()
    {
        PlayerPrefs.SetInt(PrefKey, _itemCount);
        PlayerPrefs.Save();
    }

    public void Add(int amount)
    {
        _itemCount = Mathf.Clamp(_itemCount + amount, 0, _itemData.MaxStackSize);
        UpdateUI();
    }
    public void Change(ItemData itemData, int count)
    {
        _itemData = itemData;
        _itemCount = count;
        UpdateUI();
    }
    public void Remove(int amount)
    {
        _itemCount = Mathf.Clamp(_itemCount - amount, 0, _itemData.MaxStackSize);
        UpdateUI();

        if (_itemCount <= 0)
        {
            OriginSlot?.Clear();
        }
    }

    private void UpdateUI()
    {
        bool hasAny = _itemCount > 0;

        _iconImage.sprite = _itemData.Icon;

        if (hasAny)
        {
            _countText.text = $"x{_itemCount}";
            _itemData.ItemName.StringChanged += s => _itemNameText.text = s;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalParent = transform.parent;
        _canvasGroup.blocksRaycasts = false;

        var nearest = GetComponentInParent<Canvas>();
        var rootCanvas = nearest.rootCanvas;

        transform.SetParent(rootCanvas.transform);
        transform.SetAsLastSibling();

        SoundManager.Instance.PlayInventoryDropSound();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
        _itemNameText.gameObject.SetActive(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;

        transform.SetParent(_originalParent);
        _rectTransform.anchoredPosition = Vector2.zero;

        _itemNameText.gameObject.SetActive(false);
    }


    public void SetSelected(bool selected)
    {
        _isSelected = selected;
        _itemNameText.gameObject.SetActive(_isSelected || _isPointerOver);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerOver = true;
        _itemNameText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerOver = false;
        _itemNameText.gameObject.SetActive(_isSelected);
    }
}
