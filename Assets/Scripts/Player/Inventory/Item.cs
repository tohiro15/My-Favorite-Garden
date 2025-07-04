using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _countText;

    private int _itemCount;
    private string PrefKey => $"ItemCount_{_itemData.name}";
    private void Awake()
    {
        _itemCount = PlayerPrefs.GetInt(PrefKey, _itemData.DefaultCount);
        UpdateUI();
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

    public void Remove(int amount)
    {
        _itemCount = Mathf.Clamp(_itemCount - amount, 0, _itemData.MaxStackSize);
        UpdateUI();
    }

    private void UpdateUI()
    {
        bool hasAny = _itemCount > 0;

        _iconImage.sprite = _itemData.Icon;

        _iconImage.gameObject.SetActive(hasAny);
        _countText.gameObject.SetActive(hasAny);

        if (hasAny) _countText.text = $"x{_itemCount}";
    }
}
