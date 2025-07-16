using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SellButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _sellClickSound;
    [SerializeField] private AudioClip _errorClickSound;

    private InventoryUI _inventoryUI;
    private ItemData _itemData;
    public void Init(ItemData itemData, InventoryUI inventoryUI)
    {
        _inventoryUI = inventoryUI;
        _itemData = itemData;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //if ()
        //{
        //    _audioSource.PlayOneShot(_sellClickSound);
        //}
        //else
        //{
        //    _audioSource.PlayOneShot(_errorClickSound);
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

}
