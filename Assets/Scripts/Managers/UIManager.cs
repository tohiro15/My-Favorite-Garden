using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("HUD")]
    [Space]

    [SerializeField] private Canvas _HUDCanvas;
    [SerializeField] private TMP_Text _moneyCount;

    [Header("Shop")]
    [Space]

    [SerializeField] private Canvas _buyerCanvas;
    [SerializeField] private Button _exitShopButton;


    [Header("Seller")]
    [Space]

    [SerializeField] private Canvas _sellerCanvas;
    [SerializeField] private Button _exitSellerButton;


    [Header("Backpack")]
    [Space]

    [SerializeField] private GameObject _backpackPanel;
    [SerializeField] private Button _backpackButton;
    private bool _openBackpack = false;

    private void Start()
    {
        _HUDCanvas?.gameObject.SetActive(true);
        _buyerCanvas?.gameObject.SetActive(false);
        _sellerCanvas?.gameObject.SetActive(false);

        _exitShopButton?.onClick.RemoveAllListeners();
        _exitShopButton?.onClick.AddListener(CloseShopCanvas);

        _exitSellerButton?.onClick.RemoveAllListeners();
        _exitSellerButton?.onClick?.AddListener(CloseSellerCanvas);

        _backpackButton?.onClick.RemoveAllListeners();
        _backpackButton?.onClick.AddListener(OpenCloseBackpackPanel);
        _backpackPanel?.gameObject.SetActive(_openBackpack);

        PlayerStatistic.Instance.OnMoneyChanged += UpdateMoneyCount;

        UpdateMoneyCount(PlayerStatistic.Instance.Money);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)) OpenCloseBackpackPanel();
    }
    private void CloseShopCanvas()
    {
        _buyerCanvas?.gameObject.SetActive(false);
    }

    private void CloseSellerCanvas()
    {
        _sellerCanvas?.gameObject.SetActive(false);
    }

    private void OpenCloseBackpackPanel()
    {
        _openBackpack = !_openBackpack;
        _backpackPanel?.gameObject.SetActive(_openBackpack);
    }

    public void UpdateMoneyCount(int money)
    {
        var nfi = new NumberFormatInfo()
        {
            NumberGroupSeparator = ".",
            NumberDecimalDigits = 0
        };

        _moneyCount.text = money.ToString("N0", nfi);
    }
}
