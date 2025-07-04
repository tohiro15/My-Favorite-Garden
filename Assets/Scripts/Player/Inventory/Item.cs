using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _countText;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Transform _originalParent;
    private InventorySlot _originSlot;

    private int _itemCount;
    public InventorySlot OriginSlot => _originSlot;
    private string PrefKey => $"ItemCount_{_itemData.name}";
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

        if (hasAny) _countText.text = $"x{_itemCount}";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalParent = transform.parent;
        _canvasGroup.blocksRaycasts = false;

        var nearest = GetComponentInParent<Canvas>();
        var rootCanvas = nearest.rootCanvas;

        transform.SetParent(rootCanvas.transform);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;

        transform.SetParent(_originalParent);
        _rectTransform.anchoredPosition = Vector2.zero;
    }

}
