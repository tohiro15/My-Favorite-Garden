using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _buyClickSound;
    [SerializeField] private AudioClip _errorClickSound;

    private InventoryUI _inventoryUI;
    private ItemData _itemData;
    private TextMeshProUGUI _buyText;
    private TextMeshProUGUI _priceText;
    public void Init(ItemData itemData, InventoryUI inventoryUI, TextMeshProUGUI buyText, TextMeshProUGUI priceText)
    {
        _audioSource = GetComponent<AudioSource>();
        _inventoryUI = inventoryUI;
        _itemData = itemData;
        _buyText = buyText;
        _priceText = priceText;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerStatistic.Instance.Money >= _itemData.ItemPrice)
        {
            _audioSource.PlayOneShot(_buyClickSound);
        }
        else
        {
            _audioSource.PlayOneShot(_errorClickSound);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _priceText.gameObject.SetActive(false);
        _buyText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _priceText.gameObject.SetActive(true);
        _buyText.gameObject.SetActive(false);
    }
}
