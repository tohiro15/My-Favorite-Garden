using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private ItemData _itemData;
    private TextMeshProUGUI _buyText;
    private TextMeshProUGUI _priceText;
    public void Init(ItemData itemData, TextMeshProUGUI buyText, TextMeshProUGUI priceText)
    {
        _itemData = itemData;
        _buyText = buyText;
        _priceText = priceText;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (PlayerStatistic.Instance.Money >= _itemData.ItemPrice)
        {
            SoundManager.Instance.PlayBuyClick();
        }
        else
        {
            SoundManager.Instance.PlayErrorClick();
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
